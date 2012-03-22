using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Unicoen.Apps.RefactoringDSL.Util;
using Unicoen.Languages.Java;
using Unicoen.Model;
using Unicoen.Tests;
using Unicoen.Apps.RefactoringDSL;
using Unicoen.Apps.RefactoringDSL.NamespaceDetector;

namespace Unicoen.Apps.RefactoringDSL.Tests.NamespaceDetector {
	public class ApplicationTest {
		private UnifiedProgram _model;

		[SetUp]
		public void SetUp() {
			var inputPath = FixtureUtil.GetInputPath("Java", "default", "Namespace.java");
			var code = File.ReadAllText(inputPath, Encoding.Default);
			_model = JavaFactory.GenerateModel(code);
		}

		[Test]
		public void 関数呼び出しの所属空間を特定できる() {
			var callNode = _model.FirstDescendant<UnifiedCall>();
			var belongingNamespace = Application.GetBelongingNamespace(callNode);
			Console.WriteLine(belongingNamespace.GetNamespaceString());
		}

		[Test]
		public void 自分の親を探して関数の宣言部分を取得できる() {
			var callNode = _model.FirstDescendant<UnifiedCall>();
			var definition = Application.FindDefinition(callNode, _model);
			Assert.That(definition != null);
			Assert.That(definition.Name.Name, Is.EqualTo(((UnifiedVariableIdentifier)callNode.Function).Name));
		}

		// 変数の宣言は，同一ブロック内にないといけないと判断した
		[Test]
		public void 自分の親を探して変数の宣言部分を探す() {
			// Expression 以下の VariableIdentifier を取り出す
			var viList = new List<UnifiedVariableIdentifier>();
			foreach (var unifiedVariableIdentifiers in _model.Descendants<UnifiedBinaryExpression>().Select(e => e.Descendants<UnifiedVariableIdentifier>())) {
				foreach (var uvi in unifiedVariableIdentifiers) {
					viList.Add(uvi);
				}
			}

			var target = viList.First();
			UnifiedVariableDefinition found = null;
			foreach (var node in target.FirstAncestor<UnifiedBlock>().Descendants()) {
				if (node is UnifiedVariableDefinition) {
					var vd = (UnifiedVariableDefinition)node;
					if (vd.Name.Name == target.Name) {
						found = vd;
						break;
					}
				}
			}

			if (found == null) {
				Console.WriteLine("Element not found");
			} else {
				Console.WriteLine(found.ToXml());
			}

			Console.WriteLine("terminated");

		}

		[Test]
		public void TestForFindDefinition()
		{
			var ids = _model.Descendants<UnifiedVariableIdentifier>();
			var id = ids.First();

			var def = FindDefinition(id, _model);
			if(def == null)
			{
				Console.WriteLine("aaaa");
			}
			Console.WriteLine(def);
		}

		public static UnifiedVariableDefinition FindDefinition(UnifiedVariableIdentifier identifier, UnifiedElement topNode)
		{
			UnifiedVariableDefinition found = null;
			foreach (var node in identifier.FirstAncestor<UnifiedBlock>().Descendants()) {
				if (node is UnifiedVariableDefinition) {
					var vd = (UnifiedVariableDefinition)node;
					if (vd.Name.Name == identifier.Name) {
						return vd;
					}
				}
			}

			return null;
		}




		public static IEnumerable<UnifiedElement> YieldParents(UnifiedElement node)
		{
			var n = node;
			while(n != null)
			{
				yield return n;
				n = (UnifiedElement)n.Parent;
			}
		}

			// ------------------------------------------ EXPERIMENTAL ------------------------------------------
		[Ignore]
		public void 関数が呼ばれている部分を探す() {
			var fdNode = _model.FirstDescendant<UnifiedFunctionDefinition>();
			var brothers = GetBrotherNode(fdNode);
			foreach (var brother in brothers) {
				var callNodes = brother.Descendants<UnifiedCall>();
			}




		}


		// 自分の兄弟ノード（自分も含む）を取得する
		public static IEnumerable<IUnifiedElement> GetBrotherNode(UnifiedElement node) {
			return node.FirstAncestor<IUnifiedElement>().Descendants();
		}

	}
}