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
using System.Windows.Input;
using MvvmFoundation.Wpf;
using Newtonsoft.Json;

namespace ReEksamenVinter16_17
{

    public class WPFFødevarer : ObservableCollection<Fødevare>, INotifyPropertyChanged
    {
        private static HttpClient _client = new HttpClient();

        public WPFFødevarer()
        {
            if (_client.BaseAddress == null)
            {
                _client.BaseAddress = new Uri("http://localhost:4200/");
            
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            var toInsert = readDbData();

            foreach (var i in toInsert)
                this.Add(i);

        }
        #region diverse
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

        static async void AddFødevare(Fødevare toInsert)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(toInsert));
            string json = JsonConvert.SerializeObject(toInsert);

            var response = await _client.PostAsync("api/KalorieIndholds", new StringContent(json, Encoding.UTF8, "application/json"));
        }

        static async void DeleteFødevare(int id)
        {
           // StringContent content = new StringContent(JsonConvert.SerializeObject(toInsert));
            //string json = JsonConvert.SerializeObject(toInsert);

            //var response = await _client.PostAsync("api/KalorieIndholds", new StringContent(json, Encoding.UTF8, "application/json"));

            var response = await _client.DeleteAsync($"api/KalorieIndholds/{id}");

        }

        #endregion


        #region Commands

        private ICommand _deleteCommand;

        public ICommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new RelayCommand(DeleteFødevare)); }
        }

        private void DeleteFødevare()
        {
            var allData = readDbData();

            var name = this[CurrentIndex];

            var ToDelete = (from d in allData
                where d.MadVare == name.MadVare
                select d).First();

            DeleteFødevare(ToDelete.ID);
            //Remove(ToDelete);
            this.RemoveItem(currentIndex);
            NotifyPropertyChanged();
        }

        ICommand _addCommand;
        public ICommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new RelayCommand(AddAgent)); }
        }

        private void AddAgent()
        {
            var dlg = new AddFødevare();
            dlg.Title = "Add New Agent";
            Fødevare newAgent = new Fødevare();
            dlg.DataContext = newAgent;
            if (dlg.ShowDialog() == true)
            {
                Add(newAgent);
                //CurrentSpecialityIndex = 0;
                CurrentFødevare = newAgent;
                //dirty = true;
                AddFødevare(newAgent);
                NotifyPropertyChanged();
            }
        }


        #endregion

        #region properties

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

        Fødevare currentFødevare = null;

        public Fødevare CurrentFødevare
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

        #endregion


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

    public class Fødevare
    {
        public int ID { get; set; }
        public string MadVare { get; set; }
        public string Energi { get; set; }
    }

}
