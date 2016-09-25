using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using NLog;

namespace ProtocolTemplateLib
{
    public class Template : ITemplatePart
    {
        public string Name { get; set; }
        public string IdName { get; set; }
        public List<TemplateItem> Items { get; }

        public Template()
        {
            Name = "";
            Items = new List<TemplateItem>();
        }

        public Control GetEditControl()
        {
            throw new NotImplementedException();
        }

        public string PrintToProtocol(object value)
        {
            throw new NotImplementedException();
        }

        public string GetPartOfCreateTableScript(string id)
        {
            StringBuilder builder = new StringBuilder("CREATE TABLE ");
            builder.Append(IdName);
            builder.Append(" (");
            builder.AppendLine();
            builder.Append("id int");
            for (int i = 0; i < Items.Count; i++)
            {
                builder.AppendLine(", ");
                builder.Append(Items[i].GetPartOfCreateTableScript(null));
            }
            builder.Append(" CONSTRAINT [PK_tbltest] PRIMARY KEY CLUSTERED (id)");
            builder.AppendLine(")");
            var result = builder.ToString();
            logger.Debug("Create script: '{0}'", result);
            return result;
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
