using System;
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

namespace UltrasoundProtocols
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DataBaseConnector Connector { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Surname.DisplayMemberBinding = new Binding("LastName");
            NameColumn.DisplayMemberBinding = new Binding("FirstName");
            MiddleName.DisplayMemberBinding = new Binding("MiddleName");
            id.DisplayMemberBinding = new Binding("NumberAmbulatoryCard");
            Gender.DisplayMemberBinding = new Binding("Gender");
            Birthday.DisplayMemberBinding = new Binding("Date");
            presenter = new EditPatientPresenter();
        }

        EditPatientPresenter presenter;

        private void EditPatientBotton_Click(object sender, RoutedEventArgs e)
        {
            logger.Debug("Edit Template Button is pressed");
            PatientColumn.Width = new GridLength(0, GridUnitType.Star);
            EditPatientColumn.Width = new GridLength(9, GridUnitType.Star);
        }

        private void listView_Loaded(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            logger.Debug("Loading patients");
            GuiAsyncTask<List<Patient>> task = new GuiAsyncTask<List<Patient>>();
            task.AsyncTask = () => presenter.LoadPatientListFromDataBase();
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
                presenter.ShowPatient(showController, e);
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
