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
    /// Логика взаимодействия для ComboBoxRedactor.xaml
    /// </summary>
    internal partial class ComboBoxRedactor : UserControl
    {
        public ComboBoxRedactor()
        {
            InitializeComponent();
        }
        internal EditTemplatePresenter Presenter
        {
            get { return Presenter_; }
            set
            {
                Presenter_ = null;
                VariantsTextBox.Text = value.SelectedComboBoxVariants;
                OtherEnabled.IsChecked = value.SelectedEditableEnabled;
                Presenter_ = value;
                line.Presenter = value;
            }
        }

        private EditTemplatePresenter Presenter_;

        private void VariantsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Presenter_ != null)
            {
                Presenter_.SelectedComboBoxVariants = VariantsTextBox.Text;
            }
        }

        private void OtherEnabled_Checked(object sender, RoutedEventArgs e)
        {
            if (Presenter_ != null)
            {
                Presenter_.SelectedEditableEnabled = OtherEnabled.IsChecked.Value;
            }
        }
    }
}
