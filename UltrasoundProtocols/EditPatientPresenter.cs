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
            Patient selectedItem = (Patient)e.AddedItems[0];
            showController.FirstNameTextBlock.Text = selectedItem.FirstName;
            showController.SexTextBox.Text = selectedItem.Gender.ToString();
            showController.LastNameTextBlock.Text = selectedItem.LastName;
            showController.MiddleNameTextBlock.Text = selectedItem.MiddleName;
			showController.BirthdayTextBlock.Text = selectedItem.Date.ToShortDateString();
            showController.AmbulatorCardTextBlock.Text = selectedItem.NumberAmbulatoryCard;
        }

		internal string GetDateString(DateTime dateTime)
		{
			StringBuilder date = new StringBuilder()
				.Append(dateTime.Day)
				.Append(dateTime.Month)
				.Append(dateTime.Year);
			return date.ToString();
		}
    }
}
    