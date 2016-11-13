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
    /// Interaction logic for EditFullProtocolUserControl.xaml
    /// </summary>
    public partial class EditFullProtocolUserControl : UserControl
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
        private Patient Patient;

        public delegate void OnSaveButtonClick(FullProtocol protocol);

        public event OnSaveButtonClick onSaveButtonClick;

        public EditFullProtocolUserControl()
        {
            InitializeComponent();
        }

        //Загружает асинхронно данные из бд
        private void LoadFieldsAsync()
        {
            GuiAsyncTask task = new GuiAsyncTask();
            task.AsyncTask = LoadFields;
            task.CustomExceptionAction = ShowErrorBox;
            task.SyncTask = applyFieldsToViews;
            task.Logger = logger;
            task.InfoMessage = TAG;
            task.Dispatcher = Dispatcher;
            task.Run();
        }

        //показать ошибку загрузки данных из бд
        private void ShowErrorBox(Exception exc)
        {
            MessageBoxResult dialogResult = MessageBox.Show(
                exc.Message + "\nПроизошла ошибка при загрузке из базы данных",
                "Ошибка базы данных",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        //Подгружает данные из бд
        private void LoadFields()
        {
            Equipments = UltrasoundProtocolsDataSetSelector.getMedicalEquipments();
            Doctors = UltrasoundProtocolsDataSetSelector.getActiveDoctors();
            Patient = UltrasoundProtocolsDataSetSelector.getPatient(FullProtocol_.Patient);
        }

        private void ShowPatientLoadError()
        {
            MessageBoxResult dialogResult = MessageBox.Show(
                "Пациент не загрузился :(",
                "Ошибка базы данных",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        //Применяет данные из бд к views
        private void applyFieldsToViews()
        {
            if (Patient == null)
            {
                ShowPatientLoadError();
            }
            else 
            {
                PatientName.Content = Patient.FirstName + " " + Patient.MiddleName + " " + Patient.LastName;
            }

            SourceTextBox.Text = FullProtocol_.Source;

            int doctorIndexInCombobox = 0;
            for (int i = 0; i < Doctors.Count; ++i) {
                Doctor doctor = Doctors[i];
                DoctorsComboBox.Items.Add(doctor.getName());
                if (doctor.getId() == FullProtocol_.Doctor)
                {
                    doctorIndexInCombobox = i;
                }
            }
            DoctorsComboBox.SelectedIndex = doctorIndexInCombobox;

            int equipmentIndexInCombobox = 0;
            for (int i = 0; i < Equipments.Count; ++i)
            {
                MedicalEquipment equipment = Equipments[i];
                EquipmentsComboBox.Items.Add(equipment.getName());
                if (equipment.getId() == FullProtocol_.Equipment)
                {
                    equipmentIndexInCombobox = i;
                }
            }
            EquipmentsComboBox.SelectedIndex = equipmentIndexInCombobox;
        }

        private void ApplyViewsDataToProtocol()
        {
            FullProtocol_.Source = SourceTextBox.Text;
            FullProtocol_.Doctor = Doctors[DoctorsComboBox.SelectedIndex].getId();
            FullProtocol_.Equipment = Equipments[EquipmentsComboBox.SelectedIndex].getId();
            FullProtocol_.Source = SourceTextBox.Text;
        }

        private void OutToLogger()
        {
            logger.Debug(TAG, "Source: " + FullProtocol_.Source);
            logger.Debug(TAG, "Doctor id: " + FullProtocol_.Doctor);
            logger.Debug(TAG, "Patient id: " + FullProtocol_.Patient);
            logger.Debug(TAG, "Equipment id" + FullProtocol_.Equipment);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ApplyViewsDataToProtocol();
            OutToLogger();
            onSaveButtonClick(FullProtocol_);
        }

    }
}
