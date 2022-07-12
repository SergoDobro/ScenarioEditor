using Microsoft.Win32;
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
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closed += (a, b) => {
                if (folderer.Children.Count > 0)
                    (folderer.Children[0] as ElementAndBooleans).CloseSave();
            };
        }

        private void ElementAndBooleans_OnOpen(object sender)
        {
            //(sender as ElementAndBooleans).Open();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            //if (openFileDialog.ShowDialog() == true)
            //    mainfolder = new ElementAndBooleans(openFileDialog.FileName);
            
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {

                folderBrowserDialog.ShowNewFolderButton = true;
                folderBrowserDialog.SelectedPath = Directory.GetCurrentDirectory();
                if (folderer.Children.Count > 0)
                    folderBrowserDialog.SelectedPath = (folderer.Children[0] as ElementAndBooleans).path;
                folderBrowserDialog.Description = "Выберите путь до редактируемого сценария";
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (folderBrowserDialog.SelectedPath == Directory.GetCurrentDirectory())
                    {
                        MessageBox.Show("Нельзя открыть папку, конфиги самого приложения могут быть повреждены.");
                    }
                    else
                    {
                        if (folderer.Children.Count > 0)
                            (folderer.Children[0] as ElementAndBooleans).CloseSave();
                        folderer.Children.Clear();
                        folderer.Children.Add(new ElementAndBooleans(folderBrowserDialog.SelectedPath));
                    }
                }
                
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (folderer.Children.Count > 0)
                (folderer.Children[0] as ElementAndBooleans).CloseSave();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Первая галочка - виден ли файл пользователю" +
                "\nВторая - есть ли пароль" +
                "\nЕсли да, то будет открыт доступ равее к добавлению паролей, наведитесь и нажмите на плюс, у вас добавится окно ввода пароля" +
                "\n" +
                "\nВ приложении присутствует автосохранение по изменению и закрытию сценария." +
                "\nПри открытии подпапок для всех их файлов автоматически создаются конфиг-файлы." +
                "\nНе открывайте папки не относящиеся к сценарию, могут повредиться файлы сторонние .config (поэтому лучше не открывать в приложении папку с .exe приложением)");
        }
    }
}
