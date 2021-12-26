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
    /// Interaction logic for AddCounterDialog.xaml
    /// </summary>
    public partial class AddCounterDialog : Window
    {
        
        public delegate void AddRule(string methodName, int start, int step);
        public event AddRule NewRuleReceived = null;
        public AddCounterDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            //foreach (char item in "\\/:*?\"<>|")
            //{
            //    if (txtStart.Text.Contains(item))
            //    {
            //        MessageBox.Show("Từ thay thế có chứa kí tự không hợp lệ");
            //        this.DialogResult = true;
            //        this.Close();
            //        return;
            //    }
            //} TODO: valication
            if (txtStart.Text == "" || txtStep.Text == "")
            {
                MessageBox.Show("Thông tin chưa được điền");
                this.DialogResult = false;
                return;
            }
            if (NewRuleReceived != null)
            {
                NewRuleReceived("ReplaceExtension", int.Parse(txtStart.Text), int.Parse(txtStep.Text));
            }
            this.DialogResult = true;
            this.Close();
        }
    }
}
