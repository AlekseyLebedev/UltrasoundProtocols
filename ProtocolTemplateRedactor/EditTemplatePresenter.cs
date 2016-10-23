using NLog;
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

        internal bool SetSelectedItemId(string value)
        {
            logger.Debug("Set selected item id = '{0}'", value);
            InvokeRefresh();
            SelectedItem.Id = value;
            return true;
        }

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
                    throw new NotImplementedException("Error in switch in adding element");
            }
            Template.Items.Add(item);
            logger.Info("Add element {0}", item.Type);
            return item;
        }

        internal UIElement RequestEditControl()
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

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Template Template = new ProtocolTemplateLib.Template();
        private TemplateItem SelectedItem = null;

        internal TemplateItem RemoveSelectedItem()
        {
            TemplateItem item = SelectedItem;
            logger.Debug("Remove slected item. Id='{0}'", item.Id);
            Template.Items.Remove(item);
            return item;
        }
    }
}
