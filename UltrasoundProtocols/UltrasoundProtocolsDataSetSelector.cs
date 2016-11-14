using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace UltrasoundProtocols
{
    class UltrasoundProtocolsDataSetSelector
    {
        public static List<Doctor> getActiveDoctors()
        {
			UltraSoundProtocolsDBDataSetTableAdapters.Tbl_DoctorsTableAdapter adapter =
                new UltraSoundProtocolsDBDataSetTableAdapters.Tbl_DoctorsTableAdapter();
            List<Doctor> doctors = (from table in adapter.GetData() where table.dct_status select table)
                .Select(x => new Doctor(x.dct_id, x.dct_firstname, x.dct_middlename, x.dct_lastname, x.dct_status))
                .ToList();
            return doctors;
        }

        public static Patient getPatient(int id)
        {
            UltraSoundProtocolsDBDataSetTableAdapters.Tbl_PatientsTableAdapter adapter =
                new UltraSoundProtocolsDBDataSetTableAdapters.Tbl_PatientsTableAdapter();
            List<Patient> patients = (from table in adapter.GetData() where table.pat_id == id select table)
                .Select(x => new Patient(x.pat_id, x.pat_firstname, x.pat_middlename, x.pat_lastname, (PatientGender)x.pat_gender, x.pat_birthdate, x.pat_numberambulatorycard))
                .ToList();

            if (patients.Count == 1)
            {
                return patients[0];
            }
            else
            {
                return null;
            }
        }

        public static List<Patient> getPatients()
        {
			UltraSoundProtocolsDBDataSetTableAdapters.Tbl_PatientsTableAdapter adapter =
                new UltraSoundProtocolsDBDataSetTableAdapters.Tbl_PatientsTableAdapter();
            List<Patient> patients = (from table in adapter.GetData() select table)
                .Select(x => new Patient(x.pat_id, x.pat_firstname, x.pat_middlename, x.pat_lastname, (PatientGender)x.pat_gender, x.pat_birthdate, x.pat_numberambulatorycard))
                .ToList();
            return patients;
        }

        public static List<ExaminationType> getExaminationTypes()
        {
			UltraSoundProtocolsDBDataSetTableAdapters.Tbl_ExaminationTypesTableAdapter adapter =
                new UltraSoundProtocolsDBDataSetTableAdapters.Tbl_ExaminationTypesTableAdapter();
            List<ExaminationType> examinationTypes = (from table in adapter.GetData() select table)
                .Select(x => new ExaminationType(x.ext_id, x.ext_name))
                .ToList();
            return examinationTypes;
        }

        public static List<MedicalEquipment> getMedicalEquipments()
        {
			UltraSoundProtocolsDBDataSetTableAdapters.Tbl_MedicalEquipmentsTableAdapter adapter =
                new UltraSoundProtocolsDBDataSetTableAdapters.Tbl_MedicalEquipmentsTableAdapter();
            List<MedicalEquipment> equipments = (from table in adapter.GetData() select table)
                .Select(x => new MedicalEquipment(x.meq_id, x.meq_name))
                .ToList();
            return equipments;
        }

        public static List<FullProtocol> getProtocols()
        {
			UltraSoundProtocolsDBDataSetTableAdapters.Tbl_ProtocolsTableAdapter adapter =
                new UltraSoundProtocolsDBDataSetTableAdapters.Tbl_ProtocolsTableAdapter();
            List<FullProtocol> protocols = (from table in adapter.GetData() select table)
                .Select(x => new FullProtocol(x.prt_id, x.prt_datetime, x.prt_doctor, x.prt_patient, x.prt_equipment, x.prt_source))
                .ToList();
            return protocols;
        }
        public static List<FullProtocol> getFullFilledProtocols()
        {
			UltraSoundProtocolsDBDataSetTableAdapters.Tbl_ProtocolsTableAdapter adapterProtocols =
                new UltraSoundProtocolsDBDataSetTableAdapters.Tbl_ProtocolsTableAdapter();
			UltraSoundProtocolsDBDataSetTableAdapters.Tbl_DoctorsTableAdapter adapterDoctors =
                new UltraSoundProtocolsDBDataSetTableAdapters.Tbl_DoctorsTableAdapter();
			UltraSoundProtocolsDBDataSetTableAdapters.Tbl_PatientsTableAdapter adapterPatients =
                new UltraSoundProtocolsDBDataSetTableAdapters.Tbl_PatientsTableAdapter();
			UltraSoundProtocolsDBDataSetTableAdapters.Tbl_MedicalEquipmentsTableAdapter adapterEquipments =
                new UltraSoundProtocolsDBDataSetTableAdapters.Tbl_MedicalEquipmentsTableAdapter();
            var fullFilledProtocols = (from protocols in adapterProtocols.GetData()
                                       join doctors in adapterDoctors.GetData() on protocols.prt_doctor equals doctors.dct_id
                                       join patients in adapterPatients.GetData() on protocols.prt_patient equals patients.pat_id
                                       join equipments in adapterEquipments.GetData() on protocols.prt_equipment equals equipments.meq_id
                                       select new
                                       {
                                           Id = protocols.prt_id,
                                           DateTime = protocols.prt_datetime,
                                           Doctor = doctors.dct_firstname + doctors.dct_middlename + doctors.dct_lastname,
                                           Patient = patients.pat_firstname + patients.pat_middlename + patients.pat_lastname,
                                           Equipment = equipments.meq_name,
                                           Source = protocols.prt_source
                                       })
                .Select(x => new FullProtocol(x.Id, x.DateTime, int.Parse(x.Doctor), int.Parse(x.Patient),
                int.Parse(x.Equipment), x.Source))
                .ToList();
            return fullFilledProtocols;
        }
    }
}
