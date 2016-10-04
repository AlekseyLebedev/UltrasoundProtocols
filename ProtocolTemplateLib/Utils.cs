using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ProtocolTemplateLib
{
    internal class XmlUtils
    {
        internal static void AssertAttributeNotNull(XmlNode node, string parametrname)
        {
            if (node.Attributes[parametrname] == null)
            {
                throw new XmlException(string.Format("Error loading template. Attribute '{0}' is null in node '{1}'", parametrname, node.Name));
            }
        }

        internal static void AssertNodeName(XmlNode node, string correctName, bool check = false)
        {
            if (node.Name != correctName)
            {
                string message = String.Format("Wrong node name. Expected '{0}', actual '{1}'", correctName, node.Name);
                if (check)
                    throw new XmlException(message);
                else
                    throw new ArgumentException(message);
            }
        }
    }
}
