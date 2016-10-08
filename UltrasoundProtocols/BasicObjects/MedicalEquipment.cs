using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltrasoundProtocols
{
	class MedicalEquipment
	{
		private int id;
		private string name;

		public MedicalEquipment(int id, string name)
		{
			this.id = id;
			this.name = name;
		}

		public int getId()
		{
			return id;
		}

		public string getName()
		{
			return name;
		}
	}
}
