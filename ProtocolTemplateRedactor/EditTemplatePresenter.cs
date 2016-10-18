using ProtocolTemplateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolTemplateRedactor
{
    class EditTemplatePresenter
    {
        Template template = new ProtocolTemplateLib.Template();

        internal void AddItem(int selectedIndex)
        {
            TemplateLine line = new TemplateLine();
            switch (selectedIndex)
            {
                case 0:
                    template.Items.Add(new TemplateHeader());
                    break;
                case 1:
                    line.Field = new TextBoxEditable();
                    template.Items.Add(line);
                    break;
                case 2:
                    line.Field = new ComboboxEditable();
                    template.Items.Add(line);
                    break;
                default:
                    break;
            }

        }
    }
}
