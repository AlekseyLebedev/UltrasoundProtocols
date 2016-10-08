using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace UltrasoundProtocols
{
	class UltrasoundProtocolsDataSetSelector
	{
		public List<Doctor> getActiveDoctors() {
			UltrasoundProtocolsDataSetTableAdapters.Tbl_DoctorsTableAdapter adapter =
				new UltrasoundProtocolsDataSetTableAdapters.Tbl_DoctorsTableAdapter();
			List<Doctor> doctors = (from table in adapter.GetData() where table.dct_status select table)
				.Select(x=>new Doctor(x.dct_id, x.dct_fullname, x.dct_status))
				.ToList();
			return doctors;
		}

		public List<Patient> getPatients()
		{
			UltrasoundProtocolsDataSetTableAdapters.Tbl_PatientsTableAdapter adapter =
				new UltrasoundProtocolsDataSetTableAdapters.Tbl_PatientsTableAdapter();
			List<Patient> patients = (from table in adapter.GetData() select table)
				.Select(x => new Patient(x.pat_id, x.pat_fullname, x.pat_gender, x.pat_birthdate, x.pat_numberambulatorycard))
				.ToList();
			return patients;
		}

		public List<ExaminationType> getExaminationTypes()
		{
			UltrasoundProtocolsDataSetTableAdapters.Tbl_ExaminationTypesTableAdapter adapter =
				new UltrasoundProtocolsDataSetTableAdapters.Tbl_ExaminationTypesTableAdapter();
			List<ExaminationType> examinationTypes = (from table in adapter.GetData() select table)
				.Select(x => new ExaminationType(x.ext_id, x.ext_name))
				.ToList();
			return examinationTypes;
		}

		public List<MedicalEquipment> getMedicalEquipments()
		{
			UltrasoundProtocolsDataSetTableAdapters.Tbl_MedicalEquipmentsTableAdapter adapter =
				new UltrasoundProtocolsDataSetTableAdapters.Tbl_MedicalEquipmentsTableAdapter();
			List<MedicalEquipment> equipments = (from table in adapter.GetData() select table)
				.Select(x => new MedicalEquipment(x.meq_id, x.meq_name))
				.ToList();
			return equipments;
		}

		public List<Protocol> getProtocols()
		{
			UltrasoundProtocolsDataSetTableAdapters.Tbl_ProtocolsTableAdapter adapter =
				new UltrasoundProtocolsDataSetTableAdapters.Tbl_ProtocolsTableAdapter();
			List<Protocol> protocols = (from table in adapter.GetData() select table)
				.Select(x => new Protocol(x.prt_id, x.prt_datetime, x.prt_doctor, x.prt_patient, x.prt_equipment, x.prt_source))
				.ToList();
			return protocols;
		}
		public List<FullProtocol> getFullFilledProtocols()
		{
			UltrasoundProtocolsDataSetTableAdapters.Tbl_ProtocolsTableAdapter adapterProtocols =
				new UltrasoundProtocolsDataSetTableAdapters.Tbl_ProtocolsTableAdapter();
			UltrasoundProtocolsDataSetTableAdapters.Tbl_DoctorsTableAdapter adapterDoctors =
				new UltrasoundProtocolsDataSetTableAdapters.Tbl_DoctorsTableAdapter();
			UltrasoundProtocolsDataSetTableAdapters.Tbl_PatientsTableAdapter adapterPatients =
				new UltrasoundProtocolsDataSetTableAdapters.Tbl_PatientsTableAdapter();
			UltrasoundProtocolsDataSetTableAdapters.Tbl_MedicalEquipmentsTableAdapter adapterEquipments =
				new UltrasoundProtocolsDataSetTableAdapters.Tbl_MedicalEquipmentsTableAdapter();
			var fullFilledProtocols = (from protocols in adapterProtocols.GetData()
									   join doctors in adapterDoctors.GetData() on protocols.prt_doctor equals doctors.dct_id
									   join patients in adapterPatients.GetData() on protocols.prt_patient equals patients.pat_id
									   join equipments in adapterEquipments.GetData() on protocols.prt_equipment equals equipments.meq_id
									   select new { Id = protocols.prt_id, DateTime = protocols.prt_datetime, Doctor = doctors.dct_fullname,
									   Patient = patients.pat_fullname, Equipment = equipments.meq_name, Source = protocols.prt_source})
				.Select(x => new FullProtocol(x.Id, x.DateTime, x.Doctor, x.Patient, x.Equipment, x.Source))
				.ToList();
			return fullFilledProtocols;
		}
	}
}
