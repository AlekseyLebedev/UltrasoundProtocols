using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltrasoundProtocols
{
    public class FullProtocol
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int Doctor { get; set; }
        public int Patient { get; set; }
        public int Equipment { get; set; }
        public string Source { get; set; }

        public FullProtocol(int id, DateTime dateTime, int doctor, int patient, int equipment, string source)
        {
            this.Id = id;
            this.DateTime = dateTime;
            this.Doctor = doctor;
            this.Patient = patient;
            this.Equipment = equipment;
            this.Source = source;
        }
    }
}
