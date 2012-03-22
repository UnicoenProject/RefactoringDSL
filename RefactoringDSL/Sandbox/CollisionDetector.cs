using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unicoen.Model;

namespace Unicoen.Apps.RefactoringDSL.Sandbox
{
	public class CollisionDetector
	{

		public static IEnumerable<UnifiedFunctionDefinition> FindFunctionCollision(UnifiedElement element, UnifiedFunctionDefinition target)
		{
			foreach (var function in element.Descendants<UnifiedFunctionDefinition>())
			{
				if (IsFunctionCollision(function, target))
				{
					yield return function;
				}

			}
		}


		// src と dst の衝突を調べて，衝突していれば src を，そうでなければ null を返す
		private static Boolean IsVariableCollision(UnifiedVariableDefinition src, UnifiedVariableDefinition dst)
		{
			var judges = new List<Boolean>();
			judges.Add(src.Name.Name == dst.Name.Name);

			if (judges.All(e => e))
			{
				return true;
			}
			return false;
		}

		private static Boolean IsFunctionCollision(UnifiedFunctionDefinition src, UnifiedFunctionDefinition dst)
		{
			// Judge whether src and dst have same signature
			var judges = new List<Boolean>();
			judges.Add(src.Name.Name == dst.Name.Name);
			judges.Add(src.Parameters.All(e => dst.Parameters.Any(e.Equals)));

			if (judges.All(e => e))
			{
				return true;
			}
			return false;
		}

	}
}
