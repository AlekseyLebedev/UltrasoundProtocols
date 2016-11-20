using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using ProtocolTemplateLib;
using UltrasoundProtocols.UltraSoundProtocolsDBDataSetTableAdapters;
using NLog;

namespace UltrasoundProtocols
{
    public class DataBaseController
    {
        private DataBaseSettings Settings;

        public DataBaseController(DataBaseSettings settings)
        {
            Settings = settings;
        }

        public List<Doctor> GetActiveDoctors()
        {
            Logger.Debug("Request for active doctors");
            Tbl_DoctorsTableAdapter adapter =
                new Tbl_DoctorsTableAdapter(Settings);
            List<Doctor> doctors = (from table in adapter.GetData() where table.dct_status select table)
                .Select(x => new Doctor(x.dct_id, x.dct_firstname, x.dct_middlename, x.dct_lastname, x.dct_status))
                .ToList();
            Logger.Debug("Found {0} active doctors", doctors.Count);
            return doctors;
        }

        public Patient GetPatient(int id)
        {
            Tbl_PatientsTableAdapter adapter = new Tbl_PatientsTableAdapter(Settings);
            var patients = (from x in adapter.GetData()
                            where x.pat_id == id
                            select new Patient(x.pat_id, x.pat_firstname, x.pat_middlename, x.pat_lastname,
                            (PatientGender)x.pat_gender, x.pat_birthdate, x.pat_numberambulatorycard));
            return GetFirstOrNull(patients, id);
        }

        internal Doctor GetDoctor(int id)
        {
            Logger.Debug("Request for doctor {0}", id);
            Tbl_DoctorsTableAdapter adapter = new Tbl_DoctorsTableAdapter(Settings);
            var doctors = (from table in adapter.GetData()
                           where table.dct_id == id
                           select new Doctor(table.dct_id, table.dct_firstname, table.dct_middlename, table.dct_lastname, table.dct_status));
            return GetFirstOrNull(doctors, id);
        }

        public List<Patient> GetPatients()
        {
            Tbl_PatientsTableAdapter adapter = new Tbl_PatientsTableAdapter(Settings);
            List<Patient> patients = (from x in adapter.GetData()
                                      select new Patient(x.pat_id, x.pat_firstname, x.pat_middlename,
                                      x.pat_lastname, (PatientGender)x.pat_gender, x.pat_birthdate, x.pat_numberambulatorycard))
                .ToList();
            return patients;
        }

        public List<Protocol> GetProtocols(int id)
        {
            throw new NotImplementedException();
        }

        public List<ExaminationType> GetExaminationTypes()
        {
            Logger.Debug("Request for examination types");
            Tbl_ExaminationTypesTableAdapter adapter = new Tbl_ExaminationTypesTableAdapter(Settings);
            List<ExaminationType> examinationTypes = (from table in adapter.GetData()
                                                      select new ExaminationType(table.ext_id, table.ext_name))
                .ToList();
            Logger.Debug("Found {0} examinations types", examinationTypes.Count);
            return examinationTypes;
        }

        public List<MedicalEquipment> GetMedicalEquipments()
        {
            Logger.Debug("Request for medical equipment");
            Tbl_MedicalEquipmentsTableAdapter adapter = new Tbl_MedicalEquipmentsTableAdapter(Settings);
            List<MedicalEquipment> equipments = (from table in adapter.GetData()
                                                 select new MedicalEquipment(table.meq_id, table.meq_name))
                .ToList();
            Logger.Debug("Found {0} medical equipments", equipments.Count);
            return equipments;
        }

        private T GetFirstOrNull<T>(IEnumerable<T> collection, int id) where T : class
        {
            return GetFirstOrNull(collection, id, typeof(T).Name);
        }

        private T GetFirstOrNull<T>(IEnumerable<T> collection, int id, string type) where T : class
        {
            Logger.Debug("Searching for {0} id {1}", type, id);
            try
            {

                return collection.First();
            }
            catch (InvalidOperationException ex)
            {
                Logger.Error(ex, "Element {0} id {1} not found", type, id);
                return null;
            }
        }

        private static Logger Logger = LogManager.GetCurrentClassLogger();
    }
}
