using ProtocolTemplateLib;
using System;
using System.Collections.Generic;
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
            InformationColumn.DisplayMemberBinding = new Binding("Info");
        }

        EditTemplatePresenter presenter = new EditTemplatePresenter();

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

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
    }
}
