using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltrasoundProtocols
{
	public class ExaminationType
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public ExaminationType(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}
	
	}
}
