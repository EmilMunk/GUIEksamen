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
using System.Windows.Shapes;

namespace ReEksamenVinter16_17
{
    /// <summary>
    /// Interaction logic for TabelWindow.xaml
    /// </summary>
    public partial class TabelWindow : Window
    {
        public TabelWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            Application.Current.MainWindow.Show();

            this.Close();
        }
    }
}
