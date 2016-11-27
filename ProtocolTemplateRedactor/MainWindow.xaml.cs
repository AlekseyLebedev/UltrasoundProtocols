using Microsoft.Win32;
using NLog;
using ProtocolTemplateLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace ProtocolTemplateRedactor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Presenter = new EditTemplatePresenter();
            InitializeComponent();
            TypeColumn.DisplayMemberBinding = new Binding("Type");
            IdColumn.DisplayMemberBinding = new Binding("Id");
            InformationColumn.DisplayMemberBinding = new Binding("Info");
            Presenter.Refresh += Presenter_Refresh;
            DataBaseGrid.Visibility = Visibility.Collapsed;
            Autorization.Visibility = Visibility.Visible;
        }

        private void Presenter_Refresh(object sender, EventArgs e)
        {
            ItemsListView.Items.Refresh();
        }

        EditTemplatePresenter Presenter;

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var templateItem = Presenter.AddItem(comboBoxSelect.SelectedIndex);
            ItemsListView.Items.Add(templateItem);
            ItemsListView.SelectedItem = templateItem;
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RedactorGrid.Children.Clear();
            if (e.AddedItems.Count == 1)
            {
                TemplateItem selectedItem = (TemplateItem)e.AddedItems[0];
                Presenter.SelectItem(null);
                IdTextBox.Text = selectedItem.Id;
                Presenter.SelectItem(selectedItem);
                SetRedactor(selectedItem);
                PropertiesGroupBox.IsEnabled = true;
            }
            else
            {
                SelectNoItems();
            }
        }

        private void SelectNoItems()
        {
            Presenter.SelectItem(null);
            IdTextBox.Text = "";
            PropertiesGroupBox.IsEnabled = false;
        }

        private void SetRedactor(TemplateItem selectedItem)
        {
            Control redactor = null;
            if (selectedItem is TemplateHeader)
            {
                redactor = new HeaderRedactor() { Presenter = Presenter };

            }
            else if (selectedItem is TemplateLine)
            {
                TemplateLine line = (TemplateLine)selectedItem;
                if (line.Field is TextBoxEditable)
                {
                    redactor = new LineRedactor() { Presenter = Presenter };
                }
                else if (line.Field is ComboboxEditable)
                {
                    redactor = new ComboBoxRedactor() { Presenter = Presenter };
                }
                else
                    throw new NotImplementedException("Editor doesn't support editable in template line");
            }
            else
                throw new NotImplementedException("Editor doesn't support this type of element");
            if (redactor != null)
            {
                redactor.Margin = new Thickness(0);
                RedactorGrid.Children.Add(redactor);
            }
        }

        private void PropertiesGroupBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Thickness margin = RedactorGrid.Margin;
            margin.Top = IdInfoTextBox.ActualHeight + IdInfoTextBox.Margin.Top + 5;
            RedactorGrid.Margin = margin;
        }

        private void IdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Presenter.GetSelectedItem() != null)
            {
                IdTextBox.Background = (Presenter.SetSelectedItemId(IdTextBox.Text) ? Brushes.White : Brushes.Red);
            }
            else
            {
                IdTextBox.Background = Brushes.White;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ItemsListView.Items.Remove(Presenter.RemoveSelectedItem());
        }

        private void UpdateHtmlProtocol()
        {
            ProtocolBrowser.NavigateToString(Presenter.RequestHtmlProtocol());
        }

        private void HtmlPreviewGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            UpdateHtmlProtocol();
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                if (item == ProtocolPreviewTabItem)
                {
                    UpdateHtmlProtocol();
                    continue;
                }
                if (item == PreviewTabItem)
                {
                    PreviewGrid.Children.Clear();
                    Control element = Presenter.RequestEditControl();
                    PreviewGrid.Children.Add(element);
                    continue;
                }
                if (item == EditTabItem)
                {
                    Presenter.EnterEditorTab();
                    continue;
                }
            }
        }

        private void textBoxId_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool valid = Presenter.SetTemplateId(textBoxId.Text);
            // При инициализации этот метод вызывается, и тода кнопки еще не инициализированы.
            if (SaveFileButton != null)
            {
                SaveFileButton.IsEnabled = valid;
                textBoxId.Background = valid ? Brushes.White : Brushes.Red;
            }
        }

        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Presenter.TemplateName = textBoxName.Text;
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            InitDialog(dialog);

            dialog.Title = "Сохранение шаблона";
            dialog.CheckFileExists = false;

            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                try
                {
                    Presenter.SaveTemplateToXml(dialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка сохранения файла.", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private static void InitDialog(FileDialog dialog)
        {
            dialog.AddExtension = true;
            dialog.DefaultExt = ".xml";
            dialog.Filter = "Xml files(*.xml)|*.xml|All files|*.*";
            dialog.ValidateNames = true;
        }

        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            InitDialog(dialog);

            dialog.Title = "Загрузка шаблона";
            dialog.CheckFileExists = true;

            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                try
                {
                    Presenter.LoadTemplateToXml(dialog.FileName);
                    LoadOtherTemplate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка загрузки файла.", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadOtherTemplate()
        {
            textBoxId.Text = Presenter.TemplateId;
            textBoxName.Text = Presenter.TemplateName;
            ItemsListView.Items.Clear();
            SelectNoItems();
            List<TemplateItem> allItems = Presenter.AllItems;
            foreach (var item in allItems)
            {
                ItemsListView.Items.Add(item);
            }
        }

        private void Autorization_Connected(object sender, ConnectedEventArgs e)
        {
            Presenter.Connector = e.Connector;
            Autorization.Visibility = Visibility.Collapsed;
            GuiAsyncTask<List<Template>> task = new GuiAsyncTask<List<Template>>();
            task.AsyncTask = Presenter.LoadTemplates;
            task.Dispatcher = Dispatcher;
            task.ErrorTitle = "Ошибка загрузки шаблонов";
            task.InfoMessage = "Template loading";
            task.Logger = Logger;
            task.RetryEnabled = true;
            task.SyncTask = (results) =>
            {
                foreach (var template in results)
                {
                    Logger.Info("Template loaded. Id '{0}', name '{1}'", template.IdName, template.Name);
                    TemplatesListView.Items.Add(template);
                }
                DataBaseGrid.Visibility = Visibility.Visible;
            };
            task.Fail = () => Autorization.Visibility = Visibility.Visible;
            task.Run();
        }

        private Logger Logger = LogManager.GetCurrentClassLogger();

        private void TemplatesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                Presenter.SelectedTemplate = (Template)e.AddedItems[0];
                SetTemplateButtonsEnabled(true);
            }
            else
            {
                Presenter.SelectedTemplate = null;
                SetTemplateButtonsEnabled(false);
            }
        }

        private void SetTemplateButtonsEnabled(bool enabled)
        {
            LoadTemplateButton.IsEnabled = enabled;
            DeleteTemplateButton.IsEnabled = enabled;
        }

        private void AddTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            DataBaseGroupBox.IsEnabled = false;
            GuiAsyncTask<bool> task = new GuiAsyncTask<bool>();
            task.AsyncTask = () => Presenter.SaveTemplateToDB(false);
            task.Dispatcher = Dispatcher;
            task.ErrorTitle = "Ошибка добавления шаблона";
            task.InfoMessage = "Template adding";
            task.Logger = Logger;
            task.RetryEnabled = true;
            task.SyncTask = (success) =>
            {
                if (success)
                {
                    ReloadTemplates();
                }
                else
                {
                    if (MessageBox.Show("Шаблон с таким идентификатором уже существует. Заменить?", "Добавление в базу данных", MessageBoxButton.YesNo,
                        MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        task.AsyncTask = () => Presenter.SaveTemplateToDB(true);
                        task.Run();
                        return;
                    }
                    else
                    {
                        DataBaseGroupBox.IsEnabled = true;
                    }
                }
            };
            task.Fail = () => DataBaseGroupBox.IsEnabled = true;
            task.Run();
        }

        private void ReloadTemplates()
        {
            Logger.Debug("Reloading templates");
            GuiAsyncTask<IEnumerable<Template>> reloadTask = new GuiAsyncTask<IEnumerable<Template>>();
            reloadTask.AsyncTask = Presenter.LoadTemplates;
            reloadTask.Dispatcher = Dispatcher;
            reloadTask.ErrorTitle = "Ошибка загрузки шаблонов";
            reloadTask.InfoMessage = "Template reloading";
            reloadTask.Logger = Logger;
            reloadTask.RetryEnabled = true;
            reloadTask.SyncTask = (results) =>
            {
                TemplatesListView.Items.Clear();
                foreach (var template in results)
                {
                    TemplatesListView.Items.Add(template);
                }
                DataBaseGroupBox.IsEnabled = true;
            };
            reloadTask.Fail = () =>
            {
                DataBaseGrid.Visibility = Visibility.Collapsed;
                Autorization.Visibility = Visibility.Visible;
            };
            reloadTask.Run();
        }

        private void LoadTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Загрузив шаблон из базы данных, вы потеряете все несохраненные изменения. Вы уверены, что хотите загрузить шаблон?",
                "Загрузка шаблона из базы данны", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Presenter.LoadSeletedTemplate();
                LoadOtherTemplate();
            }
        }

        private void DeleteTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            Template selected = Presenter.SelectedTemplate;
            DataBaseGroupBox.IsEnabled = false;
            GuiAsyncTask task = new GuiAsyncTask();
            task.AsyncTask = Presenter.DeleteSelectedProtocol;
            task.Dispatcher = Dispatcher;
            task.ErrorTitle = "Ошибка удаления шаблона";
            task.InfoMessage = "Template adding";
            task.Logger = Logger;
            task.RetryEnabled = true;
            task.Fail = ReloadTemplates;
            task.SyncTask = () =>
            {
                TemplatesListView.Items.Remove(selected);
                DataBaseGroupBox.IsEnabled = true;
                SetTemplateButtonsEnabled(false);
            };
            task.Run();
        }
    }
}
