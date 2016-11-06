using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ProtocolTemplateLib
{
    public abstract class ProtocolField
    {
        private Control UIControl;

        public abstract void GetFromRequest(string value);
        public abstract string AddToSaveRequest();
    }
}
