﻿using System;
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

namespace normaltree
{
    /// <summary>
    /// Логика взаимодействия для SelectItem.xaml
    /// </summary>
    public partial class SelectItem : UserControl
    {
        public string ctn { get => contentName.Content.ToString(); set => contentName.Content = value; }
        public SelectItem()
        {
            InitializeComponent();
        }
    }
}