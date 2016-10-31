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

namespace ProtocolTemplateLib
{
    /// <summary>
    /// Логика взаимодействия для LineControl.xaml
    /// </summary>
    public partial class LineControl : UserControl
    {
        public LineControl()
        {
            InitializeComponent();
        }

        public TemplateLine Line
        {
            get
            {
                return Line_;
            }
            set
            {
                ControlGrid.Children.Clear();
                ControlGrid.Children.Add(value.Field.GetEditControl());
                LineLabel.Text = value.Label;
                Line_ = value;
            }
        }

        private TemplateLine Line_;
    }
}
