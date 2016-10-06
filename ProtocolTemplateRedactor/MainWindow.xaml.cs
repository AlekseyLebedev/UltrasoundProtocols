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
            buttonAdd.Click += addButtonClick;

        }
        void addButtonClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch( comboBoxSelect.SelectedIndex )
            {
                case 0:
                    template.Items.Add(new TemplateHeader());
                    break;
                case 1:
                    template.Items.Add(new TemplateLine());
                    break;
                case 2:
                    template.Items.Add(new TemplateLine());
                    break;
                default:
                    break;
            }

            
        }
        Template template = new ProtocolTemplateLib.Template();
       
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }
}
