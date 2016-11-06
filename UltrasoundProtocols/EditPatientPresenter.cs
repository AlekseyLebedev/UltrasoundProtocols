using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltrasoundProtocols
{
    class EditPatientPresenter
    {
        internal List<Patient> LoadPatientListFromDataBase()
        {
            //TODO загрузить из базы данных лист с данными о пациентах
            List<Patient> patientList = new List<Patient>();
            patientList.Add(new Patient(0, "Александр", "Сергеевич", "Пушкин", 1,new DateTime(1799, 6, 6), "0"));
            patientList.Add(new Patient(1, "Петр", "Алексеевич", "Романов", 1, new DateTime(1672, 6, 9), "123"));
            return patientList;
        }

    }
}
