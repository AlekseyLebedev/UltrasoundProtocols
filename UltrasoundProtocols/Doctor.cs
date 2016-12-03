using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltrasoundProtocols
{
	public class Doctor
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public bool Status { get; set; }

		public Doctor(int id, string firstname, string middlename, string lastname, bool status)
		{
			this.Id = id;
			this.FirstName = firstname;
			this.MiddleName = middlename;
			this.LastName = lastname;
			this.Status = status;
		}

        public override string ToString()
        {
            return String.Format("{0} {1} {2}", LastName, FirstName, MiddleName);
        }
    }
}
