using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltrasoundProtocols
{
	class ExaminationType
	{
		private int Id { get; set; }
		private string Name { get; set; }

		public ExaminationType(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}
	
	}
}
