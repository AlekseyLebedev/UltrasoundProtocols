using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;

namespace ProtocolTemplateLib
{
    public abstract class Editable : ITemplatePart
    {
        public bool EnableOtherField { get; set; }

        public abstract void SaveXml(XmlWriter writer);

        protected abstract string PartOfCreateTableScript(string id);
        public string GetPartOfCreateTableScript(string id)
        {
            string result = PartOfCreateTableScript(id);
            if (EnableOtherField)
            {
                result += ", " + id + "_other nvarchar(255) NOT NULL";
            }
            return result;
        }

        public abstract Control GetEditControl();
        public abstract string PrintToProtocol(object value);
    }

    public class ComboboxEditable : Editable
    {
        public String[] Variants { get; set; }

        public override Control GetEditControl()
        {
            throw new NotImplementedException();
        }

        public override string PrintToProtocol(object value)
        {
            throw new NotImplementedException();
        }

        public override void SaveXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }

        protected override string PartOfCreateTableScript(string id)
        {
            throw new NotImplementedException();
        }
    }
    public class TextBoxEditable : Editable
    {
        public override Control GetEditControl()
        {
            throw new NotImplementedException();
        }

        public override string PrintToProtocol(object value)
        {
            throw new NotImplementedException();
        }

        public override void SaveXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }

        protected override string PartOfCreateTableScript(string id)
        {
            throw new NotImplementedException();
        }
    }
}
