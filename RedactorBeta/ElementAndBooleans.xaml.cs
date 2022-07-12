using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Linq;

namespace RedactorBeta
{
    
    public partial class ElementAndBooleans : UserControl
    {
        const string extentionString = ".config";
        public event Action<object> OnOpen;
        public static string pathToMain = Directory.GetCurrentDirectory();
        public string path;
        private string name;
        private string format;
        DataClass dataClass;
        public ElementAndBooleans()
        {
            Init(pathToMain);
        }
        public ElementAndBooleans(string path)
        {
            Init(path);
        }
        public void Init(string path)
        {

            InitializeComponent();
            Margin = new Thickness(20, 0, 0, 0);

            this.path = path;
            //return;
            LoadSettings();
            name = path.Split('\\').Last();
            DetermineFormat();
            SetContentName();

            CollapseButton.Content = "▶";
            if (Directory.Exists(this.path))
            {
                CollapseButton.Visibility = Visibility.Visible;
            }
            else
            {
                CollapseButton.Visibility = Visibility.Hidden;
            }

        }
        public void DetermineFormat()
        {
            if (name.Contains('.'))
            {
                format = name.Split('.').Last();
            }
            else
            {
                format = "";
            }
        }
        public void SetContentName()
        {
            string pic;
            switch (format)
            {
                case "":
                    pic = "📁"; // 🗀
                    break;
                case "doc":
                case "docx":
                case "cs":
                case "txt":
                    pic = "📄";
                    break;
                case "jpg":
                case "png":
                    pic = "🖼";
                    break;
                case "mp3":
                    pic = "🎵";
                    break;
                case "exe":
                    pic = "💽";
                    break;
                case "wav":
                case "mp4":
                    pic = "🎬"; // 🎞
                    break;
                case "xaml":
                case "xml":
                    pic = "📱";
                    break;
                case "config":
                    pic = "🔗";
                    break;
                default:
                    pic = "🗋 ";
                    break;
            }
            contentName.Content = pic+name.Replace("_", "__");
        }
        public void Load()
        {
            nextElements.Clear();
            if (Directory.Exists(path))
            {
                nextElements.AddRange(Directory.GetDirectories(path));
                nextElements.AddRange(Directory.GetFiles(path));
            }
            else
            {
            }
        }
        public void Save(object sender, RoutedEventArgs e)
        {
            dataClass.loginsAndPasswords = new List<MyTuple>();
            for (int i = 0; i < loginsAndPasswords.Items.Count-1; i++)
            {
                dataClass.loginsAndPasswords.Add((loginsAndPasswords.Items[i] as ComboBoxInput).loginPassword);
            }
            string str = JsonConvert.SerializeObject(dataClass);
            File.WriteAllText(path + extentionString, str);
        }
        public void LoadSettings()
        {
            dataClass = new DataClass();
            if (File.Exists(path + extentionString))
            {
                //load
                try
                {
                    dataClass = JsonConvert.DeserializeObject<DataClass>(File.ReadAllText(path + extentionString));
                    if (dataClass == null)
                    {
                        dataClass = new DataClass();
                        string str = JsonConvert.SerializeObject(dataClass);
                        File.WriteAllText(path + extentionString, str);
                    }
                }
                catch
                {
                    dataClass = new DataClass();
                    File.WriteAllText(path + extentionString, JsonConvert.SerializeObject(dataClass));
                }

            }
            else
            {
                dataClass = new DataClass();
                File.WriteAllText(path + extentionString, JsonConvert.SerializeObject(dataClass));
            }
            DataContext = dataClass;

            foreach (var item in dataClass.loginsAndPasswords)
            {
                loginsAndPasswords.Items.Insert(0, new ComboBoxInput(item));
                (loginsAndPasswords.Items[0] as ComboBoxInput).OnDelete += Delete;
                (loginsAndPasswords.Items[0] as ComboBoxInput).OnEdit += Save;
            }
        }

        bool isOpened = false;
        public List<string> nextElements = new List<string>() { "FolderA", "FolderB" };
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isOpened)
            {
                isOpened = true;
                CollapseButton.Content = "▼";
                Load();
                foreach (var item in nextElements)
                {
                    if (item.EndsWith(extentionString))
                        continue;
                    var element = new ElementAndBooleans(item);
                    panel.Children.Add(element);
                }

                if (OnOpen != null)
                    OnOpen(this);
            }
            else
            {
                CollapseButton.Content = "▶";
                isOpened = false;
                CloseSave();
                panel.Children.Clear();
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ComboBoxInput cbi = new ComboBoxInput();
            loginsAndPasswords.Items.Insert(0, cbi);
            cbi.OnDelete += Delete;
            cbi.OnEdit += Save;
        }
        public void Delete(ComboBoxInput cbi)
        {
            loginsAndPasswords.Items.Remove(cbi);
            Save(null, null);
        }

        public void CloseSave()
        {
            Save(null, null);
            foreach (var item in panel.Children)
            {
                if (item is ElementAndBooleans)
                    (item as ElementAndBooleans).CloseSave();
            }
        }

    }
    public static class Extention
    {
        public static string LevelUp_Debug(this string path)
        {
            return path.Remove(path.Length - 1 - path.Split('\\').Last().Length);
        }
    }
    public class DataClass
    {
        public bool isVisible { get; set; } = false;
        public bool hasPassword { get; set; } = false;
        public List<MyTuple> loginsAndPasswords { get; set; } = new List<MyTuple>();
    }
    public class MyTuple
    {
        public string Item1 { get; set; } = "";
        public string Item2 { get; set; } = "";
    }
}
