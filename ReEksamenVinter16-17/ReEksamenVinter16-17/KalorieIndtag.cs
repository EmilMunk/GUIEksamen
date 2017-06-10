using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MvvmFoundation.Wpf;
using Newtonsoft.Json;

namespace ReEksamenVinter16_17
{

    public class IndtagAfKalorier
    {
        public int ID { get; set; }
        public DateTime Day { get; set; }
        public int Indtag { get; set; }
    }



    public class WPFKalorieIndtag : ObservableCollection<KalorieIndtag>, INotifyPropertyChanged
    {
        private static HttpClient _client = new HttpClient();

        private List<Fødevare> madevarer = new List<Fødevare>();

        public WPFKalorieIndtag()
        {
            if (_client.BaseAddress == null)
            {
                _client.BaseAddress = new Uri("http://localhost:4200/");

                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            var toInsert = readDbData();

            foreach (var i in toInsert)
                madevarer.Add(i);

        }

        #region Diverse
        static List<Fødevare> readDbData()
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

            var i = JsonConvert.DeserializeObject<List<Fødevare>>(result, settings);

            return i;

        }

        static async Task<string> GetData()
        {
            string output = null;

            var response = await _client.GetAsync($"api/KalorieIndholds");
            if (response.IsSuccessStatusCode)
            {
                output = await response.Content.ReadAsStringAsync();
            }
            return output;
        }

        static async void AddIndtag(IndtagAfKalorier toInsert)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(toInsert));
            string json = JsonConvert.SerializeObject(toInsert);

            var response = await _client.PostAsync("api/KalorieIndtagAPI", new StringContent(json, Encoding.UTF8, "application/json"));
        }
        #endregion


        #region Property

        private double _totalKalorier;

        public double TotalKalorier
        {
            get { return _totalKalorier; }
            set
            {
                foreach (var VARIABLE in this)
                {
                    _totalKalorier += VARIABLE.Energi;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        private ICommand _closeCommand;

        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(AddIndtag)); }
        }

        private void AddIndtag()
        {
            IndtagAfKalorier toInsert = new IndtagAfKalorier();
            toInsert.Day = DateTime.Today;
            toInsert.Indtag = int.Parse(TotalKalorier.ToString());
            AddIndtag(toInsert);
        }


        ICommand _addCommand;
        public ICommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new RelayCommand(AddAgent)); }
        }

        private void AddAgent()
        {
            var list = new List<string>();
            foreach (var i in madevarer)
            {
                list.Add(i.MadVare);
            }

            var dlg = new AddIndtag(list);
            dlg.Title = "Add New Agent";
            KalorieIndtag newAgent = new KalorieIndtag();
            dlg.DataContext = newAgent;
            if (dlg.ShowDialog() == true)
            {

                var energi = (from a in madevarer
                              where a.MadVare == newAgent.MadVare
                              select a.Energi).First();

                newAgent.Energi = (double.Parse(energi)) * newAgent.Gram / 1000;
                Add(newAgent);
                CurrentFødevare = newAgent;
                //CurrentSpecialityIndex = 0;
                //dirty = true;
                //AddFødevare(newAgent);
                NotifyPropertyChanged();
                TotalKalorier = 0;
            }
        }
        KalorieIndtag currentFødevare = null;

        public KalorieIndtag CurrentFødevare
        {
            get { return currentFødevare; }
            set
            {
                if (currentFødevare != value)
                {
                    currentFødevare = value;
                    NotifyPropertyChanged();
                }
            }
        }


        #region INotifyPropertyChanged implementation

        public new event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }

    public class KalorieIndtag
    {
        public string MadVare { get; set; }
        public int Gram { get; set; }
        public double Energi { get; set; }
    }
}
