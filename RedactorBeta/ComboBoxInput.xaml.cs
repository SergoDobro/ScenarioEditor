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
    /// Логика взаимодействия для ComboBoxInput.xaml
    /// </summary>
    public partial class ComboBoxInput : UserControl
    {
        public Action<ComboBoxInput> OnDelete;
        public Action<object, RoutedEventArgs> OnEdit;
        public MyTuple val = new MyTuple();
        public MyTuple loginPassword
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
                if (OnEdit != null)
                    OnEdit(null, null);
            }
        }

        public ComboBoxInput(MyTuple tuple = null)
        {
            loginPassword.Log = "Логин";
            loginPassword.Pas = "Пароль";
            if (tuple!=null)
            {
                if (tuple.Log != null)
                {
                    if (tuple.Pas != null)
                    {
                        loginPassword = tuple;
                    }
                }
            }
            DataContext = loginPassword;
            InitializeComponent();

        }
        //only once
        public void BrootPairCheck()
        {
            loginPassword.Log = login.Text;
            loginPassword.Pas = password.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnDelete(this);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OnEdit != null)
                 OnEdit(null, null);
        }
    }
}
