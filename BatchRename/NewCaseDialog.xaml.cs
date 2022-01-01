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
    /// Interaction logic for NewCaseDialog.xaml
    /// </summary>
    public partial class NewCaseDialog : Window
    {
        
        
        public NewCaseDialog()
        {
            InitializeComponent();
         
        }

        public delegate void AddRule(string methodName);
        public event AddRule NewRuleReceived = null;
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
           
            if (NewRuleReceived != null)
            {
                if(cbLowerCase.IsChecked == true)
                {
                    NewRuleReceived("tolower");
                }
                if (cbRemoveSpace.IsChecked == true)
                {
                    NewRuleReceived("removespace");
                }
                if(cbPascalCase.IsChecked == true)
                {
                    NewRuleReceived("parcalcase");
                }
            }
            this.DialogResult = true;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (MainWindow.NewCaseShower[0] == 0)
            {
                cbLowerCase.Visibility = Visibility.Collapsed;
            
            }
            if (MainWindow.NewCaseShower[1] == 0)
            {
         
                cbRemoveSpace.Visibility = Visibility.Collapsed;
     
            }
            if (MainWindow.NewCaseShower[2] == 0)
            {
               
                cbPascalCase.Visibility = Visibility.Collapsed;
            }
        }
    }
}
