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
                Logger.Debug("Set selected header text = '{0}'", value);
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
                Logger.Debug("Set selected editable otherEnabled = '{0}'", value);
                InvokeRefresh();
                ((TemplateLine)SelectedItem).Field.EnableOtherField = value;
            }
        }

        internal string SelectedLineLabel
        {
            get { return ((TemplateLine)SelectedItem).Label; }
            set
            {
                Logger.Debug("Set selected line label = '{0}'", value);
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
                Logger.Debug("Set variants: '{0}'", value.Replace("\n", "\\n").Replace("\r", "\\r"));
                string[] tokens = value.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                ((ComboboxEditable)((TemplateLine)SelectedItem).Field).Variants = tokens.ToList();
            }
        }

        internal string TemplateName
        {
            get
            {
                var result = Template_.Name;
                Logger.Debug("Get template name '{0}'", result);
                return result;
            }
            set
            {
                Logger.Debug("Set template name '{0}'", value);
                Template_.Name = value;
            }
        }

        internal string TemplateId
        {
            get
            {
                var result = Template_.IdName;
                Logger.Debug("Get template id '{0}'", result);
                return result;
            }
        }

        internal List<TemplateItem> AllItems { get { return Template_.Items; } }

        internal DataBaseConnector Connector { get; set; }

        internal Template SelectedTemplate
        {
            get
            {
                return SelectedTemplate_;
            }
            set
            {
                SelectedTemplate_ = value;
                if (value == null)
                    Logger.Debug("Deselect template");
                else
                    Logger.Debug("Set template id {0} name {1}", value.IdName, value.Name);
            }
        }

        internal bool SetTemplateId(string value)
        {
            Logger.Debug("Set template id '{0}'", value);
            if (ValidateId(value))
            {
                Template_.IdName = value;
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool SetSelectedItemId(string value)
        {
            Logger.Debug("Set selected item id = '{0}'", value);
            if (ValidateId(value))
            {
                if (!ValidateUniqueId(value))
                {
                    Logger.Info("Dublicate id");
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
            Template_.Items.Add(item);
            Logger.Info("Add element {0}", item.Type);
            return item;
        }

        internal string RequestHtmlProtocol()
        {
            Logger.Debug("Request HTML");
            CreateProtocolIfNeeded();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<html>");
            builder.AppendLine("<head><meta charset=\"utf-8\"></head>");
            builder.AppendLine("<body>");
            CurrentProtocol.PrintToProtocol(builder);
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");
            string html = builder.ToString();
            Logger.Debug("Html: {0}", html);
            return html;
        }

        internal void SaveTemplateToXml(string fileName)
        {
            Logger.Debug("Saving to {0}", fileName);
            var xml = Template_.SaveToXmlString();
            Logger.Debug("Xml: {0}", xml);
            try
            {
                File.WriteAllText(fileName, xml, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error saving file");
                throw ex;
            }
        }

        internal void EnterEditorTab()
        {
            Logger.Debug("Enter editor tab");
            CurrentProtocol = null;
        }

        internal Control RequestEditControl()
        {
            Logger.Debug("Edit control requested");
            CreateProtocolIfNeeded();
            return Template_.GetEditControl();
        }

        internal void LoadTemplateToXml(string fileName)
        {
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(fileName);
                SelectOtherTemplate(Template.GetFromXml(document));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error loading file");
                throw ex;
            }
        }

        internal void SelectItem(TemplateItem item)
        {
            if (item == null)
                Logger.Debug("Select nothing");
            else
                Logger.Debug("Select item type {0} id '{1}'", item.Type, item.Id);
            SelectedItem = item;
        }

        internal List<Template> LoadTemplates()
        {
            Logger.Debug("Loading templates");
            try
            {
                using (var adpater = new TemplatesDataSetTableAdapters.Tbl_TemplatesTableAdapter(Connector.Settings))
                {
                    TemplatesDataSet.Tbl_TemplatesDataTable table = adpater.GetData();
                    return (from row in table select Template.GetFromDatabaseEntry(row.tem_name, row.tem_id, row.tem_template)).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error Loading templates");
                throw ex;
            }
        }

        internal TemplateItem GetSelectedItem()
        {
            return SelectedItem;
        }

        internal Template Template
        {
            get
            {
                return Template_;
            }
        }

        internal event EventHandler Refresh;

        internal bool SaveTemplateToDB(bool force)
        {
            Logger.Info("Saving tamplte id '{0}' name '{1}'", Template_.IdName, Template_.Name);
            using (var adapter = new TemplatesDataSetTableAdapters.Tbl_TemplatesTableAdapter(Connector.Settings))
            {
                TemplatesDataSet.Tbl_TemplatesDataTable table = adapter.GetData();
                var templates = from r in table select r;

                var alreadyContainsRow = templates.FirstOrDefault(x => (x.tem_id == Template_.IdName));
                bool containsId = alreadyContainsRow != null;
                Logger.Info("Find id: {0}", containsId);
                if (containsId && (!force))
                {
                    return false;
                }

                string xml = Template_.SaveToXmlString();
                Logger.Debug("Xml: {0}", xml);
                if (containsId)
                {
                    alreadyContainsRow.tem_name = Template_.Name;
                    alreadyContainsRow.tem_template = xml;
                    adapter.Update(alreadyContainsRow);
                }
                else
                {
                    adapter.Insert(Template_.IdName, Template_.Name, xml);
                }

                return true;
            }
        }

        internal TemplateItem RemoveSelectedItem()
        {
            TemplateItem item = SelectedItem;
            Logger.Debug("Remove slected item. Id='{0}'", item.Id);
            Template_.Items.Remove(item);
            return item;
        }

        internal void LoadSeletedTemplate()
        {
            Logger.Debug("Selected template loaded");
            Template_ = SelectedTemplate;

        }

        internal void DeleteSelectedProtocol()
        {
            Logger.Debug("Delete selected template id '{0}' name '{1}'", SelectedTemplate.IdName, SelectedTemplate.Name);
            using (var adapter = new TemplatesDataSetTableAdapters.Tbl_TemplatesTableAdapter(Connector.Settings))
            {
                TemplatesDataSet.Tbl_TemplatesDataTable table = adapter.GetData();
                adapter.Delete(SelectedTemplate.IdName, SelectedTemplate.Name);
                SelectedTemplate_ = null;
            }
        }


        private static bool ValidateId(string value)
        {
            if (value.Length == 0)
            {
                Logger.Info("Empty text set for id");
                return false;
            }
            for (int i = 0; i < value.Length; i++)
            {
                var symbol = value[i];
                if (!(char.IsDigit(symbol) || ((symbol >= 'a') && (symbol <= 'z'))
                    || ((symbol >= 'A') && (symbol <= 'Z'))))
                {
                    Logger.Info("Wrong symbol '{0}' in id", symbol);
                    return false;
                }
            }
            return true;
        }

        private void InvokeRefresh()
        {
            if (Refresh != null)
            {
                Refresh(this, new EventArgs());
            }
        }
        private bool ValidateUniqueId(string name)
        {
            foreach (var item in Template_.Items)
            {
                if (item.Id.Equals(name))
                    return false;
            }
            return true;
        }

        private void SelectOtherTemplate(Template template)
        {
            Template_ = template;
            CurrentProtocol = null;
        }

        private void CreateProtocolIfNeeded()
        {
            if (CurrentProtocol == null)
            {
                CurrentProtocol = new Protocol(Template_);
            }
        }

        private const string ADD_LABEL_TEXT = "Добавьте подпись";

        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private Template Template_ = new ProtocolTemplateLib.Template();
        private TemplateItem SelectedItem = null;
        private Random Rnd = new Random();
        private Template SelectedTemplate_ = null;
        private Protocol CurrentProtocol = null;
    }
}
