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
}
