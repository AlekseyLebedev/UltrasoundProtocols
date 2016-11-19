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
        private const string EXCEPTION_CONNECT_LOGGER_MESSAGE = "Error creating connection";
        private const string EXCEPTION_CLOSE_LOGGER_MESSAGE = "Error close connection";
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private SqlConnection Connection { get; set; }

        public SqlCommand Command { get; set; }

        public DataBaseSettings Settings { get; private set; }

        public DataBaseConnector(DataBaseSettings settings)
        {
            Settings = settings;
        }

        public string GetConnectionString()
        {
            return Settings.GetConnectionString();
        }

        public void CreateConnection()
        {
            try
            {
                Connection = new SqlConnection(Settings.GetConnectionString());
                Command = Connection.CreateCommand();
                Connection.Open();
            }
            catch (InvalidOperationException ex)
            {
                logger.Error(ex, EXCEPTION_CONNECT_LOGGER_MESSAGE);
                throw (ex);
            }
            catch (SqlException ex)
            {
                logger.Error(ex);
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, EXCEPTION_CONNECT_LOGGER_MESSAGE);
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
                logger.Error(ex, EXCEPTION_CLOSE_LOGGER_MESSAGE);
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, EXCEPTION_CLOSE_LOGGER_MESSAGE);
                throw (ex);
            }
        }

    }
}
