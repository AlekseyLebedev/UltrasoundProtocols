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

using System.Data.SqlClient;
using System.Xml;

namespace UltrasoundProtocols
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int SECOND_COLUMN_WIDTH = 10;
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
                Presenter = new Presenter(this, value);
            }
        }

        private List<Patient> _ViewedPatients;
        public List<Patient> ViewedPatients
        {
            get { return _ViewedPatients; }
            set { _ViewedPatients = value; UpdateListView(); }
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
        }

        Presenter Presenter;

        private void EditPatientBotton_Click(object sender, RoutedEventArgs e)
        {
            logger.Debug("Edit Template Button is pressed");
            Presenter.ShowPatientEditor(EditPatientControl);
            HidePatientInfo();
            ShowEditor();
        }

        public void ShowPatientInfo()
        {
            PatientColumn.Width = new GridLength(SECOND_COLUMN_WIDTH, GridUnitType.Star);
        }

        public void HidePatientInfo()
        {
            PatientColumn.Width = new GridLength(0, GridUnitType.Star);
        }

        public void ShowEditor()
        {
            EditPatientColumn.Width = new GridLength(SECOND_COLUMN_WIDTH, GridUnitType.Star);
        }

        public void HideEditor()
        {
            EditPatientColumn.Width = new GridLength(0, GridUnitType.Star);
        }

        public void HideAll()
        {
            HideEditor();
            HidePatientInfo();
        }

        public int GetSelectedListViewIndex()
        {
            return PatientsListView.SelectedIndex;
        }

        public void ClearSearch()
        {
            Search.Text = "";
        }

        public void UpdateListView()
        {
            PatientsListView.Items.Clear();
            foreach (Patient patient in ViewedPatients)
            {
                PatientsListView.Items.Add(patient);
            }
        }

        private void PatientsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                logger.Debug("show one patient");
                var patient = (Patient)e.AddedItems[0];
                Presenter.ShowPatient(patient);
                PatientInfoControl.CurrentPatient = patient;
                EditPatientColumn.Width = new GridLength(0, GridUnitType.Star);
                ShowPatientInfo();
            }
            else
            {
                Presenter.ShowPatient(null);
                HidePatientInfo();
                HideEditor();
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Presenter.ClosingWindow();
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            Presenter.CloseWindow();
        }

        private Logger logger = LogManager.GetCurrentClassLogger();

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Presenter.OnSearchTextChanged(Search.Text);
        }

        private void Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Return))
            {
                Presenter.OnSearchEnter(Search.Text);
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            Presenter.OnAddUserClick(Search.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HidePatientInfo();
            HideEditor();

            LoadAllPatients();

        }

        private void LoadAllPatients()
        {
            this.IsEnabled = false;
            logger.Debug("Loading patients");
            GuiAsyncTask<List<Patient>> task = new GuiAsyncTask<List<Patient>>();
            task.AsyncTask = () => Presenter.LoadPatientListFromDataBase();
            task.SyncTask = (patientList) =>
            {
                ViewedPatients = patientList;

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
    }
}
