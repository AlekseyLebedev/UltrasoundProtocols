using NLog;
using ProtocolTemplateLib;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProtocolTemplateRedactor
{
    class DatabaseController
    {
        private DataBaseConnector Connector;
        private Logger Logger = LogManager.GetCurrentClassLogger();

        internal DatabaseController(DataBaseConnector connector)
        {
            Connector = connector;
        }

        internal void DeleteTemplate(Template template)
        {
            var idName = template.IdName;
            var name = template.Name;
            Logger.Debug("Delete selected template id '{0}' name '{1}'", idName, name);
            using (var adapter = new TemplatesDataSetTableAdapters.Tbl_TemplatesTableAdapter(Connector.Settings))
            {
                TemplatesDataSet.Tbl_TemplatesDataTable table = adapter.GetData();
                adapter.Delete(idName, template.Name);
                SqlCommand command = new SqlCommand();
                command.Connection = Connector.Connection;
                command.CommandText = String.Format("USE [UltraSoundProtocolsDB]{0}DROP TABLE {1}", Environment.NewLine, idName);
                command.ExecuteNonQuery();
            }
        }

        internal bool SaveTemplate(Template template, bool force)
        {
            var idName = template.IdName;
            var name = template.Name;
            Logger.Info("Saving tamplte id '{0}' name '{1}'", idName, name);
            using (var adapter = new TemplatesDataSetTableAdapters.Tbl_TemplatesTableAdapter(Connector.Settings))
            {
                TemplatesDataSet.Tbl_TemplatesDataTable table = adapter.GetData();
                var templates = from r in table select r;

                var alreadyContainsRow = templates.FirstOrDefault(x => (x.tem_id == idName));
                bool containsId = alreadyContainsRow != null;
                Logger.Info("Find id: {0}", containsId);
                if (containsId && (!force))
                {
                    return false;
                }

                string xml = template.SaveToXmlString();
                Logger.Debug("Xml: {0}", xml);
                var request = template.GetCreateTableScript();
                SqlCommand command = new SqlCommand();
                command.Connection = Connector.Connection;
                if (containsId)
                {
                    alreadyContainsRow.tem_name = name;
                    alreadyContainsRow.tem_template = xml;
                    adapter.Update(alreadyContainsRow);
                    command.CommandText = String.Format("USE [UltraSoundProtocolsDB]{1}{0}{1}DROP TABLE {2}", request, Environment.NewLine, idName);
                }
                else
                {
                    adapter.Insert(idName, name, xml);
                    command.CommandText = request;
                }
                command.ExecuteNonQuery();

                return true;
            }
        }

        internal List<Template> LoadAllTemplates()
        {
            Logger.Debug("Loading templates");
            try
            {
                using (var adpater = new TemplatesDataSetTableAdapters.Tbl_TemplatesTableAdapter(Connector.Settings))
                {
                    TemplatesDataSet.Tbl_TemplatesDataTable table = adpater.GetData();
                    return (from row in table select Template.GetFromDatabaseEntry(row.tem_name, row.tem_id, row.tem_template)).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error Loading templates");
                throw ex;
            }
        }
    }
}
