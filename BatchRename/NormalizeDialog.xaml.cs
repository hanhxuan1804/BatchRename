using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for NormalizeDialog.xaml
    /// </summary>
    public partial class NormalizeDialog : Window
    {
        public NormalizeDialog()
        {
            InitializeComponent();
        }

        private void cbAddPrefix_Checked(object sender, RoutedEventArgs e)
        {
            txtPrefix.Visibility = Visibility.Visible;
        }

        private void cbAddSuffix_Checked(object sender, RoutedEventArgs e)
        {
            txtSuffix.Visibility = Visibility.Visible;
        }

        public delegate void AddRule(string methodName, string data);
        public event AddRule NewRuleReceived = null;
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {

            if (NewRuleReceived != null)
            {
                if (cbAddPrefix.IsChecked == true)
                {
                    NewRuleReceived("addprefix", txtPrefix.Text);
                }
                if (cbAddSuffix.IsChecked == true)
                {
                    NewRuleReceived("addsuffix", txtSuffix.Text);
                }
                if (cbReplacer.IsChecked == true)
                {
                    NewRuleReceived("replaceletter", "");
                }
            }
            this.DialogResult = true;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (MainWindow.NormalShower[0] == 0)
            {
                cbAddPrefix.Visibility = Visibility.Collapsed;

            }
            if (MainWindow.NormalShower[1] == 0)
            {

                cbAddSuffix.Visibility = Visibility.Collapsed;

            }
            if (MainWindow.NormalShower[2] == 0)
            {

                cbReplacer.Visibility = Visibility.Collapsed;
            }
        }
    }
}
