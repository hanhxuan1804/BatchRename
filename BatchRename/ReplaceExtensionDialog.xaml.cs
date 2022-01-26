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
    /// Interaction logic for ReplaceExtensionDialog.xaml
    /// </summary>
    public partial class ReplaceExtensionDialog : Window
    {
        public delegate void AddRule(string methodName, string extension);
        public event AddRule NewRuleReceived = null;


        ReplaceRuleValidation validation = new ReplaceRuleValidation();
        public ReplaceExtensionDialog()
        {
            InitializeComponent();
            this.DataContext = validation;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {

            if (!validation.Validate(NewString.Text, new CultureInfo("en-US")).IsValid)
            {
                MessageBox.Show("Phần mở rộng bạn chọn không đúng với yêu cầu." +
                    " Vui lòng chọn phần mở rộng khác!");
                return;
            }
            if (NewRuleReceived != null)
            {
                NewRuleReceived("replaceextension", validation.Extension);
            }
            this.DialogResult = true;
            this.Close();
        }
    }
    public partial class ReplaceRuleValidation : ValidationRule, INotifyPropertyChanged
    {
        public string Extension { get; set; }
        public string ErrorContent { get; set; }

        public ReplaceRuleValidation()
        {
            this.Extension = "";
            this.ErrorContent = "Phần mở rộng chỉ bao gồm các ký tự A-Z, a-z và 0-9";
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            
            Extension = value.ToString();
            ValidationResult result = ValidationResult.ValidResult;
            
            if (Extension != null && Extension.Length > 0)
            {
                string pattern = @"^[A-Za-z0-9]+$";
                var regex = new Regex(pattern);
                if (!regex.IsMatch(Extension))
                {
                    result = new ValidationResult(false, ErrorContent);
                }
            }
            else
            {
                result = new ValidationResult(false, "Phần mở rộng không thể bỏ trống");
            }
            return result;
        }
        
    }
    
}
