using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltrasoundProtocols
{
	class Patient
	{
		private int id;
		private string name;
		private int gender;
		private DateTime date;
		private string numberAmbulatoryCard;

		public Patient(int id, string name, int gender, DateTime date, string numberAmbulatoryCard)
		{
			this.id = id;
			this.name = name;
			this.gender = gender;
			this.date = date;
			this.numberAmbulatoryCard = numberAmbulatoryCard;
		}

		public int getId()
		{
			return id;
		}

		public string getName()
		{
			return name;
		}

		public int getGender()
		{
			return gender;
		}

		public DateTime getDate()
		{
			return date;
		}

		public string getNumberAmbulatoryCard()
		{
			return numberAmbulatoryCard;
		}
	}
}
