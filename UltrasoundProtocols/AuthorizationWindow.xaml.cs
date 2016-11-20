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
using ProtocolTemplateLib;
using NLog;

namespace UltrasoundProtocols
{
    /// <summary>
    /// Interaction logic for AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AuthorizationWindow()
        {
            InitializeComponent();
            autrorisationControl.Connected += AutrorisationControl_Connected;
        }

        private void AutrorisationControl_Connected(object sender, ConnectedEventArgs e)
        {
            MainWindow Main = new MainWindow();
            Main.Connector = e.Connector;
            this.Hide();
            Main.ShowDialog();
            this.Close();
        }
    }
}
