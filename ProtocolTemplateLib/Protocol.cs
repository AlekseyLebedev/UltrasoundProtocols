using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using NLog;

namespace ProtocolTemplateLib
{
    public class Protocol
    {

		private static Logger logger = LogManager.GetCurrentClassLogger();

		private static string login = "val_guest";
		private static string password = "hf94hd78";
		private static StringBuilder connectionString;
		private const string connectionPropertiesString =
			"Data Source=VALERIYPC\\SQLEXPRESS; Integrated Security=SSPI; Initial Catalog=UltraSoundProtocolsDB; User='{0}'; Password='{1}'";

		private static string INSERT_INTO = "INSERT INTO";
		private static string SELECT_ALL = "SELECT * FROM";
		private static string SPACE = " ";
		private static string OPEN_BRACKET = "(";
		private static string WHERE_ID = "WHERE id=";
		private static string CLOSE_BRACKET = ")";
		private static string VALUES = "VALUES";
		private static string COMMA = ",";
		private static string SEMICOLON = ";";
		private static string QUOTE = "'";
		private static string DOT = ".";

		public Template TemplateInstance { get; private set; }
        public Protocol(Template template)
        {
            TemplateInstance = template;
            List<TemplateItem> valuableItems = new List<TemplateItem>();
            foreach (var item in template.Items)
            {
                if (item.RequireValue())
                {
                    valuableItems.Add(item);
                }
            }
            Values = new Object[valuableItems.Count];
            ValuableTemplateItems = valuableItems.ToArray();
			connectionString = new StringBuilder(String.Format(connectionPropertiesString, Protocol.login, Protocol.password));
		}

		public bool ChangeRequestNewUserDataBase(string login, string password)
		{
			if (false/*TODO if connection to database closed*/)
			{
				SetLoginUserDataBase(login);
				SetPasswordUserDataBase(password);
				UpdateConnectionString();
				return true;
			}
			else
			{
				return false;
			}
		}

		private void UpdateConnectionString()
		{
			string connectionString = String.Format(connectionPropertiesString, Protocol.login, Protocol.password);
			Protocol.connectionString = new StringBuilder(connectionString);
			logger.Info("Connection string is changed.");
		}

		private void SetLoginUserDataBase(string newLogin)
		{
			if (newLogin != null)
			{
				Protocol.login = newLogin;
				logger.Info("User login is successfully changed.");
			}
		}

		private void SetPasswordUserDataBase(string newPassword)
		{
			if (newPassword != null)
			{
				Protocol.password = newPassword;
				logger.Info("User password is successfully changed.");
			}
		}

		private void SetConnectString(string newConnectString)
		{
			if (newConnectString != null)
			{
				Protocol.connectionString = new StringBuilder(newConnectString);
			}
		}

        private const string UnsupportedItemTypeExceptionMessage = "Unsopported type of template item";

        public void GetFromGui()
        {
            for (int i = 0; i < ValuableTemplateItems.Length; i++)
            {
                if (ValuableTemplateItems[i] is TemplateLine)
                {
                    Values[i] = ((TemplateLine)ValuableTemplateItems[i]).Field.GetValueFromControl();
                }
                else
                {
                    throw new ArithmeticException(UnsupportedItemTypeExceptionMessage);
                }
            }
        }

        public void SetValuesToGui()
        {
            for (int i = 0; i < ValuableTemplateItems.Length; i++)
            {
                if (ValuableTemplateItems[i] is TemplateLine)
                {
                    ((TemplateLine)ValuableTemplateItems[i]).Field.SetValueToControl(Values[i]);
                }
                else
                {
                    throw new ArithmeticException(UnsupportedItemTypeExceptionMessage);
                }
            }
        }


        public string GetPartOfHtmlProtocol()
        {
            throw new NotImplementedException();
        }
		
        public void SaveToDatabase(int ProtocolId, SqlCommand insertCommand /*, BD argument*/)
        {
			string tableName = "Tbl_doctors";//TODO getTableName(id)
			StringBuilder builder = new StringBuilder(INSERT_INTO);
			builder.Append(SPACE).Append(tableName).Append(VALUES).Append(OPEN_BRACKET);
			StringBuilder insertLine = new StringBuilder();
			for (int index = 0; index < ValuableTemplateItems.Length; ++index)
            {
				insertLine.Append(ValuableTemplateItems[index].PrintToSaveQuery(Values[index]));
				if (index < ValuableTemplateItems.Length)
				{
					insertLine.Append(COMMA);
				}
            }
			logger.Info("Added to " + tableName + " record = " + insertLine.ToString());
			builder.Append(insertLine).Append(CLOSE_BRACKET).Append(SEMICOLON);
			insertCommand.CommandText = builder.ToString();

			insertCommand.ExecuteNonQuery();
		}

        public void LoadFromDatabase(int ProtocolId, SqlCommand command)
        {
            //throw new NotImplementedException();
			int itemsIndex = 0;

			string tableName = "Tbl_doctors";//TODO getTableName(id)
            StringBuilder builder = new StringBuilder(SELECT_ALL)
				.Append(tableName)
				.AppendLine(WHERE_ID)
				.Append(ProtocolId)
				.Append(SEMICOLON);
			command.CommandText = builder.ToString();
			// TODO
			foreach (var item in ValuableTemplateItems)
            {
                // TODO: 
                // Ask for interface
            }
            // TODO
        }

        public static Protocol LoadProtocol(Template template, int id/*, DB argument*/)
        {
            Protocol protocol = new Protocol(template);

			//TODO вынести отдельно (после обсуждения)
			try {
				SqlConnection connection = new SqlConnection(Protocol.connectionString.ToString());
				SqlCommand command = connection.CreateCommand();
				connection.Open();

				protocol.LoadFromDatabase(id, command/*, bb*/);

				connection.Close();
				connection.Dispose();
			}
			catch (Exception e) {
				Console.WriteLine(e.Message + " я ввел: " + login + ", но где то накосячил");
			}

			return protocol;
        }

        private Object[] Values;
        private TemplateItem[] ValuableTemplateItems;
    }
}
