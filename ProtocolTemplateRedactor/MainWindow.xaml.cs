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
            InitializeComponent();
            TypeColumn.DisplayMemberBinding = new Binding("Type");
            IdColumn.DisplayMemberBinding = new Binding("Id");
            InformationColumn.DisplayMemberBinding = new Binding("Info");
            presenter = new EditTemplatePresenter();
            presenter.Refresh += Presenter_Refresh;
        }

        private void Presenter_Refresh(object sender, EventArgs e)
        {
            listView.Items.Refresh();
        }

        EditTemplatePresenter presenter;

        private void PreviewTabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            PreviewGrid.Children.Clear();
            Control element = presenter.RequestEditControl();
            PreviewGrid.Children.Add(element);           
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            listView.Items.Add(presenter.AddItem(comboBoxSelect.SelectedIndex));
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RedactorGrid.Children.Clear();
            if (e.AddedItems.Count == 1)
            {
                TemplateItem selectedItem = (TemplateItem)e.AddedItems[0];
                presenter.SelectItem(null);
                IdTextBox.Text = selectedItem.Id;
                presenter.SelectItem(selectedItem);
                SetRedactor(selectedItem);
                PropertiesGroupBox.IsEnabled = true;
            }
            else
            {
                presenter.SelectItem(null);
                IdTextBox.Text = "";
                PropertiesGroupBox.IsEnabled = false;
            }
        }

        private void SetRedactor(TemplateItem selectedItem)
        {
            Control redactor = null;
            if (selectedItem is TemplateHeader)
            {
                redactor = new HeaderRedactor() { Presenter = presenter };

            }
            else if (selectedItem is TemplateLine)
            {
                TemplateLine line = (TemplateLine)selectedItem;
                if (line.Field is TextBoxEditable)
                {
                    redactor = new LineRedactor() { Presenter = presenter };
                }
                else if (line.Field is ComboboxEditable)
                {
                    redactor = new ComboBoxRedactor() { Presenter = presenter };
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
            if (presenter.GetSelectedItem() != null)
            {
                IdTextBox.Background = (presenter.SetSelectedItemId(IdTextBox.Text) ? Brushes.White : Brushes.Red);
            }
            else
            {
                IdTextBox.Background = Brushes.White;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            listView.Items.Remove(presenter.RemoveSelectedItem());
        }

        private void EditTabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            presenter.EnterEditorTab();
        }

        private void ProtocolPreviewTabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            UpdateHtmlProtocol();
        }

        private void UpdateHtmlProtocol()
        {
            ProtocolBrowser.NavigateToString(presenter.RequestHtmlProtocol());
        }

        private void HtmlPreviewGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            UpdateHtmlProtocol();
        }
    }
}
