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
using NLog;

namespace UltrasoundProtocols
{
    /// <summary>
    /// Interaction logic for EditPatientUserControl.xaml
    /// </summary>
    public partial class EditPatientUserControl : UserControl
    {
        private static string TAG = "EditPatientUserControl";

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private Patient Patient_;
        public Patient Patient
        {
            get 
            {
                return Patient_;
            }

            set
            {
                Patient_ = value;
                ApplyPatientFields();
            }
        }

        public delegate void OnSaveButtonClick(Patient patient);

        public event OnSaveButtonClick onSaveButtonClick;

        public EditPatientUserControl()
        {
            InitializeComponent();
            InitializeViews();
        }

        private void InitializeViews() {
            FirstNameTextBox.Text = "";
            MiddleNameTextBox.Text = "";
            LastNameTextBox.Text = "";
            BirthdayPicker.Value = DateTime.Today;
            AmbulatorCardTextBox.Text = "";

            SexComboBox.Items.Add("Женский");
            SexComboBox.Items.Add("Мужской");
        }

        private void ApplyPatientFields() {
            FirstNameTextBox.Text = Patient_.FirstName;
            MiddleNameTextBox.Text = Patient_.MiddleName;
            LastNameTextBox.Text = Patient_.LastName;

            AmbulatorCardTextBox.Text = Patient_.NumberAmbulatoryCard;

            if (Patient_.Gender == PatientGender.Man)
            {
                SexComboBox.SelectedIndex = 1;
            }
            else
            {
                SexComboBox.SelectedIndex = 0;
            }

            BirthdayPicker.Value = Patient_.Date;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ApplyFieldsToPatient();
            OutToLog();
            if (onSaveButtonClick != null)
            {
                onSaveButtonClick(Patient_);
            }
        }

        private void ApplyFieldsToPatient()
        {
            Patient_.FirstName = FirstNameTextBox.Text;
            Patient_.MiddleName = MiddleNameTextBox.Text;
            Patient_.LastName = LastNameTextBox.Text;

            Patient_.NumberAmbulatoryCard = AmbulatorCardTextBox.Text;

            if (SexComboBox.SelectedIndex == 1)
            {
                Patient_.Gender = PatientGender.Man;
            }
            else
            {
                Patient_.Gender = PatientGender.Woman;
            }

            Patient_.Date = BirthdayPicker.Value;
        }

        private void OutToLog()
        {
            logger.Debug(TAG, "first name: " + Patient_.FirstName);
            logger.Debug(TAG, "middle name: " + Patient_.MiddleName);
            logger.Debug(TAG, "last name: " + Patient_.LastName);
            logger.Debug(TAG, "NumberAmbulatoryCard: " + Patient_.NumberAmbulatoryCard);
            logger.Debug(TAG, "Gender: " + Patient_.Gender);
            logger.Debug(TAG, "Date: " + Patient_.Date);
            logger.Debug(TAG, "Id: " + Patient_.Id);
        }

		private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
		{

		}
	}
}
