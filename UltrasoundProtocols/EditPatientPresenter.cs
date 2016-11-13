using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UltrasoundProtocols
{
    class EditPatientPresenter
    {
        UltrasoundProtocolsDataSetSelector selector;
        public EditPatientPresenter()
        {
            selector = new UltrasoundProtocolsDataSetSelector();
        }
        internal List<Patient> LoadPatientListFromDataBase()
        {
            //TODO удалить дебаговые объекты
            //Вот не стоит этого пока что делать. Тк когда не подключается база данных, дебаговые объкты спасают
            List<Patient> patientList = new List<Patient>();
            patientList.Add(new Patient(0, "Александр", "Сергеевич", "Пушкин", PatientGender.Man, new DateTime(1799, 6, 6), "0"));
            patientList.Add(new Patient(1, "Петр", "Алексеевич", "Романов", PatientGender.Man, new DateTime(1672, 6, 9), "123"));
            patientList.AddRange(selector.getPatients());
            return patientList;
        }

        internal void ShowPatient(PatientShowControl showController, SelectionChangedEventArgs e)
        {
            Patient selectedItem = (Patient)e.AddedItems[0];
            showController.FirstNameTextBlock.Text = selectedItem.FirstName;
            showController.SexTextBox.Text = selectedItem.Gender.ToString();
            showController.LastNameTextBlock.Text = selectedItem.LastName;
            showController.MiddleNameTextBlock.Text = selectedItem.MiddleName;
            showController.BirthdayTextBlock.Text = selectedItem.Date.ToString();
            showController.AmbulatorCardTextBlock.Text = selectedItem.NumberAmbulatoryCard;
        }
    }
}
    