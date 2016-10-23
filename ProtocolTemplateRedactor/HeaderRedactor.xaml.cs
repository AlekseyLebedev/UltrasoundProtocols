using ProtocolTemplateLib;
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

namespace ProtocolTemplateRedactor
{
    /// <summary>
    /// Логика взаимодействия для HeaderRedactor.xaml
    /// </summary>
    internal partial class HeaderRedactor : UserControl
    {
        internal HeaderRedactor()
        {
            InitializeComponent();
        }

        internal EditTemplatePresenter Presenter
        {
            get { return Presenter_; }
            set
            {
                Presenter_ = null;
                headerLabel.Text = value.SelectedHeaderText;
                Presenter_ = value;
            }
        }

        private void headerLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Presenter_ != null)
            {
                Presenter_.SelectedHeaderText = headerLabel.Text;
            }
        }

        private EditTemplatePresenter Presenter_;
    }
}
