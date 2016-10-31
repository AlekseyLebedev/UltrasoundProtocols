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
    /// Interaction logic for EditPatientUserControl.xaml
    /// </summary>
    public partial class EditPatientUserControl : UserControl
    {
        private Patient Patient_
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
            BirthdayTextBox.Text = "";
            AmbulatorCardTextBox.Text = "";

            SexComboBox.Items.Add("Мужской");
            SexComboBox.Items.Add("Женский");
        }

        private void ApplyPatientFields() {
            FirstNameTextBox.Text = Patient_.FirstName;
            MiddleNameTextBox.Text = Patient_.MiddleName;
            LastNameTextBox.Text = Patient_.LastName;

            AmbulatorCardTextBox.Text = Patient_.NumberAmbulatoryCard;

            if (Patient_.Gender == 0)
            {
                SexComboBox.SelectedIndex = 0;
            }
            else
            {
                SexComboBox.SelectedIndex = 1;
            }

            BirthdayTextBox.Text = Patient_.Date.ToString();

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            onSaveButtonClick(Patient_);
        }
    }
}
