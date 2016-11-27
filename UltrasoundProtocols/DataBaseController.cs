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
            using (Tbl_DoctorsTableAdapter adapter = new Tbl_DoctorsTableAdapter(Settings))
            {
                List<Doctor> doctors = (from table in adapter.GetData() where table.dct_status select ConvertDoctor(table))
                    .ToList();

                Logger.Debug("Found {0} active doctors", doctors.Count);
                return doctors;
            }
        }

        public Patient GetPatient(int id)
        {
            using (Tbl_PatientsTableAdapter adapter = new Tbl_PatientsTableAdapter(Settings))
            {
                var patients = (from x in adapter.GetData()
                                where x.pat_id == id
                                select ConvertPatient(x));
                return GetFirstOrNull(patients, id);
            }
        }

        internal MedicalEquipment GetMedicalEquipment(int id)
        {
            Logger.Debug("Request for mediacal equipment {0}", id);
            using (Tbl_MedicalEquipmentsTableAdapter adapter = new Tbl_MedicalEquipmentsTableAdapter(Settings))
            {
                var equipments = (from table in adapter.GetData()
                                  where table.meq_id == id
                                  select ConvertMedicalEquipment(table));
                return GetFirstOrNull(equipments, id);
            }
        }

        internal Doctor GetDoctor(int id)
        {
            Logger.Debug("Request for doctor {0}", id);
            using (Tbl_DoctorsTableAdapter adapter = new Tbl_DoctorsTableAdapter(Settings))
            {
                var doctors = (from table in adapter.GetData()
                               where table.dct_id == id
                               select ConvertDoctor(table));
                return GetFirstOrNull(doctors, id);
            }
        }

        public List<Patient> GetPatients()
        {
            Logger.Debug("Request for patrients");
            using (Tbl_PatientsTableAdapter adapter = new Tbl_PatientsTableAdapter(Settings))
            {
                List<Patient> patients = (from x in adapter.GetData() select ConvertPatient(x))
                    .ToList();
                Logger.Debug("Found {0} patrients", patients.Count);
                return patients;
            }
        }

        public List<Protocol> GetProtocols(int id)
        {
            // TODO
            throw new NotImplementedException();
        }

        public List<Template> GetExaminationTypes()
        {
            Logger.Debug("Request for templates");
            using (Tbl_TemplatesTableAdapter adapter = new Tbl_TemplatesTableAdapter(Settings))
            {
                List<Template> examinationTypes = (from table in adapter.GetData()
                                                   select ConvertTemplate(table)).ToList();
                Logger.Debug("Found {0} templates", examinationTypes.Count);
                return examinationTypes;
            }
        }

        public List<MedicalEquipment> GetMedicalEquipments()
        {
            Logger.Debug("Request for medical equipment");
            using (Tbl_MedicalEquipmentsTableAdapter adapter = new Tbl_MedicalEquipmentsTableAdapter(Settings))
            {
                List<MedicalEquipment> equipments = (from table in adapter.GetData() select ConvertMedicalEquipment(table))
                    .ToList();
                Logger.Debug("Found {0} medical equipments", equipments.Count);
                return equipments;
            }
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

        private static Patient ConvertPatient(UltraSoundProtocolsDBDataSet.Tbl_PatientsRow x)
        {
            return new Patient(x.pat_id, x.pat_firstname, x.pat_middlename, x.pat_lastname,
                                        (PatientGender)x.pat_gender, x.pat_birthdate, x.pat_numberambulatorycard);
        }

        private static Doctor ConvertDoctor(UltraSoundProtocolsDBDataSet.Tbl_DoctorsRow table)
        {
            return new Doctor(table.dct_id, table.dct_firstname, table.dct_middlename, table.dct_lastname, table.dct_status);
        }

        private static MedicalEquipment ConvertMedicalEquipment(UltraSoundProtocolsDBDataSet.Tbl_MedicalEquipmentsRow table)
        {
            return new MedicalEquipment(table.meq_id, table.meq_name);
        }

        private static Template ConvertTemplate(UltraSoundProtocolsDBDataSet.Tbl_TemplatesRow table)
        {
            return Template.GetFromDatabaseEntry(table.tem_name, table.tem_id, table.tem_template);
        }

        private static Logger Logger = LogManager.GetCurrentClassLogger();
    }
}
