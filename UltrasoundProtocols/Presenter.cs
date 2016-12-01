using NLog;
using ProtocolTemplateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UltrasoundProtocols
{
    class Presenter
    {
        //Текущий выбранный пациент
        private Patient CurrentPatient;
        private int CurrentPatientIndex;
        private DataBaseController Controller;
        private DataBaseConnector Connector;
        private Logger Logger = LogManager.GetCurrentClassLogger();
        private MainWindow MainWindow;
        private bool PatientCreating = false;

        public List<Patient> AllPatients { get; set; }

        public Presenter(MainWindow mainWindow, DataBaseConnector connector)
        {
            this.MainWindow = mainWindow;

            Logger.Info("Connect to dataBase.");
            Controller = new DataBaseController(connector.Settings);
            Connector = connector;
            mainWindow.EditPatientControl.onSaveButtonClick += OnEditSaveButtonClick;
        }

        internal List<Patient> LoadPatientListFromDataBase()
        {
            Logger.Info("Loading patients from dataBase.");
            List<Patient> patientList = new List<Patient>();
            patientList.AddRange(Controller.GetPatients());
            AllPatients = patientList;
            return patientList;
        }

        internal void ShowPatient(PatientShowControl showController, SelectionChangedEventArgs e)
        {
            PatientCreating = false;
            Logger.Info("Showing patient");
            CurrentPatient = (Patient)e.AddedItems[0];
            CurrentPatientIndex = MainWindow.GetSelectedListViewIndex();
            showController.FirstNameTextBlock.Text = CurrentPatient.FirstName;
            showController.SexTextBox.Text = CurrentPatient.Gender.ToString();
            showController.LastNameTextBlock.Text = CurrentPatient.LastName;
            showController.MiddleNameTextBlock.Text = CurrentPatient.MiddleName;
            showController.BirthdayTextBlock.Text = CurrentPatient.BirthDate.ToShortDateString();
            showController.AmbulatorCardTextBlock.Text = CurrentPatient.NumberAmbulatoryCard;
        }

        internal string GetDateString(DateTime dateTime)
        {
            StringBuilder date = new StringBuilder()
                .Append(dateTime.Day)
                .Append(dateTime.Month)
                .Append(dateTime.Year);
            return date.ToString();
        }

        internal void ShowPatientEditor(EditPatientUserControl editController)
        {
            editController.Patient = CurrentPatient;
        }

        internal void OnEditSaveButtonClick(Patient patient)
        {
            Logger.Debug("Saving patient");

            GuiAsyncTask task = new GuiAsyncTask();
            task.Dispatcher = MainWindow.Dispatcher;
            task.ErrorTitle = "Ошибка сохранения пациента";
            task.Fail = MainWindow.HideEditor;
            task.InfoMessage = "Saving patient";
            task.RetryEnabled = true;

            if (PatientCreating)
            {
                Logger.Debug("Adding patient");
                task.AsyncTask = () => patient.Id = Controller.AddPatient(patient);
                task.SyncTask = () =>
                {
                    AllPatients.Add(patient);
                    MainWindow.ViewedPatients = AllPatients;
                    MainWindow.ClearSearch();
                };
            }
            else
            {
                Logger.Debug("Updating patient");
                CurrentPatient = patient;
                task.AsyncTask = () => Controller.UpdatePatient(patient);
                task.SyncTask = () =>
                {
                    MainWindow.UpdateListView();
                };
            }

            task.Run();
            MainWindow.HideEditor();
        }

        internal void OnCreateUser(string query)
        {
            MainWindow.HideAll();
            Patient patient = new Patient();
            string[] tokens = query.Split();
            if (tokens.Length > 0)
                patient.LastName = tokens[0];
            if (tokens.Length > 1)
                patient.FirstName = tokens[1];
            if (tokens.Length > 2)
                patient.MiddleName = tokens[2];
            patient.Gender = PatientGender.Man;
            patient.BirthDate = new DateTime(1995, 7, 7);
            MainWindow.ShowEditor();
            MainWindow.EditPatientControl.Patient = patient;
            PatientCreating = true;
        }

        private List<Patient> SearchByAmbulator(string query)
        {
            List<Patient> ambulatorCardFilter = new List<Patient>();
            foreach (Patient patient in AllPatients)
            {
                if (patient.NumberAmbulatoryCard.StartsWith(query))
                {
                    ambulatorCardFilter.Add(patient);
                }
            }

            return ambulatorCardFilter;
        }

        private bool IsPatientInQuery(string query, Patient patient)
        {
            string[] tokens = query.Split(new char[] { ' ' });

            switch (tokens.Length)
            {
                case 1:
                    return (patient.FirstName.Contains(tokens[0]) || patient.MiddleName.Contains(tokens[0]) || patient.LastName.Contains(tokens[0]));
                case 2:
                    return ((patient.LastName.Contains(tokens[0]) && patient.FirstName.Contains(tokens[1])) || (patient.LastName.Contains(tokens[0]) && patient.MiddleName.Contains(tokens[1])) ||
                        (patient.FirstName.Contains(tokens[0]) && patient.MiddleName.Contains(tokens[1])));
                case 3:
                    return patient.LastName.Contains(tokens[0])
                        && patient.FirstName.Contains(tokens[1])
                        && patient.MiddleName.Contains(tokens[2]);
                default:
                    return false;
            }
        }

        private List<Patient> SearchByName(string query)
        {
            List<Patient> nameFilter = new List<Patient>();
            foreach (Patient patient in AllPatients)
            {
                if (IsPatientInQuery(query, patient))
                {
                    nameFilter.Add(patient);
                }
            }

            return nameFilter;
        }

        internal void OnSearchTextChanged(string query)
        {
            MainWindow.HideAll();
            PatientCreating = false;

            if (query.Equals(""))
            {
                MainWindow.ViewedPatients = AllPatients;
                return;
            }

            List<Patient> filteredPatients = SearchByAmbulator(query);
            if (filteredPatients.Count != 0)
            {
                MainWindow.ViewedPatients = filteredPatients;
                return;
            }

            filteredPatients = SearchByName(query);
            if (filteredPatients.Count != 0)
            {
                MainWindow.ViewedPatients = filteredPatients;
                return;
            }

            MainWindow.ViewedPatients = new List<Patient>();
        }

        internal void OnSearchEnter(string query)
        {
            if (MainWindow.ViewedPatients.Count == 0)
            {
                OnCreateUser(query);
            }
        }

        internal void OnAddUserClick(string lastname)
        {
            OnCreateUser(lastname);
        }

        internal void CloseWindow()
        {
            try
            {
                Connector.CloseConnection();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Can't close connection");
            }
        }

        internal void ClosingWindow()
        {
        }
    }
}
