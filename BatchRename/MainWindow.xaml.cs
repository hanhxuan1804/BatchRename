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
        BindingList<CStorageFile> list = new BindingList<CStorageFile>();//list file load by user
        BindingList<IRenameRule> rules = new BindingList<IRenameRule>();//list rule choice by user
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO load DLL file
            listFiles.ItemsSource = list;
            listChoice.ItemsSource = rules;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            int index = list.Count;// biến giữ số file hiện tại trong danh sách
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
            if(rules.Count > 0)
            {
                MessageBoxResult choice= MessageBox.Show("Bạn có muốn áp dụng các quy tắt đã chọn trước đó?", "Thông báo", MessageBoxButton.YesNo);
                if(choice == MessageBoxResult.Yes)
                {
                    
                    for(int i = index;i< list.Count; i++)
                    {
                        for (int j = 0; j < rules.Count; j++)
                        {
                            list[i].NewName = rules[j].Rename(list[i].NewName);
                            list[i].NewName = rules[j].Rename(list[i].NewName,i);
                        }
                    }
                }
            }
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

        public static string GenerateName(int len)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)];
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;
        }
        private void btnRandomName_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].NewName = GenerateName(20);
            }
        }

        private void btnReplace_Click(object sender, RoutedEventArgs e)
        {
            ReplaceExtensionDialog screen = new ReplaceExtensionDialog();
            screen.NewRuleReceived += (method, extension) =>
            {
                //Check Method
                CReplceExtensionRule rule = new CReplceExtensionRule(extension);
                rule.Description= "Đổi phần mở rộng thành: " + extension;
                rules.Add(rule);
            };
            if (screen.ShowDialog() == true)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].NewName = rules[rules.Count - 1].Rename(list[i].NewName);
                }
            };
        }

        private void btnAddCounter_Click(object sender, RoutedEventArgs e)
        {
            AddCounterDialog screen = new AddCounterDialog();
            screen.NewRuleReceived += (method, start, step) =>
            {
                //Check Method
                CAddCounterRule rule = new CAddCounterRule(start,step);
                rule.Description = $"Thêm số thứ tự vào cuối file (start: {start}, step: {step})";
                rules.Add(rule);
            };
            if (screen.ShowDialog() == true)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].NewName = rules[rules.Count-1].Rename(list[i].NewName,i);
                }
            };
        }

        private void btnNewCase_Click(object sender, RoutedEventArgs e)
        {
            int count = rules.Count;
            NewCaseDialog screen = new NewCaseDialog();
            screen.NewRuleReceived += (method) =>
            {
                //Check Method
                IRenameRule rule;
                switch (method)
                {
                    case "lowercase":
                        rule = new CToLowerRule();
                        rule.Description = "Tất cả chuyển sang chữ thường";
                        rules.Add(rule);
                        break;
                    case "removespace":
                        rule = new CRemoveSpaceRule();
                        rule.Description = "Xóa tất cả khoảng trắng";
                        rules.Add(rule);
                        break;
                    case "pascalcase":
                        rule = new CParcalCaseRule();
                        rule.Description = "Chuyển về dạng pascal case";
                        rules.Add(rule);
                        break;
                }                
            };
            if (screen.ShowDialog() == true)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    for(int j = count; j < rules.Count; j++)
                    {
                        list[i].NewName = rules[j].Rename(list[i].NewName);
                    }
                    
                }
            };
        }


    }
}
