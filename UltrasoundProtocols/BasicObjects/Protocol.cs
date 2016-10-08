using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltrasoundProtocols
{
	class FullProtocol
	{
		//пока что это заглушка
		int id;
		DateTime dateTime;
		string doctor;
		string patient;
		string equipment;
		string source;
		public FullProtocol(int id, DateTime dateTime, string doctor, string patient, string equipment, string source)
		{
			this.id = id;
			this.dateTime = dateTime;
			this.doctor = doctor;
			this.patient = patient;
			this.equipment = equipment;
			this.source = source;
		}
	}

	class Protocol
	{
		private int id;
		private DateTime dateTime;
		private int doctor;
		private int patient;
		private int equipment;
		private string source;

		public Protocol(int id, DateTime dateTime, int doctor, int patient, int equipment, string source)
		{
			this.id = id;
			this.dateTime = dateTime;
			this.doctor = doctor;
			this.patient = patient;
			this.equipment = equipment;
			this.source = source;
		}

		public int getId()
		{
			return id;
		}

		public DateTime getDateTime()
		{
			return dateTime;
		}

		public int getDoctorId()
		{
			return doctor;
		}

		public int getPatientId()
		{
			return patient;
		}

		public int getEquipmentId()
		{
			return equipment;
		}

		public string getSource()
		{
			return source;
		}
	}
}
