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
    /// Interaction logic for AddFødevare.xaml
    /// </summary>
    public partial class AddIndtag : Window
    {


        public AddIndtag(List<string> madvarer)
        {
            InitializeComponent();

            Combo.ItemsSource = madvarer;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
