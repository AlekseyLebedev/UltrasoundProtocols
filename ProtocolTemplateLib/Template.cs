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
        public List<TemplateItem> Items { get; }

        public Template()
        {
            Name = "";
            Items = new List<TemplateItem>();
        }

        public UIElement GetEditControl()
        {
            Grid grid = new Grid();
            grid.Margin = new System.Windows.Thickness(0);
            grid.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            double totalHeight = 0;
            foreach (TemplateItem item in Items)
            {
                UIElement element = item.GetEditControl();
                totalHeight += 5;
                if (element is Control)
                {
                    Control control = element as Control;
                    control.Margin = new Thickness(0, totalHeight, 0, 0);
                    control.VerticalAlignment = VerticalAlignment.Top;
                    totalHeight += control.Height;
                }
                else
                {
                    Grid childgrid = element as Grid;
                    childgrid.Margin = new Thickness(0, totalHeight, 0, 0);
                    childgrid.VerticalAlignment = VerticalAlignment.Top;
                    totalHeight += childgrid.Height;
                }
                grid.Children.Add(element);
            }
            return grid;

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
            writer.WriteStartElement(NodeNameTemplate);
            foreach (var item in Items)
            {
                item.SaveXml(writer);
            }
            writer.WriteEndElement();
        }

        public string SaveToXmlString()
        {
            StringBuilder result = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(result);
            writer.WriteStartDocument();
            SaveXml(writer);
            writer.WriteEndDocument();
            return result.ToString();
        }

        public static Template GetFromXml(XmlDocument document)
        {
            if (document.ChildNodes.Count != 2)
            {
                throw new XmlException("Wrong root node number");
            }

            var rootNodeName = document.ChildNodes[1].Name;
            if (rootNodeName != NodeNameTemplate)
            {
                throw new XmlException(String.Format("Wrong root node name. Expected {0}, found {1}", NodeNameTemplate, rootNodeName));
            }
            Template template = new Template();
            foreach (XmlNode node in document.ChildNodes[1].ChildNodes)
            {
                template.Items.Add(TemplateItem.GetFromXml(node));
            }
            return template;
        }

        private const string NodeNameTemplate = "template";

        private static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
