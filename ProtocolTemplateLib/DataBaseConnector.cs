using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ProtocolTemplateLib
{
	class DataBaseConnector
	{
		private SqlConnection Connection { get; set; }

		public SqlCommand Command { get; set; }

		private DataBaseSettings dataBaseSettings;

		public DataBaseConnector(DataBaseSettings settings)
		{
			dataBaseSettings = settings;
		}

		public void CreateConnection()
		{
			try
			{
				Connection = new SqlConnection(dataBaseSettings.GetConnectionString());
				Command = Connection.CreateCommand();
				Connection.Open();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message + "Connection failed.");
			}
		}

		public void CloseConnection()
		{
			try
			{
				Connection.Close();
				Connection.Dispose();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message + "Connection was not closed.");
			}
		}

		//SqlCommand GetSqlCommand()
		//{
		//	return Command;
		//}

	}
}
