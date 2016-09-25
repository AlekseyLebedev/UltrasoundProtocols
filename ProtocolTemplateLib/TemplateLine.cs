using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ProtocolTemplateLib
{
    public abstract class TemplateItem : ITemplatePart
    {
        public string Id { get; set; }
        public abstract Control GetEditControl();
        public abstract string GetPartOfCreateTableScript(string id);
        public abstract string PrintToProtocol(object value);
    }

    public class TemplateLine : TemplateItem
    {
        public string Lebel { get; set; }
        public Editable Field { get; set; }

        public override Control GetEditControl()
        {
            throw new NotImplementedException();
        }

        public override string GetPartOfCreateTableScript(string id)
        {
            return Field.GetPartOfCreateTableScript(Id);
        }

        public override string PrintToProtocol(object value)
        {
            throw new NotImplementedException();
        }
    }

    public class TemplateHeader : TemplateItem
    {
        public string Header { get; set; }

        public override Control GetEditControl()
        {
            throw new NotImplementedException();
        }

        public override string GetPartOfCreateTableScript(string id)
        {
            throw new NotImplementedException();
        }

        public override string PrintToProtocol(object value)
        {
            throw new NotImplementedException();
        }
    }
}
