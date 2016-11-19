using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltrasoundProtocols
{
	public class Doctor
	{
		public int Id { get; set; }
		public string Firstname { get; set; }
		public string Middlename { get; set; }
		public string Lastname { get; set; }
		public bool Status { get; set; }

		public Doctor(int id, string firstname, string middlename, string lastname, bool status)
		{
			this.Id = id;
			this.Firstname = firstname;
			this.Middlename = middlename;
			this.Lastname = lastname;
			this.Status = status;
		}
	}
}
