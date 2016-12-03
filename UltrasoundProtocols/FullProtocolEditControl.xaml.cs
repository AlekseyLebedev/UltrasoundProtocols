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
using ProtocolTemplateLib;

namespace UltrasoundProtocols
{
    /// <summary>
    /// Interaction logic for EditFullProtocolUserControl.xaml
    /// </summary>
    public partial class FullProtocolEditControl : UserControl
    {
        private static string TAG = "EditFullProtocolUserControl";

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private FullProtocol FullProtocol_;
        public FullProtocol FullProtocol
        {
            get
            {
                return FullProtocol_;
            }

            set
            {
                FullProtocol_ = value;
                LoadFieldsAsync();
            }
        }

        private List<MedicalEquipment> Equipments;
        private List<Doctor> Doctors;
        public Patient Patient { get; set; }

        public event EventHandler<SaveButtonClickEventArgs> SaveButtonClick;

        public DataBaseController Controller { get; set; }

        public FullProtocolEditControl()
        {
            InitializeComponent();
        }

        //Загружает асинхронно данные из бд
        private void LoadFieldsAsync()
        {
            IsEnabled = false;
            GuiAsyncTask task = new GuiAsyncTask();
            task.AsyncTask = LoadFields;
            task.SyncTask = applyFieldsToViews;
            task.Logger = logger;
            task.InfoMessage = TAG;
            task.Dispatcher = Dispatcher;
            task.ErrorTitle = "Ошибка загрузки из БД";
            task.RetryEnabled = true;
            task.Run();
        }


        //Подгружает данные из бд
        private void LoadFields()
        {
            Equipments = Controller.GetMedicalEquipments();
            Doctors = Controller.GetActiveDoctors();
        }

        //Применяет данные из бд к views
        private void applyFieldsToViews()
        {
            PatientName.Text = Patient.FirstName + " " + Patient.MiddleName + " " + Patient.LastName;

            SourceTextBox.Text = FullProtocol_.Source;

            int doctorIndexInCombobox = 0;
            for (int i = 0; i < Doctors.Count; ++i)
            {
                Doctor doctor = Doctors[i];
                DoctorsComboBox.Items.Add(doctor.FirstName);
                if (doctor.Id == FullProtocol_.DoctorId)
                {
                    doctorIndexInCombobox = i;
                }
            }
            DoctorsComboBox.SelectedIndex = doctorIndexInCombobox;

            int equipmentIndexInCombobox = 0;
            for (int i = 0; i < Equipments.Count; ++i)
            {
                MedicalEquipment equipment = Equipments[i];
                EquipmentsComboBox.Items.Add(equipment.Name);
                if (equipment.Id == FullProtocol_.EquipmentId)
                {
                    equipmentIndexInCombobox = i;
                }
            }
            EquipmentsComboBox.SelectedIndex = equipmentIndexInCombobox;

            DatePicker.Value = FullProtocol_.Date;

            IsEnabled = true;
        }

        private void ApplyViewsDataToProtocol()
        {
            FullProtocol_.Source = SourceTextBox.Text;
            FullProtocol_.DoctorId = Doctors[DoctorsComboBox.SelectedIndex].Id;
            FullProtocol_.EquipmentId = Equipments[EquipmentsComboBox.SelectedIndex].Id;
            FullProtocol_.Source = SourceTextBox.Text;
            FullProtocol_.Date = DatePicker.Value;
        }

        private void OutToLogger()
        {
            logger.Debug("Source: " + FullProtocol_.Source);
            logger.Debug("Doctor id: " + FullProtocol_.DoctorId);
            logger.Debug("Patient id: " + FullProtocol_.PatientId);
            logger.Debug("Equipment id" + FullProtocol_.EquipmentId);
            logger.Debug("Date: " + FullProtocol_.Date);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ApplyViewsDataToProtocol();
            OutToLogger();
            SaveButtonClick(this, new SaveButtonClickEventArgs(FullProtocol_));
        }

        public class SaveButtonClickEventArgs : EventArgs
        {
            public FullProtocol Value { get; private set; }
            public SaveButtonClickEventArgs(FullProtocol value)
            {
                Value = value;
            }
        }

    }
}
