using NLog;
using ProtocolTemplateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProtocolTemplateRedactor
{
    class EditTemplatePresenter
    {
        internal string SelectedHeaderText
        {
            get { return ((TemplateHeader)SelectedItem).Header; }
            set
            {
                logger.Debug("Set selected header text = '{0}'", value);
                InvokeRefresh();
                ((TemplateHeader)SelectedItem).Header = value;
            }
        }
        internal string SelectedItemId
        {
            get { return SelectedItem.Id; }
        }

        internal bool SelectedEditableEnabled
        {
            get { return ((TemplateLine)SelectedItem).Field.EnableOtherField; }
            set
            {
                logger.Debug("Set selected editable otherEnabled = '{0}'", value);
                InvokeRefresh();
                ((TemplateLine)SelectedItem).Field.EnableOtherField = value;
            }
        }

        internal string SelectedLineLabel
        {
            get { return ((TemplateLine)SelectedItem).Label; }
            set
            {
                logger.Debug("Set selected line label = '{0}'", value);
                InvokeRefresh();
                ((TemplateLine)SelectedItem).Label = value;
            }
        }


        internal string SelectedComboBoxVariants
        {
            get
            {
                List<String> variants = ((ComboboxEditable)((TemplateLine)SelectedItem).Field).Variants;
                StringBuilder result = new StringBuilder();
                for (int i = 0; i < variants.Count; i++)
                {
                    if ((i + 1) < variants.Count)
                    {
                        result.AppendLine(variants[i]);
                    }
                    else
                    {
                        result.Append(variants[i]);
                    }
                }
                return result.ToString();
            }
            set
            {
                logger.Debug("Set variants: '{0}'", value.Replace("\n", "\\n").Replace("\r", "\\r"));
                string[] tokens = value.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                ((ComboboxEditable)((TemplateLine)SelectedItem).Field).Variants = tokens.ToList();
            }
        }

        internal bool SetSelectedItemId(string value)
        {
            logger.Debug("Set selected item id = '{0}'", value);
            if (value.Length == 0)
            {
                logger.Info("Empty text set for id");
                return false;
            }
            for (int i = 0; i < value.Length; i++)
            {
                var symbol = value[i];
                if (!(char.IsDigit(symbol) || ((symbol >= 'a') && (symbol <= 'z'))
                    || ((symbol >= 'A') && (symbol <= 'Z'))))
                {
                    logger.Info("Wrong symbol '{0}' in id", symbol);
                    return false;
                }
            }
            if (!ValidateUniqueId(value))
            {
                logger.Info("Dublicate id");
                return false;
            }
            SelectedItem.Id = value;
            InvokeRefresh();
            return true;
        }

        private const string ADD_LABEL_TEXT = "Добавьте подпись";

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
                    line.Label = ADD_LABEL_TEXT;
                    item = line;
                    break;
                case 2:
                    line = new TemplateLine();
                    line.Field = new ComboboxEditable() { Variants = new List<string>() };
                    line.Label = ADD_LABEL_TEXT;
                    item = line;
                    break;
                default:
                    throw new NotImplementedException("Error in switch in adding element");
            }
            do
            {
                item.Id = "item" + Rnd.Next();
            } while (!ValidateUniqueId(item.Id));
            Template.Items.Add(item);
            logger.Info("Add element {0}", item.Type);
            return item;
        }

        internal Control RequestEditControl()
        {
            logger.Debug("Edit control requested");
            return Template.GetEditControl();
        }

        internal void SelectItem(TemplateItem item)
        {
            if (item == null)
                logger.Debug("Select nothing");
            else
                logger.Debug("Select item type {0} id '{1}'", item.Type, item.Id);
            SelectedItem = item;
        }

        internal TemplateItem GetSelectedItem()
        {
            return SelectedItem;
        }


        internal event EventHandler Refresh;

        private void InvokeRefresh()
        {
            if (Refresh != null)
            {
                Refresh(this, new EventArgs());
            }
        }
        private bool ValidateUniqueId(string name)
        {
            foreach (var item in Template.Items)
            {
                if (item.Id.Equals(name))
                    return false;
            }
            return true;
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Template Template = new ProtocolTemplateLib.Template();
        private TemplateItem SelectedItem = null;
        private Random Rnd = new Random();

        internal TemplateItem RemoveSelectedItem()
        {
            TemplateItem item = SelectedItem;
            logger.Debug("Remove slected item. Id='{0}'", item.Id);
            Template.Items.Remove(item);
            return item;
        }
    }
}
