using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace ProtocolTemplateLib
{
    public abstract class Editable : ITemplatePart
    {
        public bool EnableOtherField { get; set; }

        public string Id { get; set; }

        public abstract void SaveXml(XmlWriter writer);

        protected abstract string PartOfCreateTableScript();

        public string GetPartOfCreateTableScript()
        {
            string result = PartOfCreateTableScript();
            if (EnableOtherField)
            {
                result += ", " + Id + "_other nvarchar(255) NOT NULL";
            }
            return result;
        }

        public static Editable GetFromXml(XmlNode node)
        {
            Editable result;
            switch (node.Name)
            {
                case NodeNameComboBox:
                    result = new ComboboxEditable();
                    break;
                case NodeNameTextBox:
                    result = new TextBoxEditable();
                    break;
                default:
                    throw new XmlException(String.Format("Wrong node name for Editable. Not found '{0}' type.", node.Name));
            }
            result.LoadFromXml(node);
            return result;
        }

        public abstract object GetValueFromControl();
        public abstract object SetValueToControl(Object value);
        public abstract UIElement GetEditControl();
        public abstract string PrintToProtocol(object value);
        public abstract string PrintToSaveQuery(object value);

        protected static void LocateControlStandart(Control control)
        {
            control.VerticalAlignment = VerticalAlignment.Top;
            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            control.Margin = new Thickness(5);
        }

        protected void SaveOtherEnabled(XmlWriter writer)
        {
            writer.WriteAttributeString(AttributeNameOtherEnabled, EnableOtherField.ToString());
        }

        protected void LoadOtherEnabled(XmlNode node)
        {
            XmlUtils.AssertAttributeNotNull(node, AttributeNameOtherEnabled);
            try
            {
                EnableOtherField = Boolean.Parse(node.Attributes[AttributeNameOtherEnabled].Value);
            }
            catch (FormatException ex)
            {
                throw new XmlException(string.Format("Error loading template. Attribute '{0}' is not boolean in node '{1}'", AttributeNameOtherEnabled, node.Name), ex);
            }
        }

        protected abstract void LoadFromXml(XmlNode node);

        protected const string AttributeNameOtherEnabled = "other";
        protected const string NodeNameComboBox = "combobox";
        protected const string NodeNameTextBox = "textbox";

    }

    public class ComboboxEditable : Editable
    {
        private const string NodeNameVariant = "variant";
        private const string AttributeNameValue = "value";

        public String[] Variants { get; set; }

        public override UIElement GetEditControl()
        {
            ComboBox control = new ComboBox();
            LocateControlStandart(control);
            foreach (var item in Variants)
            {
                control.Items.Add(item);
            }
            control.IsEditable = EnableOtherField;
            return control;
        }

        public override string PrintToProtocol(object value)
        {
            throw new NotImplementedException();
        }

        public override void SaveXml(XmlWriter writer)
        {
            writer.WriteStartElement(NodeNameComboBox);
            SaveOtherEnabled(writer);
            foreach (var item in Variants)
            {
                writer.WriteStartAttribute(NodeNameVariant);
                writer.WriteAttributeString(AttributeNameValue, item);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        protected override string PartOfCreateTableScript()
        {
            return Id + " int";
        }

        protected override void LoadFromXml(XmlNode node)
        {
            XmlUtils.AssertNodeName(node, NodeNameComboBox);
            LoadOtherEnabled(node);
            List<string> variants = new List<string>();
            foreach (XmlNode item in node.ChildNodes)
            {
                XmlUtils.AssertNodeName(item, NodeNameVariant, true);
                XmlUtils.AssertAttributeNotNull(item, AttributeNameValue);
                variants.Add(item.Attributes[AttributeNameValue].Value);
            }
            Variants = variants.ToArray();
        }

        public override string PrintToSaveQuery(object value)
        {
            return (EnableOtherField ? (string)value : ((int)value).ToString());
        }

        public override object GetValueFromControl()
        {
            throw new NotImplementedException();
        }

        public override object SetValueToControl(Object value)
        {
            throw new NotImplementedException();
        }
    }
    public class TextBoxEditable : Editable
    {
        public override UIElement GetEditControl()
        {
            TextBox control = new TextBox();
            LocateControlStandart(control);
            return control;
        }

        public override string PrintToProtocol(object value)
        {
            throw new NotImplementedException();
        }

        public override void SaveXml(XmlWriter writer)
        {
            writer.WriteStartElement(NodeNameTextBox);
            writer.WriteEndElement();
        }

        protected override void LoadFromXml(XmlNode node)
        {
            // No properties
            XmlUtils.AssertNodeName(node, NodeNameTextBox);
        }

        protected override string PartOfCreateTableScript()
        {
            return Id + " nvarchar(1024)";
        }

        public override string PrintToSaveQuery(object value)
        {
            return (string)value;
        }

        public override object GetValueFromControl()
        {
            throw new NotImplementedException();
        }

        public override object SetValueToControl(Object value)
        {
            throw new NotImplementedException();
        }
    }
}
