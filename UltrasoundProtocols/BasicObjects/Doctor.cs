using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltrasoundProtocols
{
	public class Doctor
	{
		private int id;
		private string name;
		private bool status;

		public Doctor(int id, string name, bool status)
		{
			this.id = id;
			this.name = name;
			this.status = status;
		}

		public int getId()
		{
			return id;
		}

		public string getName()
		{
			return name;
		}

		public bool getStatus()
		{
			return status;
		}
	}
}
