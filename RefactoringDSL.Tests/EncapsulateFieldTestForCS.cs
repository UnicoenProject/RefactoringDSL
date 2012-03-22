using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Unicoen.Languages.CSharp;
using Unicoen.Languages.Java;
using Unicoen.Model;
using Unicoen.Tests;

namespace Unicoen.Apps.RefactoringDSL.Tests
{
	public class EncapsulateFieldTestForCS
	{
		private UnifiedProgram _model;

		[SetUp]
		public void SetUp()
		{
			var inputPath = FixtureUtil.GetInputPath("CSharp", "EncapsulateField.cs");
			var code = File.ReadAllText(inputPath, Encoding.Default);
			_model = CSharpFactory.GenerateModel(code);
		}

		[Test]
		public void 読み込みテスト()
		{
			Console.WriteLine(CSharpFactory.GenerateCode(_model));
		}

		[Test]
		public void リファクタリングする()
		{
			var enc = new EncapsulateField(_model);
			const string targetClassName = "iiiii";
			var refactored = enc.Refactor(targetClassName);
			Console.WriteLine(CSharpFactory.GenerateCode(refactored));
		}
	}
}