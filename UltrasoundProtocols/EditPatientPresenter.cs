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

        public EditPatientPresenter(DataBaseConnector connector)
        {
            Controller = new DataBaseController(connector.Settings);
            Connector = connector;
        }

        internal List<Patient> LoadPatientListFromDataBase()
        {
            //TODO удалить дебаговые объекты
            List<Patient> patientList = new List<Patient>();
            patientList.Add(new Patient(0, "Александр", "Сергеевич", "Пушкин", PatientGender.Man, new DateTime(1799, 6, 6), "0"));
            patientList.Add(new Patient(1, "Петр", "Алексеевич", "Романов", PatientGender.Man, new DateTime(1672, 6, 9), "123"));
			patientList.AddRange(Controller.GetPatients());
            return patientList;
        }

        internal void ShowPatient(PatientShowControl showController, SelectionChangedEventArgs e)
        {
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
    