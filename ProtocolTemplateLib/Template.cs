using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using NLog;
using System.Windows;

namespace ProtocolTemplateLib
{
    public class Template : ITemplatePart
    {
        public string Name { get; set; }
        public string IdName { get; set; }
        public List<TemplateItem> Items { get; private set; }

        public Template()
        {
            Name = "";
            Items = new List<TemplateItem>();
        }

        private const int MarginBetweenControlsInEditControl = 5;

        public Control GetEditControl()
        {
            StackPanel panel = new StackPanel();
            panel.Margin = new System.Windows.Thickness(0);
            panel.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            panel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            foreach (TemplateItem item in Items)
            {
                Control control = item.GetEditControl();
                control.VerticalAlignment = VerticalAlignment.Top;
                control.Margin = new Thickness(0);
                panel.Children.Add(control);
            }
            ProtocolEditControl result = new ProtocolEditControl();
            result.SetContent(panel);
            return result;
        }

        public string GetPartOfCreateTableScript()
        {
            StringBuilder builder = new StringBuilder("CREATE TABLE ");
            builder.Append(IdName);
            builder.Append(" (");
            builder.AppendLine();
            builder.Append("id int");
            for (int i = 0; i < Items.Count; i++)
            {
                var part = Items[i].GetPartOfCreateTableScript();
                if (part != null)
                {
                    builder.AppendLine(", ");
                    builder.Append(part);
                }
            }
            builder.Append(" CONSTRAINT [");
            builder.Append(IdName);
            builder.Append(" ] PRIMARY KEY CLUSTERED (id)");
            builder.AppendLine(")");
            var result = builder.ToString();
            logger.Info("Create script: '{0}'", result);
            return result;
        }

        public void SaveXml(XmlWriter writer)
        {
            writer.WriteStartElement(NODE_NAME_TEMPLATE);
            writer.WriteAttributeString(NAME_ATTRIBUTE_NAME, Name);
            writer.WriteAttributeString(ID_NAME_ATTRIBUTE_NAME, IdName);
            foreach (var item in Items)
            {
                item.SaveXml(writer);
            }
            writer.WriteEndElement();
        }

        public string SaveToXmlString()
        {
            StringBuilder result = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(result, settings);
            writer.WriteStartDocument();
            SaveXml(writer);
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
            return result.ToString().Replace(" encoding=\"utf-16\"", "");
        }


        public static Template GetFromXml(XmlDocument document)
        {
            if (document.ChildNodes.Count != 2)
            {
                throw new XmlException("Wrong root node number");
            }

            var rootnode = document.ChildNodes[1];
            var rootNodeName = rootnode.Name;
            if (rootNodeName != NODE_NAME_TEMPLATE)
            {
                throw new XmlException(String.Format("Wrong root node name. Expected {0}, found {1}", NODE_NAME_TEMPLATE, rootNodeName));
            }
            Template template = new Template();
            if (rootnode.Attributes[NAME_ATTRIBUTE_NAME] == null)
            {
                throw new XmlException(String.Format(EXCEPTION_NO_ATTRIBUTE_MESSAGE, NAME_ATTRIBUTE_NAME));
            }
            else
            {
                template.Name = rootnode.Attributes[NAME_ATTRIBUTE_NAME].Value;
            }
            if (rootnode.Attributes[ID_NAME_ATTRIBUTE_NAME] == null)
            {
                throw new XmlException(String.Format(EXCEPTION_NO_ATTRIBUTE_MESSAGE, ID_NAME_ATTRIBUTE_NAME));
            }
            else
            {
                template.IdName = rootnode.Attributes[ID_NAME_ATTRIBUTE_NAME].Value;
            }
            foreach (XmlNode node in document.ChildNodes[1].ChildNodes)
            {
                template.Items.Add(TemplateItem.GetFromXml(node));
            }
            return template;
        }

        private const string NODE_NAME_TEMPLATE = "template";
        private const string NAME_ATTRIBUTE_NAME = "name";
        private const string ID_NAME_ATTRIBUTE_NAME = "id";
        private const string EXCEPTION_NO_ATTRIBUTE_MESSAGE = "Where is no attribute {0}";

        private static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
