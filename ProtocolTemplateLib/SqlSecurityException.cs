using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolTemplateLib
{
	public class SqlSecurityException : Exception
	{
		public SqlSecurityException()
		{

		}

		public SqlSecurityException(string message) : base(message)
		{

		}

		public SqlSecurityException(string message, Exception inner) : base(message, inner)
		{

		}

	}
}
