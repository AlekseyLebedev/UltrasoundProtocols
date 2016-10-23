using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace ProtocolTemplateLib
{
    public abstract class TemplateItem : ITemplatePart
    {
        public string Id { get; set; }
        public abstract UIElement GetEditControl();
        public abstract string GetPartOfCreateTableScript();
        public abstract string PrintToProtocol(object value);
        public abstract string PrintToSaveQuery(object value);
        public abstract void SaveXml(XmlWriter writer);
        public abstract bool RequireValue();
        public static TemplateItem GetFromXml(XmlNode node)
        {
            TemplateItem result;
            switch (node.Name)
            {
                case NodeNameHeader:
                    result = new TemplateHeader();
                    break;
                case NodeNameLine:
                    result = new TemplateLine();
                    break;
                default:
                    throw new XmlException(String.Format("Wrong node name for TemplateItem. Not found '{0}' type.", node.Name));
            }
            result.LoadFromXml(node);
            return result;

        }

        public string Info
        {
            get
            {
                return GetInfo();
            }
        }

        public string Type
        {
            get
            {
                return GetGuiType();
            }
        }

        protected abstract string GetGuiType();
        protected abstract string GetInfo(); 
        protected abstract void LoadFromXml(XmlNode node);
        protected const string NodeNameLine  = "line";
        protected const string NodeNameHeader = "header";
    }

    public class TemplateLine : TemplateItem
    {
        public string Label { get
            {
                return Label_;
            }
            set
            {
                if (Field_ != null)
                {
                    Field_.Id = value;
                }
                Label_ = value;
            }
        }
        public Editable Field
        {
            get
            {
                return Field_;
            }
            set
            {
                Field_ = value;
                Field_.Id = Id;
            }
        }

        public override UIElement GetEditControl()
        {
            Grid grid = new Grid();
            grid.Margin = new System.Windows.Thickness(0);
            //grid.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            TextBlock label = new TextBlock();
            label.Text = Label;
            label.Margin = new Thickness(5, 0, 0, 0);
            label.VerticalAlignment = VerticalAlignment.Stretch;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.Width = 200;
            Control field = (Control)Field.GetEditControl();
            Thickness old = field.Margin;
            old.Left += label.Width + 2 * label.Margin.Left;
            field.Margin = old;
            grid.Children.Add(label);
            grid.Children.Add(field);
            return grid;

        }

        public override string GetPartOfCreateTableScript()
        {
            return Field.GetPartOfCreateTableScript();
        }

        public override string PrintToProtocol(object value)
        {
            throw new NotImplementedException();
        }

        public override void SaveXml(XmlWriter writer)
        {
            writer.WriteStartElement(NodeNameLine);
            writer.WriteAttributeString(AttributeNameLabel, Label);
            Field.SaveXml(writer);
            writer.WriteEndElement();
        }

        protected override void LoadFromXml(XmlNode node)
        {
            XmlUtils.AssertNodeName(node, NodeNameLine);
            XmlUtils.AssertAttributeNotNull(node, AttributeNameLabel);
            if (node.ChildNodes.Count != 1)
            {
                throw new XmlException("Line node has no info about editable");
            }
            Field = Editable.GetFromXml(node.ChildNodes[0]);
            Label = node.Attributes[AttributeNameLabel].Value;
        }

        public override string PrintToSaveQuery(object value)
        {
            return Field.PrintToSaveQuery(value);
        }

        public override bool RequireValue()
        {
            return true;
        }

        protected override string GetGuiType()
        {
            // TODO
            return "Исправить на типо контрола";
        }

        protected override string GetInfo()
        {
            // TODO
            return "TODO";
        }

        private string Label_;
        private Editable Field_;

        private const string AttributeNameLabel = "label";
    }

    public class TemplateHeader : TemplateItem
    {
        public string Header { get; set; }

        public override UIElement GetEditControl()
        {
            TextBox block = new TextBox();
            block.Text = Header;
            block.FontSize = 20;
            block.Margin = new Thickness(0);
            return block;
        }

        public override string GetPartOfCreateTableScript()
        {
            return null;
        }

        public override string PrintToProtocol(object value)
        {
            throw new NotImplementedException();
        }

        public override void SaveXml(XmlWriter writer)
        {
            writer.WriteStartElement(NodeNameHeader);
            writer.WriteAttributeString(AttributeNameLabel, Header);
            writer.WriteEndElement();
        }

        protected override void LoadFromXml(XmlNode node)
        {
            XmlUtils.AssertNodeName(node, NodeNameLine);
            XmlUtils.AssertAttributeNotNull(node, AttributeNameLabel);
            Header = node.Attributes[AttributeNameLabel].Value;
        }

        public override string PrintToSaveQuery(object value)
        {
            // Заголовки не сохраняются в БД
            throw new NotSupportedException();
        }

        public override bool RequireValue()
        {
            return false;
        }

        protected override string GetGuiType()
        {
            return "Заголовок";
        }

        protected override string GetInfo()
        {
            return Header;
        }

        private const string AttributeNameLabel = "label";
    }
}
