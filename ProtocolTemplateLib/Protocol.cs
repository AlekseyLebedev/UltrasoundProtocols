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

        private static string INSERT_INTO = "INSERT INTO";
        private static string SELECT_ALL = "SELECT * FROM";
        private static string SPACE = " ";
        private static string OPEN_BRACKET = "(";
        private static string WHERE_ID = "WHERE id=";
        private static string CLOSE_BRACKET = ")";
        private static string VALUES = "VALUES";
        private static string COMMA = ",";
        private static string SEMICOLON = ";";

        public Template TemplateInstance { get; private set; }
        public Protocol(Template template)
        {
            TemplateInstance = template;
            Fields = new List<ProtocolField>();
            foreach (var item in template.Items)
            {
                if (item.RequireValue())
                {
                    Fields.Add(item.CreateFieldIntance());
                }
            }
        }

        private const string UnsupportedItemTypeExceptionMessage = "Unsopported type of template item";

        public void PrintToProtocol(StringBuilder builder)
        {
            int fieldIndex = 0;
            foreach (var item in TemplateInstance.Items)
            {
                item.PrintToProtocol(builder, (item.RequireValue() ? Fields[fieldIndex++] : null));
            }
        }

        public void SaveToDatabase(int ProtocolId, DataBaseConnector connector)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connector.Connection;
            StringBuilder builder = new StringBuilder(INSERT_INTO);
            builder.Append(SPACE).Append("@TableId").Append(SPACE).Append(VALUES).Append(OPEN_BRACKET);
            command.Parameters.AddWithValue("@TableId", TemplateInstance.IdName);
            builder.Append("@ProtocolId").Append(COMMA);
            command.Parameters.AddWithValue("@ProtocolId", ProtocolId);

            StringBuilder insertLine = new StringBuilder();
            for (int index = 0; index < Fields.Count; ++index)
            {
                insertLine.Append("@Value" + index);
                if (index < Fields.Count - 1)
                {
                    insertLine.Append(COMMA);
                }
                command.Parameters.AddWithValue("@Value" + index, Fields[index].AddToSaveRequest());
            }
            logger.Info("Added to " + TemplateInstance.IdName + " record = " + insertLine.ToString());
            builder.Append(insertLine).Append(CLOSE_BRACKET).Append(SEMICOLON);

            command.CommandText = builder.ToString();

            command.ExecuteNonQuery();
        }

        public void LoadFromDatabase(int ProtocolId, DataBaseConnector connector)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connector.Connection;
            StringBuilder builder = new StringBuilder(SELECT_ALL).Append(SPACE).Append("@TableId").Append(SPACE)
                .AppendLine(WHERE_ID).Append("@ProtocolId").Append(SEMICOLON);
            command.Parameters.AddWithValue("@TableId", TemplateInstance.IdName);
            command.Parameters.AddWithValue("@ProtocolId", ProtocolId);
            command.CommandText = builder.ToString();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                int fieldIndex = 0;
                for (int i = 0; i < reader.FieldCount; ++i)
                {
                    int fieldSize = Fields[fieldIndex].GetFieldCount();
                    String[] field = new String[fieldSize];
                    for (int j = 0; j < fieldSize; ++j, i++)
                    {
                        field[j] = reader[i].ToString();
                    }
                    Fields[fieldIndex++].GetFromRequest(field);
                }
                logger.Info("Loaded protocol from " + TemplateInstance.IdName + " by id = " + ProtocolId);
            }
        }

        public Protocol LoadProtocol(Template template, int id, DataBaseConnector connector)
        {
            Protocol protocol = new Protocol(template);
            protocol.LoadFromDatabase(id, connector);
            return protocol;
        }

        private List<ProtocolField> Fields;
    }
}
