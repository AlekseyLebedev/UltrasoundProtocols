using NLog;
using ProtocolTemplateLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace ProtocolTemplateRedactor
{
    class EditTemplatePresenter
    {
        internal EditTemplatePresenter()
        {
        }

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

        internal string TemplateName
        {
            get
            {
                var result = Template.Name;
                logger.Debug("Get template name '{0}'", result);
                return result;
            }
            set
            {
                logger.Debug("Set template name '{0}'", value);
                Template.Name = value;
            }
        }

        internal string TemplateId
        {
            get
            {
                var result = Template.IdName;
                logger.Debug("Get template id '{0}'", result);
                return result;
            }
        }

        internal bool SetTemplateId(string value)
        {
            logger.Debug("Set template id '{0}'", value);
            if (ValidateId(value))
            {
                Template.IdName = value;
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool SetSelectedItemId(string value)
        {
            logger.Debug("Set selected item id = '{0}'", value);
            if (ValidateId(value))
            {
                if (!ValidateUniqueId(value))
                {
                    logger.Info("Dublicate id");
                    return false;
                }
                SelectedItem.Id = value;
                InvokeRefresh();
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool ValidateId(string value)
        {
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

        internal string RequestHtmlProtocol()
        {
            logger.Debug("Request HTML");
            CreateProtocolIfNeeded();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<html>");
            builder.AppendLine("<head><meta charset=\"utf-8\"></head>");
            builder.AppendLine("<body>");
            CurrentProtocol.PrintToProtocol(builder);
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");
            string html = builder.ToString();
            logger.Debug("Html: {0}", html);
            return html;
        }

        internal void SaveTemplateToXml(string fileName)
        {
            logger.Debug("Saving to {0}", fileName);
            var xml = Template.SaveToXmlString();
            logger.Debug("Xml: {0}", xml);
            try
            {
                File.WriteAllText(fileName, xml, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error saving file");
                throw ex;
            }
        }

        internal void EnterEditorTab()
        {
            logger.Debug("Enter editor tab");
            CurrentProtocol = null;
        }

        internal Control RequestEditControl()
        {
            logger.Debug("Edit control requested");
            CreateProtocolIfNeeded();
            return Template.GetEditControl();
        }

        internal void LoadTemplateToXml(string fileName)
        {
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(fileName);
                Template = Template.GetFromXml(document);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error loading file");
                throw ex;
            }
        }

        private void CreateProtocolIfNeeded()
        {
            if (CurrentProtocol == null)
            {
                CurrentProtocol = new Protocol(Template);
            }
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
        private Protocol CurrentProtocol = null;

        internal TemplateItem RemoveSelectedItem()
        {
            TemplateItem item = SelectedItem;
            logger.Debug("Remove slected item. Id='{0}'", item.Id);
            Template.Items.Remove(item);
            return item;
        }
    }
}
