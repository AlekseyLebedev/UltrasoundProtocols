using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltrasoundProtocols
{
	public class Patient
	{
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public DateTime Date { get; set; }
        public string NumberAmbulatoryCard { get; set; }

        public Patient(int id, string FirstName, string MiddleName, string LastName,
            int gender, DateTime date, string numberAmbulatoryCard)
		{
            this.Id = id;
            this.FirstName = FirstName;
            this.MiddleName = MiddleName;
            this.LastName = LastName;
            this.Gender = gender;
            this.Date = date;
			this.NumberAmbulatoryCard = numberAmbulatoryCard;
		}
	}
}
