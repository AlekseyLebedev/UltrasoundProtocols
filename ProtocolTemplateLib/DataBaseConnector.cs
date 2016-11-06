using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NLog;

namespace ProtocolTemplateLib
{
	public class DataBaseConnector
	{

		private static Logger logger = LogManager.GetCurrentClassLogger();

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
			catch (InvalidOperationException ex)
			{
				logger.Error(ex);
				throw (ex);
			}
			catch (SqlException ex)
			{
				logger.Error(ex);
				throw (ex);
			}
			catch (Exception ex)
			{
				logger.Fatal(ex);
				throw (ex);
			}
		}

		public void CloseConnection()
		{
			try
			{
				Connection.Close();
				Connection.Dispose();
			}
			catch (SqlException ex)
			{
				logger.Error(ex);
				throw (ex);
			}
			catch (Exception ex)
			{
				logger.Fatal(ex);
				throw (ex);
			}
		}

	}
}
