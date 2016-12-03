using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UltrasoundProtocols
{
    /// <summary>
    /// Логика взаимодействия для PatientShowController.xaml
    /// </summary>
    public partial class PatientInfoControl : UserControl
    {
        public PatientInfoControl()
        {
            InitializeComponent();
        }

        public Patient CurrentPatient
        {
            get
            {
                return CurrentPatient_;
            }
            set
            {
                FirstNameTextBlock.Text = value.FirstName;
                SexTextBox.Text = value.Gender.ToString();
                LastNameTextBlock.Text = value.LastName;
                MiddleNameTextBlock.Text = value.MiddleName;
                BirthdayTextBlock.Text = value.BirthDate.ToShortDateString();
                AmbulatorCardTextBlock.Text = value.NumberAmbulatoryCard;
                CurrentPatient_ = value;
            }
        }

        private Patient CurrentPatient_;
    }
}
