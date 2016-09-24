using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolTemplateLib
{
    public abstract class Editable
    {
        public bool EnableOtherField;
    }

    public class ComboboxEditable
    {
        public String[] Variants { get; set; }
    }
}
