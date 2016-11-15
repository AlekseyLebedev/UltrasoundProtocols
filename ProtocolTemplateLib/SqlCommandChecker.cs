using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolTemplateLib
{
	class SqlCommandChecker
	{
		public static bool checkSqlCommandForInjections(string command)
		{
			bool isChecked = true;
			string lowerCommand = command.ToLower();
			List<string> injections = new List<string>() { "drop", "delete" };

			foreach (string injection in injections)
			{
				if (lowerCommand.Contains(injection))
				{
					isChecked = false;
				}
			}

			return isChecked;
		}
	}
}
