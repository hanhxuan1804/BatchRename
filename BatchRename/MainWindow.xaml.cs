﻿using System;
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

using Microsoft.Win32;
using Fluent;
using System.IO;
using IRenameRules;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

//TODO: save list rule

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   
    public partial class MainWindow : RibbonWindow
    {
        BindingList<CStorageFile> list = new BindingList<CStorageFile>();//list file load by user
        BindingList<IRenameRule> rules = new BindingList<IRenameRule>();//list rule choice by user
        BindingList<CListRules> combo = new BindingList<CListRules>();//list combo rules save
        public MainWindow()
        {
            InitializeComponent();
        }

        List<IRenameRule> _prototypes; // Prototype

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {

            listFiles.ItemsSource = list;
            listChoice.ItemsSource = rules;
            _prototypes = new List<IRenameRule>();

            // Nạp danh sách các tập tin dll
            string exePath = Assembly.GetExecutingAssembly().Location;
            string folder = Path.GetDirectoryName(exePath) + "\\DLL";
            FileInfo[] fis = new DirectoryInfo(folder).GetFiles("*.dll");

            foreach (var f in fis) // Lần lượt duyệt qua các file dll
            {


                Assembly assembly = Assembly.LoadFile(f.FullName);
                var types = assembly.GetTypes();

                foreach (var t in types)
                {
                    if (t.IsClass && typeof(IRenameRule).IsAssignableFrom(t))
                    {
                        IRenameRule c = (IRenameRule)Activator.CreateInstance(t);
                        _prototypes.Add(c);
                    }
                }
            }
            loadToolShower();
            loadComboRule();
        }

        
        public static int[] NewCaseShower = { 1, 1, 1 };
        public static int[] NormalShower = { 1, 1, 1 };
        /*Hàm này kiểm tra xem các rule có từ dll đã load được chưa
        nếu chưa thì ẩn rule trên giao diện*/
        private void loadToolShower()
        {
            if (!_prototypes.Any(item => item.Key == "replaceextension"))
            {
                btnReplace.Visibility = Visibility.Hidden;
                btnReplace.Visibility = Visibility.Collapsed;
            }
            if (!_prototypes.Any(item => item.Key == "addcounter"))
            {
                btnAddCounter.Visibility = Visibility.Hidden;
                btnAddCounter.Visibility = Visibility.Collapsed;
            }
            if (!_prototypes.Any(item => item.Key == "tolower")
                && !_prototypes.Any(item => item.Key == "removespace")
                && !_prototypes.Any(item => item.Key == "parcalcase"))
            {
                btnNewCase.Visibility = Visibility.Hidden;
                btnNewCase.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (!_prototypes.Any(item => item.Key == "tolower"))
                {
                    NewCaseShower[0] = 0;
                }
                if (!_prototypes.Any(item => item.Key == "removespace"))
                {
                    NewCaseShower[1] = 0;
                }
                if (!_prototypes.Any(item => item.Key == "parcalcase"))
                {
                    NewCaseShower[2] = 0;
                }
            }
            if (!_prototypes.Any(item => item.Key == "addprefix")
               && !_prototypes.Any(item => item.Key == "addsuffix")
               && !_prototypes.Any(item => item.Key == "replaceletter"))
            {
                btnNormalize.Visibility = Visibility.Hidden;
                btnNormalize.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (!_prototypes.Any(item => item.Key == "addprefix"))
                {
                    NormalShower[0] = 0;
                }
                if (!_prototypes.Any(item => item.Key == "addsuffix"))
                {
                    NormalShower[1] = 0;
                }
                if (!_prototypes.Any(item => item.Key == "replaceletter"))
                {
                    NormalShower[2] = 0;
                }
            }
        }
        /*Hàm này load các com bo rule đã lưu trước đó*/
        private void loadComboRule()
        {
            var writer = new StreamWriter("recentrules.txt", true);
            writer.Close();
            using (var reader = new StreamReader("recentrules.txt"))
            {
                while (!reader.EndOfStream)
                {
                    CListRules one = new CListRules();
                    one.Rules = new List<IRenameRule>();
                    string line = reader.ReadLine();
                    if (line == null) return;
                    if (line.Split(' ').Length < 2) return;
                    one.Name = line.Split(' ')[0];
                    int count = int.Parse(line.Split(' ')[1]);
                    for (int i = 0; i < count; i++)
                    {
                        string lineRule = reader.ReadLine();
                        
                        if (lineRule == null) return; 
                        string[] vs = lineRule.Split(" decript: ");
                        if (vs.Length < 2) return;
                        string[] words = vs[0].Split(' ');
                        IRenameRule rule = _prototypes.First(rules => rules.Key == words[0]);
                        List<string> ws = words.ToList();
                        ws.RemoveAt(0);
                        rule = rule.Clone(ws);
                        rule.Description = vs[1];
                        one.Rules.Add(rule);
                    }
                    combo.Add(one);
                }
                reader.Close();
            }
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
                    f.Type = "File";
                    if (!list.Any(item => item.Path == f.Path))
                    {
                        list.Add(f);
                    }
                }
            };
            if (rules.Count > 0 && index != list.Count)
            {
                MessageBoxResult choice = MessageBox.Show("Bạn có muốn áp dụng các quy tắt đã chọn trước đó?", "Thông báo", MessageBoxButton.YesNo);
                if (choice == MessageBoxResult.Yes)
                {

                    for (int i = index; i < list.Count; i++)
                    {
                        for (int j = 0; j < rules.Count; j++)
                        {
                            list[i].NewName = rules[j].Rename(list[i].NewName);
                            list[i].NewName = rules[j].Rename(list[i].NewName, i);
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
            if (list[index].Type == "Folder")
            {
                return;
            }
            list.RemoveAt(index);
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Type == "File")
                {
                    list.RemoveAt((int)i);
                    i--;
                }
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Type == "File")
                {

                    FileInfo file = new FileInfo(list[i].Path);
                    if (file.Exists && file != null)
                    {
                        file.MoveTo(Path.Combine(file.Directory.FullName, list[i].NewName));

                        list[i].Name = file.Name;
                        list[i].Path = file.FullName;
                    }
                }
                else
                {
                    DirectoryInfo directory = new DirectoryInfo(list[i].Path);
                    if (directory.Exists && directory != null)
                    {
                        directory.MoveTo(Path.Combine(directory.Parent.FullName, list[i].NewName));
                        list[i].Name = directory.Name;
                        list[i].Path = directory.FullName;
                    }
                }

            }
        }

        private void btnReplace_Click(object sender, RoutedEventArgs e)
        {
            ReplaceExtensionDialog screen = new ReplaceExtensionDialog();
            screen.NewRuleReceived += (method, extension) =>
            {
                //Check Method
                IRenameRule result = _prototypes.Find(item => item.Key == method);
                List<string> data = new List<string>();
                data.Add(extension);
                IRenameRule rule = result.Clone(data);

                rule.Description = "Đổi phần mở rộng thành: " + extension;
                rules.Add(rule);
            };
            if (screen.ShowDialog() == true)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Type == "Folder") continue;
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
                var result = _prototypes.Find(item => item.Key == method);
                List<string> data = new List<string>();
                data.Add(start.ToString());
                data.Add(step.ToString());
                IRenameRule rule = result.Clone(data);
                rule.Description = $"Thêm số thứ tự vào cuối (start: {start}, step: {step})";
                rules.Add(rule);
            };
            if (screen.ShowDialog() == true)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].putFolderExtension();
                    list[i].NewName = rules[rules.Count - 1].Rename(list[i].NewName, i);
                    list[i].removeFolderExtension();
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
                var result = _prototypes.Find(item => item.Key == method);
                switch (method)
                {
                    case "tolower":
                        rule = result.Clone(new List<string>());
                        rule.Description = "Tất cả chuyển sang chữ thường";
                        rules.Add(rule);
                        break;
                    case "removespace":
                        rule = result.Clone(new List<string>());
                        rule.Description = "Xóa tất cả khoảng trắng";
                        rules.Add(rule);
                        break;
                    case "parcalcase":
                        rule = result.Clone(new List<string>());
                        rule.Description = "Chuyển về dạng pascal case";
                        rules.Add(rule);
                        break;
                }
            };
            if (screen.ShowDialog() == true)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = count; j < rules.Count; j++)
                    {
                        list[i].putFolderExtension();
                        list[i].NewName = rules[j].Rename(list[i].NewName);
                        list[i].removeFolderExtension();
                    }

                }
            };
        }

        private void btnNormalize_Click(object sender, RoutedEventArgs e)
        {
            int count = rules.Count;
            NormalizeDialog screen = new NormalizeDialog();
            screen.NewRuleReceived += (method, data) =>
            {
                //Check Method
                IRenameRule rule;
                var datas = new List<string>();
                datas.Add(data);
                var result = _prototypes.Find(item => item.Key == method);
                switch (method)
                {
                    case "addprefix":
                        rule = result.Clone(datas);
                        rule.Description = $"Thêm tiền tố {data}";
                        rules.Add(rule);
                        break;
                    case "addsuffix":
                        rule = result.Clone(datas);
                        rule.Description = $"Thêm hậu tố {data}";
                        rules.Add(rule);
                        break;
                    case "replaceletter":
                        List<string> needles = new List<string>();
                        needles.Add("-");
                        needles.Add("_");
                        needles.Add(" ");
                        needles.Add(".");
                        rule = result.Clone(needles);
                        rule.Description = "Thay thế các ký tự '-', '_', ' ' bằng '.'";
                        rules.Add(rule);
                        break;
                }
            };
            if (screen.ShowDialog() == true)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = count; j < rules.Count; j++)
                    {
                        list[i].putFolderExtension();
                        list[i].NewName = rules[j].Rename(list[i].NewName);
                        list[i].removeFolderExtension();
                    }
                }
            };
        }

        //List choice edit
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //edit
            int id = listChoice.SelectedIndex;

            switch (rules[id].Key)
            {
                case "replaceextension":
                    ReplaceExtensionDialog screen = new ReplaceExtensionDialog();
                    screen.NewRuleReceived += (method, extension) =>
                    {
                        //Check Method
                        var result = _prototypes.Find(item => item.Key == method);
                        List<string> data = new List<string>();
                        data.Add(extension);
                        IRenameRule rule = result.Clone(data);

                        rule.Description = "Đổi phần mở rộng thành: " + extension;
                        rules[id] = rule;
                    };
                    if (screen.ShowDialog() == true)
                    {
                        reapplyAllRules();
                    };
                    break;
                case "addcounter":
                    AddCounterDialog screen2 = new AddCounterDialog();
                    screen2.NewRuleReceived += (method, start, step) =>
                    {
                        //Check Method
                        var result = _prototypes.Find(item => item.Key == method);
                        List<string> data = new List<string>();
                        data.Add(start.ToString());
                        data.Add(step.ToString());
                        IRenameRule rule = result.Clone(data);
                        rule.Description = $"Thêm số thứ tự vào cuối file (start: {start}, step: {step})";

                        rules[id] = rule;
                    };
                    if (screen2.ShowDialog() == true)
                    {
                        reapplyAllRules();
                    };
                    break;

                default:
                    MessageBox.Show("Luật này chỉ có thể xóa !", "Thông báo", MessageBoxButton.OK);
                    break;
            }

        }
        //list choice remove
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            rules.RemoveAt(listChoice.SelectedIndex);
            reapplyAllRules();
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
                int id = list[i].NewName.LastIndexOf('.');
                string ex = "";
                if (id != -1 && list[i].Type == "File")
                {
                    ex = list[i].NewName.Substring(id, list[i].NewName.Length - id);
                }

                list[i].NewName = $"{GenerateName(15)}{ex}";
            }
        }
        public void reapplyAllRules()
        {
            for (int i = 0; i < list.Count; i++)
            {
                foreach (var r in rules)
                {
                    list[i].NewName = r.Rename(list[i].Name);
                    list[i].NewName = r.Rename(list[i].NewName, i);
                }
                if (rules.Count == 0)
                {
                    list[i].NewName = list[i].Name;
                }
            }
        }

        private void btnAddFolder_Click(object sender, RoutedEventArgs e)
        {
            int index = list.Count;// biến giữ số file hiện tại trong danh sách
            FolderBrowserDialog openFolderDialog = new FolderBrowserDialog();     
            openFolderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult result = openFolderDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                CStorageFile f = new CStorageFile();

                f.Path = openFolderDialog.SelectedPath;
                f.Name = Path.GetFileName(f.Path);
                f.NewName = f.Name;
                f.Type = "Folder";
                if (!list.Any(item => item.Path == f.Path))
                {
                    list.Add(f);
                }
            }

            if (rules.Count > 0 && index != list.Count)
            {
                MessageBoxResult choice = System.Windows.MessageBox.Show("Bạn có muốn áp dụng các quy tắt đã chọn trước đó?", "Thông báo", MessageBoxButton.YesNo);
                if (choice == MessageBoxResult.Yes)
                {

                    for (int i = index; i < list.Count; i++)
                    {
                        list[i].putFolderExtension();
                        for (int j = 0; j < rules.Count; j++)
                        {

                            list[i].NewName = rules[j].Rename(list[i].NewName);
                            list[i].NewName = rules[j].Rename(list[i].NewName, i);
                        }
                        list[i].removeFolderExtension();
                    }
                }
            }
        }
        private void btnRemoveFolder_Click(object sender, RoutedEventArgs e)
        {
            int index = listFiles.SelectedIndex;
            if (index < 0 || index > list.Count)
            {
                return;
            }
            if (list[index].Type == "File")
            {
                return;
            }
            list.RemoveAt(index);
        }
        private void btnClearAllFolder_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Type == "Folder")
                {
                    list.RemoveAt((int)i);
                    i--;
                }
            }
        }

        private void btnSaveRules_Click(object sender, RoutedEventArgs e)
        {
            if (rules.Count == 0) return;
            CListRules one = new CListRules();
            one.Name = "";//TODO: dialog to get name
            one.Rules = rules.ToList();
            combo.Add(one);
        }

        private void RibbonWindow_Closing(object sender, CancelEventArgs e)
        {
            //write combo vào file để sử dụng lần sau
            using (var writer = new StreamWriter("recentrules.txt"))
            {
                foreach (var co in combo)
                {
                    writer.WriteLine($"{co.Name} {co.Rules.Count}");
                    foreach (var rule in co.Rules)
                    {
                        writer.WriteLine(rule.toString());
                    }
                }
            }
        }
    }
}
