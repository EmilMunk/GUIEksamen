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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace ReEksamenVinter16_17
{
    public class test
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static HttpClient _client = new HttpClient();

        private List<IndtagAfKalorier> _kalorieIndtags = new List<IndtagAfKalorier>();

        public MainWindow()
        {
            InitializeComponent();

            if (_client.BaseAddress == null)
            {
                _client.BaseAddress = new Uri("http://localhost:4200/");

                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            _kalorieIndtags = readDbData();

            var list = new List<test>();
            list.Add(new test() {Name = "Land1", Count = 1000});

            

        }

        #region GetDataFromDatabase
        static List<IndtagAfKalorier> readDbData()
        {
            var result = Task.Run(() => GetData()).Result;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) =>
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        System.Diagnostics.Debugger.Break();
                    }
                }
            };

            var i = JsonConvert.DeserializeObject<List<IndtagAfKalorier>>(result, settings);

            return i;

        }

        static async Task<string> GetData()
        {
            string output = null;

            var response = await _client.GetAsync($"api/KalorieIndtagAPI");
            if (response.IsSuccessStatusCode)
            {
                output = await response.Content.ReadAsStringAsync();
            }
            return output;
        }
        #endregion


        private void Tabel_Click(object sender, RoutedEventArgs e)
        {
            TabelWindow tabelWindow = new TabelWindow();
            tabelWindow.Show();
            this.Hide();
        }

        private void Indtag_OnClick(object sender, RoutedEventArgs e)
        {
            Indtag IndtabWindow = new Indtag();
            IndtabWindow.Show();
            this.Hide();
        }

        private void DatePicker_OnCalendarClosed(object sender, RoutedEventArgs e)
        {
            var energi = (from k in _kalorieIndtags
                where k.Day == this.DatePicker.SelectedDate.Value
                select k.Indtag).First();

            this.KalorieprDag.Text = energi.ToString();
        }

        private void ChartButton_OnClick(object sender, RoutedEventArgs e)
        {
            ChartWindow window = new ChartWindow(_kalorieIndtags);
            window.Show();
            this.Hide();
        }
    }
}
