using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltrasoundProtocols
{
	class ExaminationType
	{
		private int id;
		private string name;

		public ExaminationType(int id, string name)
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
