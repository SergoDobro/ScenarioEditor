using System;
using System.Collections.Generic;
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

namespace RedactorBeta
{
    /// <summary>
    /// Логика взаимодействия для LoginsAndPasswords.xaml
    /// </summary>
    public partial class LoginsAndPasswords : UserControl
    {
        public Action OnSave;
        public LoginsAndPasswords()
        {
            InitializeComponent();
        }
        public LoginsAndPasswords(DataClass dataClass)
        {

            foreach (var item in dataClass.LoginsAndPasswords)
            {
                loginsAndPasswords.Children.Insert(0, new ComboBoxInput(new MyTuple() { Log = item.Key, Pas = item.Value }));
                (loginsAndPasswords.Children[0] as ComboBoxInput).OnDelete += Delete;
                (loginsAndPasswords.Children[0] as ComboBoxInput).OnEdit += Save_LogPasPair;
            }
        }
        public void SaveToDataClass(DataClass dataClass)
        {

            for (int i = 0; i < loginsAndPasswords.Children.Count - 1; i++)
            {
                if (!dataClass.LoginsAndPasswords.ContainsKey((loginsAndPasswords.Children[i] as ComboBoxInput).loginPassword.Log))
                {
                    dataClass.LoginsAndPasswords.Add((loginsAndPasswords.Children[i] as ComboBoxInput).loginPassword.Log, (loginsAndPasswords.Children[i] as ComboBoxInput).loginPassword.Pas);
                }
            }
        }
        private void Add_New_Passwords_Pair(object sender, RoutedEventArgs e)
        {

            ComboBoxInput cbi = new ComboBoxInput();
            cbi.login.Text += loginsAndPasswords.Children.Count;
            loginsAndPasswords.Children.Insert(loginsAndPasswords.Children.Count-1, cbi);
            cbi.OnDelete += Delete;
            cbi.OnEdit += Save_LogPasPair;
            Save_LogPasPair(null, null);
            cbi.BrootPairCheck(); //only once
        }
        public void Delete(ComboBoxInput cbi)
        {
            loginsAndPasswords.Children.Remove(cbi);
            Save_LogPasPair(null, null);
        }

        public void Load(DataClass dataClass)
        {
            foreach (var item in dataClass.LoginsAndPasswords)
            {
                loginsAndPasswords.Children.Insert(0, new ComboBoxInput(new MyTuple() { Log = item.Key, Pas = item.Value }));
                (loginsAndPasswords.Children[0] as ComboBoxInput).OnDelete += Delete;
                (loginsAndPasswords.Children[0] as ComboBoxInput).OnEdit += Save_LogPasPair;
            }
        }
        public void Save_LogPasPair(object obj, RoutedEventArgs ev)
        {
            for (int i = 0; i < loginsAndPasswords.Children.Count-1; i++)
            {
                bool isOk = true;
                for (int j = 0; j < loginsAndPasswords.Children.Count - 1; j++)
                {
                    if (i!=j)
                    {
                        if ((loginsAndPasswords.Children[i] as ComboBoxInput).login.Text == (loginsAndPasswords.Children[j] as ComboBoxInput).login.Text)
                        {
                            (loginsAndPasswords.Children[j] as ComboBoxInput).login.BorderBrush = Brushes.Red;
                            isOk = false;
                        }

                    }
                }
                if (isOk)
                {
                    (loginsAndPasswords.Children[i] as ComboBoxInput).login.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
                }
                else
                {
                    (loginsAndPasswords.Children[i] as ComboBoxInput).login.BorderBrush = Brushes.Red;
                }
            }
            //OnSave();
        }
    }
}
