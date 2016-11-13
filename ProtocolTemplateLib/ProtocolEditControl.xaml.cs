﻿using System;
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

namespace ProtocolTemplateLib
{
    /// <summary>
    /// Логика взаимодействия для ProtocolEditControl.xaml
    /// </summary>
    public partial class ProtocolEditControl : UserControl
    {
        public ProtocolEditControl()
        {
            InitializeComponent();
        }

        public void SetContent(UIElement element)
        {
            Viewer.Content = element;
        }
    }
}
