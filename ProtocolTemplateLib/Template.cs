using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolTemplateLib
{
    public class Template
    {
        public string Name { get; set; }
        public List<TemplateItem> Items { get; }

        public Template()
        {
            Name = "";
            Items = new List<TemplateItem>();
        }
    }
}
