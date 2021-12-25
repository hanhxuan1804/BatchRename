using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class CStorageFile : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string NewName { get; set; }
        public string Path { get; set; }
        public string Error { get; set; }
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

        public override string ToString()
        {
            return this.Name + " " + this.NewName + " " + this.Path + " " + this.Error;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
