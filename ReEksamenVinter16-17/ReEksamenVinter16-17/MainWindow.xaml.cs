using System;
using System.Collections;
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


    public class AllData
    {
        public int ID { get; set; }
        public DateTime Day { get; set; }
        public string MadVare { get; set; }
        public int Amount { get; set; }
        public float Kalorier { get; set; }
    }



    public static class MyExtensions
    {
        public static IEnumerable EachDay(this DateTime start, DateTime end)
        {
            // Remove time info from start date (we only care about day). 
            DateTime currentDay = new DateTime(start.Year, start.Month, start.Day);

            while (currentDay <= end)
            {
                yield return currentDay;
                currentDay = currentDay.AddDays(1);
            }
        }
    }

    public class test
    {
        public DateTime Date { get; set; }
        public float Count { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static HttpClient _client = new HttpClient();

        private List<AllData> _kalorieIndtags = new List<AllData>();

        private List<test> ToSendData = new List<test>();


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


            string today = "10/06-2017";
            DateTime start = DateTime.Parse(today);

            DateTime end = DateTime.Today;

            foreach (DateTime day in start.EachDay(end))
            {
                var ToInsertInList = new test();
                ToInsertInList.Date = day;
                foreach (var i in _kalorieIndtags)
                {
                    ToInsertInList.Count += i.Kalorier;
                }
                ToSendData.Add(ToInsertInList);
            }



            //var list = new List<test>();
            //list.Add(new test() { Name = "Land1", Count = 1000 });



        }

        #region GetDataFromDatabase
        static List<AllData> readDbData()
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

            var i = JsonConvert.DeserializeObject<List<AllData>>(result, settings);

            return i;

        }

        static async Task<string> GetData()
        {
            string output = null;

            var response = await _client.GetAsync($"api/IndtastIndtagAPI");
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
            var energi = (from k in ToSendData
                          where k.Date == this.DatePicker.SelectedDate.Value
                          select k.Count).First();

            this.KalorieprDag.Text = energi.ToString();



        }

        private void ChartButton_OnClick(object sender, RoutedEventArgs e)
        {
            ChartWindow window = new ChartWindow(ToSendData);
            window.Show();
            this.Hide();
        }
    }
}
