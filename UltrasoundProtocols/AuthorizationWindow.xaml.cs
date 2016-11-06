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
        }

        private void ShowErrorBox(Exception exc)
        {
            MessageBoxResult dialogResult = MessageBox.Show(
                exc.Message + "\nПопробовать ещё раз?",
                "Ошибка подключения",
                MessageBoxButton.YesNo,
                MessageBoxImage.Error);

            if (dialogResult == MessageBoxResult.Yes)
            {
                TryConnect();
                return;
            }
            else if (dialogResult == MessageBoxResult.No)
            {
                //relax
            }
        }

        private void TryConnect()
        {
            DataBaseSettings Settings = new DataBaseSettings();
            Settings.Login = LoginBox.Text;
            Settings.Password = PasswordBox.Password;
            Settings.DataSource = ServerBox.Text;

            DataBaseConnector Connector = new DataBaseConnector(Settings);

            try
            {
                Connector.CreateConnection();
            }
            catch (Exception exc)
            {
                ShowErrorBox(exc);
            }

            //all good start MainWindow
            MainWindow Main = new MainWindow();
            Main.Connector = Connector;
            Main.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("Login=" + LoginBox.Text);
            TryConnect();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ServerBox.Text = Properties.Settings.Default.ServerName;
            LoginBox.Text = Properties.Settings.Default.Login;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.ServerName = ServerBox.Text;
            Properties.Settings.Default.Login = LoginBox.Text;
            Properties.Settings.Default.Save();
        }
    }
}
