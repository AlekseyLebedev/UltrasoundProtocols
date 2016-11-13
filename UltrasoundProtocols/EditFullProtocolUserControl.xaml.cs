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
                LoadFields();
            }
        }

        public EditFullProtocolUserControl()
        {
            InitializeComponent();
        }

        private void LoadFields()
        {

        }

        private void ApplyFullProtocolFields()
        {

        }
    }
}
