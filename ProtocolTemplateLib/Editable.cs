using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;

namespace ProtocolTemplateLib
{
    public abstract class Editable
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

        public abstract Control CreateEditControl(); 
    }

    public class ComboboxEditable
    {
        public String[] Variants { get; set; }
    }
    public class TextBoxEditable
    {
    }
}
