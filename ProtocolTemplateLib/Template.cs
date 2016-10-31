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
            Grid grid = new Grid();
            grid.Margin = new System.Windows.Thickness(0);
            grid.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            List<Control> controls = new List<Control>();
            foreach (TemplateItem item in Items)
            {
                Control control = item.GetEditControl();
                control.VerticalAlignment = VerticalAlignment.Top;
                control.Margin = new Thickness(0);
                grid.Children.Add(control);
                controls.Add(control);
            }
            grid.SizeChanged += new SizeChangedEventHandler((Object, SizeChangedEventArgs) =>
            {
                for (int i = 1; i < controls.Count; i++)
                {
                    controls[i].Margin = new Thickness(0, controls[i - 1].Margin.Top + controls[i - 1].ActualHeight +
                        MarginBetweenControlsInEditControl, 0, 0);
                }
            });
            ProtocolEditControl result = new ProtocolEditControl();
            result.SetContent(grid);
            return result;

        }

        private void LastControl_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            throw new NotImplementedException();
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
