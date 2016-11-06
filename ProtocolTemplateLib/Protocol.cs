using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using System.Data.SqlClient;

namespace ProtocolTemplateLib
{
    public class Protocol
    {

		private static Logger logger = LogManager.GetCurrentClassLogger();

		private static string login = "val_guest";
		private static string password = "hf94hd78";

		private DataBaseConnector dataBaseConnector;
		
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
            Fields = new List<ProtocolField>();
            foreach (var item in template.Items)
            {
                if (item.RequireValue())
                {
                    Fields.Add(item.GetFieldIntance());
                }
            }
            Values = new Object[valuableItems.Count];
            ValuableTemplateItems = valuableItems.ToArray();
			dataBaseConnector = new DataBaseConnector(new DataBaseSettings());
			dataBaseConnector.CreateConnection();
		}

		//public bool ChangeRequestNewUserDataBase(string login, string password)
		//{
		//	if (false/*TODO if connection to database closed*/)
		//	{
		//		SetLoginUserDataBase(login);
		//		SetPasswordUserDataBase(password);
		//		UpdateConnectionString();
		//		return true;
		//	}
		//	else
		//	{
		//		return false;
		//	}
		//}

        private const string UnsupportedItemTypeExceptionMessage = "Unsopported type of template item";

        public string GetPartOfHtmlProtocol()
        {
            throw new NotImplementedException();
        }
		
        public void SaveToDatabase(int ProtocolId)
        {
			string tableName = TemplateInstance.IdName; 
            StringBuilder builder = new StringBuilder(INSERT_INTO);
			builder.Append(SPACE).Append(tableName).Append(VALUES).Append(OPEN_BRACKET);
			StringBuilder insertLine = new StringBuilder();
			for (int index = 0; index < Fields.Count; ++index)
            {
				insertLine.Append(Fields[index].AddToSaveRequest());
				if (index < Fields.Count)
				{
					insertLine.Append(COMMA);
				}
            }
			logger.Info("Added to " + tableName + " record = " + insertLine.ToString());
			builder.Append(insertLine).Append(CLOSE_BRACKET).Append(SEMICOLON);

			dataBaseConnector.Command.CommandText = builder.ToString();
			dataBaseConnector.Command.ExecuteNonQuery();
		}

        public void LoadFromDatabase(int ProtocolId, SqlCommand command)
        {
            string tableName = TemplateInstance.IdName;
            StringBuilder builder = new StringBuilder(SELECT_ALL)
				.Append(tableName)
				.AppendLine(WHERE_ID)
				.Append(ProtocolId)
				.Append(SEMICOLON);
			command.CommandText = builder.ToString();

			using (SqlDataReader reader = command.ExecuteReader())
			{
				for (int i = 0; i < reader.FieldCount - 1; ++i)
				{
                    //TODO for Velera
					Fields[i].GetFromRequest(reader[i].ToString());
				}
			}
		}

        public Protocol LoadProtocol(Template template, int id)
        {
            Protocol protocol = new Protocol(template);
			
			//TODO вынести отдельно (после обсуждения)
			try {
				protocol.LoadFromDatabase(id, dataBaseConnector.Command);
			}
			catch (Exception e) {
				Console.WriteLine(e.Message + " я ввел: " + login + ", но где то накосячил");
			}
			return protocol;
        }

        private List<ProtocolField> Fields;
    }
}
