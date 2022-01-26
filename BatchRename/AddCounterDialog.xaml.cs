using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        AddCounterValidationRule addCounterRule = new AddCounterValidationRule();
        public AddCounterDialog()
        {
            InitializeComponent();
            this.DataContext = addCounterRule;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
           
            if (!addCounterRule.Validate(txtStart.Text, new CultureInfo("en-US")).IsValid
                || !addCounterRule.Validate(txtStep.Text, new CultureInfo("en-US")).IsValid)
            {
                MessageBox.Show("Vui lòng điền chính xác thông tin cho thứ tự bắt đầu và bước nhảy");
                return;
            }
            if (NewRuleReceived != null)
            {
                NewRuleReceived("addcounter", int.Parse(txtStart.Text), int.Parse(txtStep.Text));
            }
            this.DialogResult = true;
            this.Close();
        }
    }
    public partial class AddCounterValidationRule : ValidationRule, INotifyPropertyChanged
    {

        public string Start { get; set; }
        public string Step { get; set; }
        public string ErrorContent { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public AddCounterValidationRule() {
            this.Start = "";
            this.Step = "";
            this.ErrorContent = "";
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            string buffer = value.ToString();
            ValidationResult result = ValidationResult.ValidResult;

            if (buffer != null && buffer.Length > 0)
            {
                string pattern = @"^[0-9]+$";
                var regex = new Regex(pattern);
                if (regex.IsMatch(buffer))
                {
    
                }
                else { 
                    result = new ValidationResult(false, "Phần này chỉ bao gồm chữ số!");
                }
            }
            else
            {
                result = new ValidationResult(false, "Phần này không được để trống!");
            }
            return result;
        }

    }
}
