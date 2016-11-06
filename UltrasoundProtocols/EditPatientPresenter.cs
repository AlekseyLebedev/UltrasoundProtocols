using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            //TODO загрузить из базы данных лист с данными о пациентах
            List<Patient> patientList = new List<Patient>();
            patientList.Add(new Patient(0, "Александр", "Сергеевич", "Пушкин", PatientGender.Man, new DateTime(1799, 6, 6), "0"));
            patientList.Add(new Patient(1, "Петр", "Алексеевич", "Романов", PatientGender.Man, new DateTime(1672, 6, 9), "123"));
            patientList.AddRange(selector.getPatients());
            return patientList;
        }

    }
}
    