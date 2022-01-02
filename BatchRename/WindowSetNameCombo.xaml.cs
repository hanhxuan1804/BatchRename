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
    /// Interaction logic for WindowSetNameCombo.xaml
    /// </summary>
    public partial class WindowSetNameCombo : Window
    {
        public WindowSetNameCombo()
        {
            InitializeComponent();
        }
        public delegate void AddRule(string Name);
        public event AddRule NewRuleReceived = null;
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {

            if (NewRuleReceived != null)
            {
                    NewRuleReceived(txtName.Text);
            }
            this.DialogResult = true;
            this.Close();
        }
    }
}
