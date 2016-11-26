﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ProtocolTemplateLib;
using System.Threading;
using NLog;

using System.Data.SqlClient;
using System.Xml;

namespace UltrasoundProtocols
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataBaseConnector Connector_;
        public DataBaseConnector Connector
        {
            get { return Connector_; }
            set
            {
                if (Presenter != null)
                {
                    throw new NotSupportedException("Settings has been already set");
                }
                Connector_ = value;
                Presenter = new EditPatientPresenter(value);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Surname.DisplayMemberBinding = new Binding("LastName");
            NameColumn.DisplayMemberBinding = new Binding("FirstName");
            MiddleName.DisplayMemberBinding = new Binding("MiddleName");
            id.DisplayMemberBinding = new Binding("NumberAmbulatoryCard");
            Birthday.DisplayMemberBinding = new Binding("Date");
            Birthday.DisplayMemberBinding.StringFormat = "dd.MM.yyyy";

			//проверка корректности работы сохранения данных
			//SqlCommand command = new SqlCommand();
			//XmlDocument doc = new XmlDocument();
			//doc.Load("D:\\ABBYY\\UltraSoundProtocol\\UltrasoundProtocols\\ProtocolTemplateRedactor\\bin\\Debug\\test.xml");
			//Template template = ProtocolTemplateLib.Template.GetFromXml(doc);
			//Protocol protocol = new Protocol(template);
			//protocol.SaveToDatabase(1, command);
        }

        EditPatientPresenter Presenter;

        private void EditPatientBotton_Click(object sender, RoutedEventArgs e)
        {
            logger.Debug("Edit Template Button is pressed");
            Presenter.ShowPatientEditor(editController);
            PatientColumn.Width = new GridLength(0, GridUnitType.Star);
            EditPatientColumn.Width = new GridLength(9, GridUnitType.Star);
        }

        private void listView_Loaded(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            logger.Debug("Loading patients");
            GuiAsyncTask<List<Patient>> task = new GuiAsyncTask<List<Patient>>();
            task.AsyncTask = () => Presenter.LoadPatientListFromDataBase();
            task.SyncTask = (patientList) =>
              {
                  foreach (Patient patient in patientList)
                  {
                      listView.Items.Add(patient);
                  }
                  this.IsEnabled = true;
              };
            task.Fail = () => Environment.Exit(1);
            task.RetryEnabled = true;
            task.ErrorTitle = "Ошибка загрузки пациентов";
            task.Dispatcher = Dispatcher;
            task.Logger = logger;
            task.InfoMessage = "Loading Patients";
            task.Run();
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                logger.Debug("show one patient");
                Presenter.ShowPatient(showController, e);
                EditPatientColumn.Width = new GridLength(0, GridUnitType.Star);
                PatientColumn.Width = new GridLength(9, GridUnitType.Star);
            }
            else
            {
                // TODO
            }

        }

        private Logger logger = LogManager.GetCurrentClassLogger();
    }
}
