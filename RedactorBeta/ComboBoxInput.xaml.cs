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
            loginPassword.Item1 = "Логин";
            loginPassword.Item2 = "Пароль";
            if (tuple!=null)
            {
                if (tuple.Item1 != null)
                {
                    if (tuple.Item2 != null)
                    {
                        loginPassword = tuple;
                    }
                }
            }
            DataContext = loginPassword;
            InitializeComponent();

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
