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



    public class WPFKalorieIndtag : ObservableCollection<AllData>, INotifyPropertyChanged
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


            var toDisplay = readDbDataToDisplay();

            foreach (var index in toDisplay)
            {
                if (index.Day == DateTime.Today)
                    Add(index);
            }

            TotalKalorier = 0;

        }

        #region Diverse

        #region GetDataFromDatabase
        static List<AllData> readDbDataToDisplay()
        {
            var result = Task.Run(() => GetDataToDisplay()).Result;
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

        static async Task<string> GetDataToDisplay()
        {
            string output = null;

            var response = await _client.GetAsync($"api/IndtastIndtagAPI");
            if (response.IsSuccessStatusCode)
            {
                output = await response.Content.ReadAsStringAsync();
            }
            return output;
        }

        static async void DeleteIndtag(int id)
        {
            // StringContent content = new StringContent(JsonConvert.SerializeObject(toInsert));
            //string json = JsonConvert.SerializeObject(toInsert);

            //var response = await _client.PostAsync("api/KalorieIndholds", new StringContent(json, Encoding.UTF8, "application/json"));

            var response = await _client.DeleteAsync($"api/IndtastIndtagAPI/{id}");

        }

        #endregion



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

        static async void AddIndtag(AllData toInsert)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(toInsert));
            string json = JsonConvert.SerializeObject(toInsert);

            var response = await _client.PostAsync("api/IndtastIndtagAPI", new StringContent(json, Encoding.UTF8, "application/json"));
        }
        #endregion


        #region Property


        int currentIndex = -1;

        public int CurrentIndex
        {
            get { return currentIndex; }
            set
            {
                if (currentIndex != value)
                {
                    currentIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private double _totalKalorier;

        public double TotalKalorier
        {
            get { return _totalKalorier; }
            set
            {
                _totalKalorier = 0;
                foreach (var VARIABLE in this)
                {
                    _totalKalorier += VARIABLE.Kalorier;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        private ICommand _deleteCommand;

        public ICommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new RelayCommand(DeleteFødevare)); }
        }

        private void DeleteFødevare()
        {
            var allData = readDbDataToDisplay();

            var name = this[CurrentIndex];

            var ToDelete = (from d in this
                            where (d.MadVare == name.MadVare
                            && d.Kalorier == name.Kalorier &&
                            d.Amount == name.Amount)
                            select d).First();

            DeleteIndtag(ToDelete.ID);
            //Remove(ToDelete);
            this.RemoveItem(currentIndex);
            NotifyPropertyChanged();
            TotalKalorier = 0;
        }


        private ICommand _closeCommand;

        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(AddIndtag)); }
        }

        private void AddIndtag()
        {
            //AllData toInsert = new AllData();
            //toInsert.Day = DateTime.Today;
            //toInsert.Indtag = int.Parse(TotalKalorier.ToString());
            //AddIndtag(toInsert);
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
            AllData newAgent = new AllData();
            dlg.DataContext = newAgent;
            if (dlg.ShowDialog() == true)
            {
                var energi = (from a in madevarer
                              where a.MadVare == newAgent.MadVare
                              select a.Energi).First();

                newAgent.Kalorier = (float.Parse(energi)) * newAgent.Amount / 100;
                newAgent.Day = DateTime.Today;
                //Add(newAgent);
                CurrentFødevare = newAgent;
                //CurrentSpecialityIndex = 0;
                //dirty = true;
                //AddFødevare(newAgent);
                NotifyPropertyChanged();
                Add(newAgent);
                AddIndtag(newAgent);

                TotalKalorier = 0;
            }
        }
        AllData currentFødevare = null;

        public AllData CurrentFødevare
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
