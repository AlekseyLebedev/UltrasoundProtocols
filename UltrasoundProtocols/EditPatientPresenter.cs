using NLog;
using ProtocolTemplateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UltrasoundProtocols
{
    class EditPatientPresenter
    {
        //Текущий выбранный пациент
        private Patient currentPatient;
        private DataBaseController Controller;
        private DataBaseConnector Connector;
        private Logger logger = LogManager.GetCurrentClassLogger();

        public EditPatientPresenter(DataBaseConnector connector)
        {
            logger.Info("Connect to dataBase.");
            Controller = new DataBaseController(connector.Settings);
            Connector = connector;
        }

        internal List<Patient> LoadPatientListFromDataBase()
        {
            logger.Info("Loading patients from dataBase.");
            List<Patient> patientList = new List<Patient>();
            patientList.AddRange(Controller.GetPatients());
            return patientList;
        }

        internal void ShowPatient(PatientShowControl showController, SelectionChangedEventArgs e)
        {
            logger.Info("Showing patient");
            currentPatient = (Patient)e.AddedItems[0];
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
            editController.FirstNameTextBox.Text = currentPatient.FirstName;
            editController.SexComboBox.SelectedIndex = (int)currentPatient.Gender;
            editController.LastNameTextBox.Text = currentPatient.LastName;
            editController.MiddleNameTextBox.Text = currentPatient.MiddleName;
            editController.BirthdayPicker.Text = currentPatient.Date.ToLongDateString();
            editController.AmbulatorCardTextBox.Text = currentPatient.NumberAmbulatoryCard;
        } 
        
    }
}
    