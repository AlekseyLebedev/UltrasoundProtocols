using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ProtocolTemplateLib
{
    public interface ITemplatePart
    {
        Control GetEditControl();

        String PrintToProtocol(Object value);

        string GetPartOfCreateTableScript(string id);

    }
}
