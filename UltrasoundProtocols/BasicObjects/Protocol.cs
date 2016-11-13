using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltrasoundProtocols
{
    public class FullProtocol
    {
        private int Id { get; set; }
        private DateTime DateTime { get; set; }
        private int Doctor { get; set; }
        private int Patient { get; set; }
        private int Equipment { get; set; }
        private string Source { get; set; }

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
