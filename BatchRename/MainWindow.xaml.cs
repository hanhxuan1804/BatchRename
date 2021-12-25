using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Fluent;
using System.IO;
namespace System.IO
{
    public static class FileInfoExtensions
    {
        public static void Rename(this FileInfo fileInfo, string newName)
        {
            fileInfo.MoveTo(Path.Combine(fileInfo.Directory.FullName, newName));
        }
    }
}

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        BindingList<CStorageFile> list = new BindingList<CStorageFile>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            listFiles.ItemsSource = list;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    CStorageFile f = new CStorageFile();
                    f.Name = System.IO.Path.GetFileName(filename);
                    f.NewName = f.Name;
                    f.Path = filename;
                    f.Error = "None";
                    list.Add(f);
                }
            };
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            int index = listFiles.SelectedIndex;
            if (index < 0 || index > list.Count)
            {
                return;
            }
            list.RemoveAt(index);
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            list.Clear();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < list.Count; i++)
            {
                FileInfo file = new FileInfo(list[i].Path);
                if (file.Exists)
                {
                    file.Rename(list[i].NewName);
                    list[i].Name = list[i].NewName;
                }
            }   
        }
        
    }
}
