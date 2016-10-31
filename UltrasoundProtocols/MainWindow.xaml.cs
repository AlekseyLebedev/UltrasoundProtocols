using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
			UltrasoundProtocolsDataSetSelector selector = new UltrasoundProtocolsDataSetSelector();
			List<Doctor> doctors = selector.getActiveDoctors();
			var hzchtoetopokachto = selector.getFullFilledProtocols();
			//Debugger.Break();
		}
    }
}
