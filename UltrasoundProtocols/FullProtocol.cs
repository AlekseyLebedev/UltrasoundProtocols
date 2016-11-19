using ProtocolTemplateLib;
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

        public List<Protocol> Protocols { get; set; }

        public FullProtocol(int id, DateTime dateTime, int doctor, int patient, int equipment, string source)
        {
            this.Id = id;
            this.DateTime = dateTime;
            this.Doctor = doctor;
            this.Patient = patient;
            this.Equipment = equipment;
            this.Source = source;
            Protocols = null;
        }

        public List<Protocol> LoadProtocols(DataBaseController controller)
        {
            Protocols = controller.GetProtocols(Id);
            return Protocols;
        }

        public string PrintToProtocol()
        {
            StringBuilder builder = new StringBuilder();
            if (Protocols == null)
            {
                throw new NotSupportedException("Can't print protocols, which not loaded");
            }
            return builder.ToString();
        }
    }
}
