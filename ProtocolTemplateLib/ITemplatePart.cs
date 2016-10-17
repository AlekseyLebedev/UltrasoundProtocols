using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace ProtocolTemplateLib
{
    public interface ITemplatePart
    {
        UIElement GetEditControl();
        string GetPartOfCreateTableScript();

        void SaveXml(XmlWriter writer);
    }
}
