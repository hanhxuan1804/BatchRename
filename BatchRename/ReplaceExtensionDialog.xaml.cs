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
    /// Interaction logic for ReplaceExtensionDialog.xaml
    /// </summary>
    public partial class ReplaceExtensionDialog : Window
    {
        public delegate void AddRule(string methodName, string extension);
        public event AddRule NewRuleReceived = null;
        public ReplaceExtensionDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (char item in "\\/:*?\"<>|")
            {
                if (NewString.Text.Contains(item))
                {
                    MessageBox.Show("Từ thay thế có chứa kí tự không hợp lệ");
                    this.DialogResult = true;
                    this.Close();
                    return;
                }
            }
            if (NewString.Text == "")
            {
                MessageBox.Show("Từ thay thế chưa được điền");
                this.DialogResult = true;
                this.Close();
                return;
            }
            if (NewRuleReceived != null)
            {
                NewRuleReceived("replaceextension", NewString.Text);
            }
            this.DialogResult = true;
            this.Close();
        }
    }
}
