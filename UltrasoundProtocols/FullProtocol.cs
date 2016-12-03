using NLog;
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
        public DateTime Date { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int EquipmentId { get; set; }
        public string Source { get; set; }

        public List<Protocol> Protocols { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public MedicalEquipment Equipment { get; set; }

        public FullProtocol()
        {
            Id = -1;
            Date = DateTime.Now;
            DoctorId = 0;
            PatientId = 0;
            EquipmentId = 0;
            Source = "";
        }

        public FullProtocol(int id, DateTime dateTime, int doctor, int patient, int equipment, string source)
        {
            this.Id = id;
            this.Date = dateTime;
            this.DoctorId = doctor;
            this.PatientId = patient;
            this.EquipmentId = equipment;
            this.Source = source;
            Protocols = null;
        }

        public void Load(DataBaseController controller)
        {
            Protocols = controller.GetProtocols(Id);
            Doctor = controller.GetDoctor(DoctorId);
            Equipment = controller.GetMedicalEquipment(EquipmentId);
            Patient = controller.GetPatient(PatientId);
        }

        private const string BEGIN_MARKED_TAG = "<strong>";
        private const string END_MARKED_TAG = "</strong>";
        private const string NEW_LINE_TAG = "<br />";

        public string PrintToProtocol()
        {
            if (Protocols == null || Patient == null || Doctor == null)
            {
                throw new NotSupportedException("Can't print protocols, which not loaded");
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<html>");
            builder.AppendLine("<head><meta charset=\"utf-8\"></head>");
            builder.AppendLine("<body>");
            builder.AppendLine("<h1>Протокол ульразвукового исследования</h1>");
            builder.AppendFormat("{0}Пациент:{1} {2} {3} {4}", BEGIN_MARKED_TAG, END_MARKED_TAG, Patient.FirstName, Patient.MiddleName, Patient.LastName);
            builder.AppendLine(NEW_LINE_TAG);
            builder.AppendFormat("{0}Дата исследования:{1} {2:dd.MM.yyyy}", BEGIN_MARKED_TAG, END_MARKED_TAG, Date);
            builder.AppendLine(NEW_LINE_TAG);
            if (Source != null)
            {
                builder.AppendFormat("{0}Направление:{1} {2}", BEGIN_MARKED_TAG, END_MARKED_TAG, Source);
                builder.AppendLine(NEW_LINE_TAG);
            }
            if (Equipment != null)
            {
                builder.AppendFormat("{0}Исследование проводилось на оборудовании:{1} {2}", BEGIN_MARKED_TAG, END_MARKED_TAG, Equipment.Name);
                builder.AppendLine(NEW_LINE_TAG);
            }
            foreach (Protocol item in Protocols)
            {
                item.PrintToProtocol(builder);
            }
            builder.AppendLine(NEW_LINE_TAG);
            builder.AppendFormat("{0}Врач:{1} {2} {3} {4} _____", BEGIN_MARKED_TAG, END_MARKED_TAG, Doctor.FirstName, Doctor.MiddleName, Doctor.LastName);
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");
            string html = builder.ToString();
            Logger.Debug("Html: {0}", html);
            return html;
        }

        private static Logger Logger = LogManager.GetCurrentClassLogger();

    }
}
