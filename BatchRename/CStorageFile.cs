using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BatchRename
{
    public class CStorageFile : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string NewName { get; set; }
        public string Path { get; set; }
        public string Error { get; set; }

        public string Type { get; set; }//1: file, 2: folder
        public CStorageFile(string name, string newname, string path, string err)
        {
            this.Name = name;
            this.NewName = newname;
            this.Path = path;
            this.Error = err;
        }

        public CStorageFile()
        {
        }

        public void putFolderExtension()
        {
            if (this.Type == "Folder")
            {
                this.NewName = this.NewName + ".folder";
            }     
        }
        public void removeFolderExtension()
        {
            if (this.Type == "Folder")
            {
                int id = this.NewName.LastIndexOf(".folder");
                if (id != -1)
                {
                    this.NewName = this.NewName.Substring(0, id);
                }
            }
        }

        public override string ToString()
        {
            return this.Name + " " + this.NewName + " " + this.Path + " " + this.Error;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    //Đây là 2 lớp dùng để thay đổi chiều cao và chiều rộng của mỗi cột trong listview
    //Tạm thời để ở đây
    [ValueConversion(typeof(double), typeof(double))]
    public class WidthFourConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((double)value) * 0.25;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((double)value) / 0.25;
        }

        #endregion
    }

    [ValueConversion(typeof(double), typeof(double))]
    public class HeightConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((double)value - 150.0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((double)value) + 150.0;
        }

        #endregion
    }
}
