using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
using Newtonsoft.Json;

namespace ReEksamenVinter16_17
{
    /// <summary>
    /// Interaction logic for Indtag.xaml
    /// </summary>
    public partial class Indtag : Window
    {

        public Indtag()
        {
            InitializeComponent();

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();
        }

        private void AddButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
