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

namespace ProtocolTemplateLib
{
    /// <summary>
    /// Interaction logic for AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationControl : UserControl
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AuthorizationControl()
        {
            InitializeComponent();
        }

        public event EventHandler<ConnectedEventArgs> Connected;

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
            }
            else
            {
                LoginButton.IsEnabled = true;
            }
        }

        private void TryConnect()
        {
            Properties.Settings.Default.ServerName = ServerBox.Text;
            Properties.Settings.Default.Login = LoginBox.Text;
            Properties.Settings.Default.Save();

            LoginButton.IsEnabled = false;

            DataBaseSettings Settings = new DataBaseSettings();
            Settings.Login = LoginBox.Text;
            Settings.Password = PasswordBox.Password;
            Settings.ServerName = ServerBox.Text;
            DataBaseConnector Connector = new DataBaseConnector(Settings);

            GuiAsyncTask task = new GuiAsyncTask();
            task.AsyncTask = Connector.CreateConnection;
            task.CustomExceptionAction = ShowErrorBox;
            task.SyncTask = () =>
              {
                  Connected(this, new ConnectedEventArgs(Connector));
              };
            task.Logger = logger;
            task.InfoMessage = "login";
            task.Dispatcher = Dispatcher;
            task.Run();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("Login=" + LoginBox.Text);
            logger.Info("Server=" + ServerBox.Text);
            TryConnect();
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            ServerBox.Text = Properties.Settings.Default.ServerName;
            LoginBox.Text = Properties.Settings.Default.Login;
        }
    }

    public class ConnectedEventArgs : EventArgs
    {
        public DataBaseConnector Connector { get; private set; }

        public ConnectedEventArgs(DataBaseConnector connector)
        {
            Connector = connector;
        }
    }
}
