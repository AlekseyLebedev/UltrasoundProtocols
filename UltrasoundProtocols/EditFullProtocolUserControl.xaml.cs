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
    /// Interaction logic for EditFullProtocolUserControl.xaml
    /// </summary>
    public partial class EditFullProtocolUserControl : UserControl
    {
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

        private List<MedicalEquipment> equipments;
        private List<Doctor> doctors;

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
            //task.Logger = logger;
            task.InfoMessage = "EditFullProtocolUserControl";
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

        }

        //Применяет данные из бд к views
        private void applyFieldsToViews()
        {

        }

    }
}
