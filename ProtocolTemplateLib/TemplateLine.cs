using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolTemplateLib
{
    public abstract class TemplateItem
    {

    }

    public class TemplateLine : TemplateItem
    {
        public string Lebel { get; set; }
        public Editable Field { get; set; }

    }

    public class TemplateHeader : TemplateItem
    {
        public string Header { get; set; }
    }
}
