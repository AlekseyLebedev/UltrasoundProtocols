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
    /// Логика взаимодействия для LineRedactor.xaml
    /// </summary>
    internal partial class LineRedactor : UserControl
    {
        public LineRedactor()
        {
            InitializeComponent();
        }

        private void labelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Presenter_ != null)
            {
                Presenter_.SelectedLineLabel = labelTextBox.Text;
            }
        }
        internal EditTemplatePresenter Presenter
        {
            get { return Presenter_; }
            set
            {
                Presenter_ = null;
                labelTextBox.Text = value.SelectedLineLabel;
                Presenter_ = value;
            }
        }

        private EditTemplatePresenter Presenter_;
    }
}
