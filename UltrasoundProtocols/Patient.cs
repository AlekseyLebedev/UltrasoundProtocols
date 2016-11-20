using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace UltrasoundProtocols
{
	public enum PatientGender
	{
        [Description("Женщина")]
        Woman = 0,
        [Description("Мужчина")]
        Man = 1
	};
    public class PatientEnumDescriptionValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = typeof(PatientGender);
            var name = Enum.GetName(type, value);
            System.Reflection.FieldInfo fi = type.GetField(name);
            var descriptionAttrib = (DescriptionAttribute)
                Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

            return descriptionAttrib.Description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class Patient
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public PatientGender Gender { get; set; }
		public DateTime Date { get; set; }
		public string NumberAmbulatoryCard { get; set; }

		public Patient(int id, string FirstName, string MiddleName, string LastName,
			PatientGender gender, DateTime date, string numberAmbulatoryCard)
		{
			this.Id = id;
			this.FirstName = FirstName;
			this.MiddleName = MiddleName;
			this.LastName = LastName;
			this.Gender = gender;
			this.Date = date;
			this.NumberAmbulatoryCard = numberAmbulatoryCard;
		}
	}
}
