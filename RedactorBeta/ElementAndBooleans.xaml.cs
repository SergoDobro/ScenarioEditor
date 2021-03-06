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

            loginsAndPasswords.OnSave += () => { Save(null, null); };

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
            dataClass.LoginsAndPasswords = new Dictionary<string, string>();
            loginsAndPasswords.SaveToDataClass(dataClass);
            string str = JsonConvert.SerializeObject(dataClass);
            File.WriteAllText(path + extentionString, str);

            MainWindow.getInstance.Saved();
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
            loginsAndPasswords.Load(dataClass);
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
}
