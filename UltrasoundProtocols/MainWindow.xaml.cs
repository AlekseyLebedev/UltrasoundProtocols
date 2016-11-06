﻿using System;
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
            Surname.DisplayMemberBinding = new Binding("LastName");
            Name.DisplayMemberBinding = new Binding("FirstName");
            MiddleName.DisplayMemberBinding = new Binding("MiddleName");
            id.DisplayMemberBinding = new Binding("NumberAmbulatoryCard");
            Gender.DisplayMemberBinding = new Binding("Gender");
            Birthday.DisplayMemberBinding = new Binding("Date");
            presenter = new EditPatientPresenter();
	 	}

        EditPatientPresenter presenter;
        private void listView_Loaded(object sender, RoutedEventArgs e)
        {
            List<Patient> patientList = presenter.LoadPatientListFromDataBase();
            foreach(Patient patient in patientList)
            {
                listView.Items.Add(patient);
            }
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PatientColumn.Width = new GridLength(9, GridUnitType.Star);
        }
    }
}
