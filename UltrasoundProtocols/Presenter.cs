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

        public Presenter(MainWindow mainWindow, DataBaseConnector connector)
        {
            this.mainWindow = mainWindow;

            Logger.Info("Connect to dataBase.");
            Controller = new DataBaseController(connector.Settings);
            Connector = connector;
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
            editController.onSaveButtonClick += OnEditSaveButtonClick;
        }

        internal void OnEditSaveButtonClick(Patient patient)
        {
            currentPatient = patient;
            mainWindow.HideEditor();
            mainWindow.UpdateListViewItem(currentPatientIndex, patient);
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
