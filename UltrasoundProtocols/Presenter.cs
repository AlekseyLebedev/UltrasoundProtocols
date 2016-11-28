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
        private Patient currentPatient;
        private int currentPatientIndex;
        private DataBaseController Controller;
        private DataBaseConnector Connector;
        private Logger Logger = LogManager.GetCurrentClassLogger();
        private MainWindow mainWindow;
        private bool searchActive = false;
        private bool patientCreating = false;

        public Presenter(MainWindow mainWindow, DataBaseConnector connector)
        {
            this.mainWindow = mainWindow;

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
            return patientList;
        }

        internal void ShowPatient(PatientShowControl showController, SelectionChangedEventArgs e)
        {
            patientCreating = false;
            Logger.Info("Showing patient");
            currentPatient = (Patient)e.AddedItems[0];
            currentPatientIndex = mainWindow.GetSelectedListViewIndex();
            showController.FirstNameTextBlock.Text = currentPatient.FirstName;
            showController.SexTextBox.Text = currentPatient.Gender.ToString();
            showController.LastNameTextBlock.Text = currentPatient.LastName;
            showController.MiddleNameTextBlock.Text = currentPatient.MiddleName;
            showController.BirthdayTextBlock.Text = currentPatient.Date.ToShortDateString();
            showController.AmbulatorCardTextBlock.Text = currentPatient.NumberAmbulatoryCard;
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
            editController.Patient = currentPatient;
        }

        internal void OnEditSaveButtonClick(Patient patient)
        {
            if (patientCreating)
            {
                mainWindow.allPatients.Add(patient);
                mainWindow.ViewedPatients = mainWindow.allPatients;
                mainWindow.ClearSearch();
            }
            else
            {
                currentPatient = patient;
                mainWindow.UpdateListView();
            }

            mainWindow.HideEditor();
        }

        private List<Patient> searchByAmbulator(string query)
        {
            List<Patient> ambulatorCardFilter = new List<Patient>();
            foreach (Patient patient in mainWindow.allPatients)
            {
                if (patient.NumberAmbulatoryCard.StartsWith(query))
                {
                    ambulatorCardFilter.Add(patient);
                }
            }

            return ambulatorCardFilter;
        }

        private bool isPatientInQuery(string query, Patient patient)
        {
            string[] tokens = query.Split(new char[] {' '});

            switch (tokens.Length)
            {
                case 1:
                    return patient.LastName.Contains(tokens[0]);
                case 2:
                    return patient.LastName.Contains(tokens[0])
                        && patient.FirstName.Contains(tokens[1]);
                case 3:
                    return patient.LastName.Contains(tokens[0])
                        && patient.FirstName.Contains(tokens[1])
                        && patient.MiddleName.Contains(tokens[2]);
                default:
                    return false;
            }
        }

        private List<Patient> searchByName(string query)
        {
            List<Patient> nameFilter = new List<Patient>();
            foreach (Patient patient in mainWindow.allPatients)
            {
                if (isPatientInQuery(query, patient)) {
                    nameFilter.Add(patient);
                }
            }

            return nameFilter;
        }

        internal void OnSearchTextChanged(string query)
        {
            mainWindow.HideAll();
            patientCreating = false;

            if (query.Equals("")) {
                mainWindow.ViewedPatients = mainWindow.allPatients;
                searchActive = false;
                return;
            }

            searchActive = true;

            List<Patient> filteredPatients = searchByAmbulator(query);
            if (filteredPatients.Count != 0)
            {
                mainWindow.ViewedPatients = filteredPatients;
                return;
            }

            filteredPatients = searchByName(query);
            if (filteredPatients.Count != 0)
            {
                mainWindow.ViewedPatients = filteredPatients;
                return;
            }

            mainWindow.ViewedPatients = new List<Patient>();
        }

        internal void OnSearchEnter(string query)
        {
            if (mainWindow.ViewedPatients.Count == 0)
            {
                Patient patient = new Patient();
                patient.LastName = query;
                patient.Gender = PatientGender.Man;
                patient.Date = new DateTime(1995, 7, 7);
                mainWindow.ShowEditor();
                mainWindow.EditPatientControl.Patient = patient;
                patientCreating = true;
            }
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
