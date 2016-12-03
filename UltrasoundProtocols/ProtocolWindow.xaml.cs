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
using System.Windows.Shapes;

namespace UltrasoundProtocols
{
    /// <summary>
    /// Логика взаимодействия для ProtocolWindow.xaml
    /// </summary>
    public partial class ProtocolWindow : Window
    {
        public ProtocolWindow()
        {
            InitializeComponent();
        }

        public Patient CurrentPatient { get; set; }

        public DataBaseController Database { get; set; }

        public FullProtocol Value
        {
            get
            {
                return Value_;
            }
            set
            {
                if (value == null)
                {
                    Value_ = new FullProtocol();
                    ShowEditProtocol();
                }
                else
                {
                    Value_ = value;
                    ShowHtmlProtocol();
                }
            }
        }

        private void ShowEditProtocol()
        {
            FullProtocolEditor.Controller = Database;
            FullProtocolEditor.Patient = CurrentPatient;
            FullProtocolEditor.FullProtocol = Value;
            EditViewColumn.Width = new GridLength(1, GridUnitType.Star);
            HtmlViewColumn.Width = new GridLength(0, GridUnitType.Star);
            EditProtocolRootGrid.IsEnabled = true;
        }

        private void ShowHtmlProtocol()
        {
            HtmlViewColumn.Width = new GridLength(1, GridUnitType.Star);
            EditViewColumn.Width = new GridLength(0, GridUnitType.Star);
            ProtocolWebBrowser.NavigateToString(Value.PrintToProtocol());
        }

        private FullProtocol Value_;

        private void FullProtocolEditor_SaveButtonClick(object sender, FullProtocolEditControl.SaveButtonClickEventArgs e)
        {
            EditingSuccesseded = true;
            Value = e.Value;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private bool EditingSuccesseded = false;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = EditingSuccesseded;
        }
    }
}
