using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolTemplateLib
{
    public class Protocol
    {
        public Template TemplateInstance { get; private set; }
        public Protocol(Template template)
        {
            TemplateInstance = template;
            List<TemplateItem> valuableItems = new List<TemplateItem>();
            foreach (var item in template.Items)
            {
                if (item.RequireValue())
                {
                    valuableItems.Add(item);
                }
            }
            Values = new Object[valuableItems.Count];
            ValuableTemplateItems = valuableItems.ToArray();
        }

        private const string UnsupportedItemTypeExceptionMessage = "Unsopported type of template item";

        public void GetFromGui()
        {
            for (int i = 0; i < ValuableTemplateItems.Length; i++)
            {
                if (ValuableTemplateItems[i] is TemplateLine)
                {
                    Values[i] = ((TemplateLine)ValuableTemplateItems[i]).Field.GetValueFromControl();
                }
                else
                {
                    throw new ArithmeticException(UnsupportedItemTypeExceptionMessage);
                }
            }
        }

        public void SetValuesToGui()
        {
            for (int i = 0; i < ValuableTemplateItems.Length; i++)
            {
                if (ValuableTemplateItems[i] is TemplateLine)
                {
                    ((TemplateLine)ValuableTemplateItems[i]).Field.SetValueToControl(Values[i]);
                }
                else
                {
                    throw new ArithmeticException(UnsupportedItemTypeExceptionMessage);
                }
            }
        }


        public string GetPartOfHtmlProtocol()
        {
            throw new NotImplementedException();
        }

        // TODO for Ivan
        public void SaveToDatabase(int ProtocolId /*, BD argument*/)
        {
            throw new NotImplementedException();
            int itemsIndex = 0;
            StringBuilder builder = new StringBuilder("insert...");
            // TODO
            foreach (var item in ValuableTemplateItems)
            {
                // TODO handle result and work with it
                item.PrintToSaveQuery(Values[itemsIndex++]);
            }
            // TODO
        }

        public void LoadFromDatabase(int ProtocolId /*, DB argument*/)
        {
            throw new NotImplementedException();
            int itemsIndex = 0;
            StringBuilder builder = new StringBuilder("select...");
            // TODO
            foreach (var item in ValuableTemplateItems)
            {
                // TODO: 
                // Ask for interface
            }
            // TODO
        }

        public static Protocol LoadProtocol(Template template, int id /*, DB argument*/)
        {
            Protocol protocol = new Protocol(template);
            protocol.LoadFromDatabase(id/*, bb*/);
            return protocol;
        }

        private Object[] Values;
        private TemplateItem[] ValuableTemplateItems;
    }
}
