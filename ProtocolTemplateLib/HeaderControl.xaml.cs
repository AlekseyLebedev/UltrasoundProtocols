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
    /// Логика взаимодействия для HeaderControl.xaml
    /// </summary>
    public partial class HeaderControl : UserControl
    {
        public HeaderControl()
        {
            InitializeComponent();
        }

        public TemplateHeader Header
        {
            get
            {
                return Header_;
            }
            set
            {
                HeaderLabel.Text = value.Header;
                Header_ = value;
            }
        }

        private TemplateHeader Header_;
    }
}
