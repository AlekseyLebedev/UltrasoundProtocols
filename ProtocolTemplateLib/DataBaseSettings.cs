﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolTemplateLib
{
	public class DataBaseSettings
	{

		public string GetConnectionString()
		{
			return String.Format(connectionPropertiesString, DataSource, Login, Password);
		}

		private string Login { get; set; }

		private string Password { get; set; }

		private string DataSource { get; set; }//"VALERIYPC\\SQLEXPRESS"

		private const string connectionPropertiesString =
			"Data Source='{0}'; Integrated Security=SSPI; Initial Catalog=UltraSoundProtocolsDB; User='{1}'; Password='{2}'";

	}
}
