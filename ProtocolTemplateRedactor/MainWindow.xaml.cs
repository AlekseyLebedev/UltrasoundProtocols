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
        public struct ExampleType
        {
            public ExampleType(string _type, string _info)
            {
                type = _type;
                info = _info;
            }

            string type;
            public string Type
            {
                get { return type; }
                set { type = value; }
            }

            string info;
            public string Info
            {
                get { return info; }
                set { info = value; }
            }
        }
        
        public MainWindow()
        {
            InitializeComponent();
            buttonAdd.Click += addButtonClick;
            GridView view = new GridView();

            GridViewColumn col1 = new GridViewColumn();
            col1.Header = "Type";
            col1.DisplayMemberBinding = new Binding("Type");
            view.Columns.Add(col1);

            GridViewColumn col2 = new GridViewColumn();
            col2.Header = "Info";
            col2.DisplayMemberBinding = new Binding("Info");
            view.Columns.Add(col2);

            listView.View = view;

            listView.Items.Add(new ExampleType("Запись", "Какой-то текст"));
            listView.Items.Add(new ExampleType("Доктор", "Иванов Федор Михайлович"));
        }
        void addButtonClick(object sender, EventArgs e)
        {
            presenter.AddItem(comboBoxSelect.SelectedIndex);
            
        }
        EditTemplatePresenter presenter = new EditTemplatePresenter();
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }
}
