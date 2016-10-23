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
            UIElement element = presenter.RequestEditControl();
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
                if (selectedItem is TemplateHeader)
                {
                    HeaderRedactor headerRedactor = new HeaderRedactor();
                    headerRedactor.Margin = new Thickness(0);
                    headerRedactor.Presenter = presenter;
                    RedactorGrid.Children.Add(headerRedactor);
                }
                else
                {
                    if (selectedItem is TemplateLine)
                    {

                    }
                    else
                        throw new NotImplementedException("Editro doesn't support this type of element");
                }
                PropertiesGroupBox.IsEnabled = true;
            }
            else
            {
                presenter.SelectItem(null);
                IdTextBox.Text = "";
                PropertiesGroupBox.IsEnabled = false;
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
    }
}
