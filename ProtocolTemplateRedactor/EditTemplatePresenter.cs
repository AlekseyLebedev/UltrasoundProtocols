using ProtocolTemplateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ProtocolTemplateRedactor
{
    class EditTemplatePresenter
    {
        Template template = new ProtocolTemplateLib.Template();

        internal TemplateItem AddItem(int selectedIndex)
        {
            TemplateItem item = new TemplateLine();
            switch (selectedIndex)
            {
                case 0:
                    item = new TemplateHeader();
                    break;
                case 1:
                    TemplateLine line = new TemplateLine();
                    line.Field = new TextBoxEditable();
                    item = line;
                    break;
                case 2:
                    line = new TemplateLine();
                    line.Field = new ComboboxEditable();
                    item = line;
                    break;
                default:
                    break;
            }
            template.Items.Add(item);
            return item;
        }

        internal UIElement RequestEditControl()
        {
            return template.GetEditControl();
        }
    }
}
