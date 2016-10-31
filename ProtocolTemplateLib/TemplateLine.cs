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
        public string Id { get
            {
                return Id_;
            }
            set
            {
                
                Id_ = value;
            }
        }
        public abstract Control GetEditControl();
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
        
        protected virtual void SetId(string value)
        {
        }
        protected abstract string GetGuiType();
        protected abstract string GetInfo(); 
        protected abstract void LoadFromXml(XmlNode node);
        protected const string NodeNameLine  = "line";
        protected const string NodeNameHeader = "header";
        private string Id_;
    }

    public class TemplateLine : TemplateItem
    {
        public string Label { get
            {
                return Label_;
            }
            set
            {
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

        public override Control GetEditControl()
        {
            LineControl control = new LineControl();
            control.Line = this;
            control.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            control.Margin = new Thickness(0);
            return control;
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
            return "Строка с полем: " + Field.GetTypeName();
        }

        protected override string GetInfo()
        {
            if (Label == null)
            {
                return "Нет подписи";
            }
            return Label;
        }

        protected override void SetId(string value)
        {
            if (Field_ != null)
            {
                Field_.Id = value;
            }
            base.SetId(value);
        }

        private string Label_;
        private Editable Field_;

        private const string AttributeNameLabel = "label";
    }

    public class TemplateHeader : TemplateItem
    {
        public string Header { get; set; }

        public override Control GetEditControl()
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
