using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MvvmFoundation.Wpf;
using Newtonsoft.Json;
using WPF.Modeller;
using WPF.Views;

namespace WPF.Controllere
{
    public class BackLogController : ObservableCollection<BackLog>, INotifyPropertyChanged
    {
        private static HttpClient _client = new HttpClient();

        public ObservableCollection<BackLog> _toDo = new ObservableCollection<BackLog>();
        public ObservableCollection<BackLog> _doing = new ObservableCollection<BackLog>();
        public ObservableCollection<BackLog> _backlog = new ObservableCollection<BackLog>();
        public ObservableCollection<BackLog> _done = new ObservableCollection<BackLog>();

        private SolidColorBrush _backgroundColor;


        public ObservableCollection<BackLog> ToDo
        {
            get { return _toDo; }
            set { _toDo = value; }
        }

        public ObservableCollection<BackLog> Doing
        {
            get { return _doing; }
            set { _doing = value; }
        }

        public ObservableCollection<BackLog> Backlog
        {
            get { return _backlog; }
            set { _backlog = value; }
        }

        public ObservableCollection<BackLog> Done
        {
            get { return _done; }
            set { _done = value; }
        }

        public BackLogController()
        {
            if (_client.BaseAddress == null)
            {
                _client.BaseAddress = new Uri("http://localhost:4200/");

                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            var result = readDbDataToDisplay();

            foreach (var i in result)
            {
                if (i.States == BackLog.State.IsToDo)
                    ToDo.Add(i);
                else if (i.States == BackLog.State.IsDoing)
                    Doing.Add(i);
                else if (i.States == BackLog.State.Backlog)
                    Backlog.Add(i);
                else
                    Done.Add(i);
            }

            string color = Properties.Settings.Default.Color;
            var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            BackgroundColor = brush;

            NotifyPropertyChanged();

        }

        public SolidColorBrush BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                NotifyPropertyChanged();
            }
        }

        public void RefreshMainWindow()
        {
            ToDo.Clear();
            Doing.Clear();
            Backlog.Clear();
            Done.Clear();
            var result = readDbDataToDisplay();

            foreach (var i in result)
            {
                if (i.States == BackLog.State.IsToDo)
                    ToDo.Add(i);
                else if (i.States == BackLog.State.IsDoing)
                    Doing.Add(i);
                else if (i.States == BackLog.State.Backlog)
                    Backlog.Add(i);
                else
                    Done.Add(i);
            }
            NotifyPropertyChanged();
        }

        #region WebAPI 

        static List<BackLog> readDbDataToDisplay()
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

            var i = JsonConvert.DeserializeObject<List<BackLog>>(result, settings);

            return i;

        }

        static async Task<string> GetDataToDisplay()
        {
            string output = null;

            var response = await _client.GetAsync($"api/BackLogAPI");
            if (response.IsSuccessStatusCode)
            {
                output = await response.Content.ReadAsStringAsync();
            }
            return output;
        }

        

        static async void AddTask(BackLog toInsert)
        {
            //StringContent content = new StringContent(JsonConvert.SerializeObject(toInsert));
            string json = JsonConvert.SerializeObject(toInsert);

            var response = await _client.PostAsync("api/BackLogAPI", new StringContent(json, Encoding.UTF8, "application/json"));
        }

        static async void PutTask(BackLog toInsert)
        {
            //StringContent content = new StringContent(JsonConvert.SerializeObject(toInsert));
            string json = JsonConvert.SerializeObject(toInsert);

            var response = await _client.PutAsync($"api/BackLogAPI/{toInsert.BackLogId}", new StringContent(json, Encoding.UTF8, "application/json"));
        }

        #endregion

        #region Commands

        ICommand _addCommand;
        public ICommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new RelayCommand(AddTask)); }
        }

        private void AddTask()
        {

            BackLog newItem = new BackLog();
            var dlg = new AddTask();
            dlg.Title = "Add new task";
            BackLog newTask = new BackLog();
            dlg.DataContext = newTask;
            if (dlg.ShowDialog() == true)
            {
                newTask.States = BackLog.State.Backlog;
                
                NotifyPropertyChanged();
                Backlog.Add(newTask);

                AddTask(newTask);
            }
        }

        ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(Close)); }
        }

        private void Close()
        {
            Application.Current.MainWindow.Close();
        }

        ICommand _changeColorToBlue;
        public ICommand ChangeColorToBlue
        {
            get { return _changeColorToBlue ?? (_changeColorToBlue = new RelayCommand(ChangeColorToBlueExecute)); }
        }

        private void ChangeColorToBlueExecute()
        {
            Properties.Settings.Default.Color = "Aquamarine";
            Properties.Settings.Default.Save();
            string color = Properties.Settings.Default.Color;
            var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            BackgroundColor = brush;
        }


        ICommand _changeColorToDefault;
        public ICommand ChangeColorToDefault
        {
            get { return _changeColorToDefault ?? (_changeColorToDefault = new RelayCommand(ChangeColorToDefaultExecute)); }
        }

        private void ChangeColorToDefaultExecute()
        {
            Properties.Settings.Default.Color = "White";
            Properties.Settings.Default.Save();
            string color = Properties.Settings.Default.Color;
            var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            BackgroundColor = brush;
        }


        public void ViewDetails(string name)
        {
            var dlg = new EditView();
            BackLog toEdit = new BackLog();
            switch (name)
            {
                case "Backlog":
                    toEdit = Backlog[BackLogCurrentIndex];
                    dlg.DataContext = toEdit;

                    if (dlg.ShowDialog() == true)
                    {
                        PutTask(toEdit);
                        RefreshMainWindow();
                    }
                    break;
                case "IsToDo":
                    toEdit = ToDo[ToDoCurrentIndex];
                    dlg.DataContext = toEdit;

                    if (dlg.ShowDialog() == true)
                    {
                        PutTask(toEdit);
                        RefreshMainWindow();
                    }
                    break;
                case "Doing":
                    toEdit = Doing[DoingCurrentIndex];
                    dlg.DataContext = toEdit;

                    if (dlg.ShowDialog() == true)
                    {
                        PutTask(toEdit);
                        RefreshMainWindow();
                    }
                    break;
                case "Done":
                    toEdit = Done[DoneCurrentIndex];
                    dlg.DataContext = toEdit;

                    if (dlg.ShowDialog() == true)
                    {
                        PutTask(toEdit);
                        RefreshMainWindow();
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Properties

        private int _toDoCurrentIndex = 0;
        private int _doingCurrentIndex = 0;
        private int _backlogCurrentIndex = 0;
        private int _doneCurrentIndex = 0;

        public int ToDoCurrentIndex
        {
            get { return _toDoCurrentIndex;}
            set {
                if (_toDoCurrentIndex != value)
                {
                    _toDoCurrentIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int DoingCurrentIndex
        {
            get { return _doingCurrentIndex; }
            set
            {
                if (_doingCurrentIndex != value)
                {
                    _doingCurrentIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int BackLogCurrentIndex
        {
            get { return _backlogCurrentIndex; }
            set
            {
                if (_backlogCurrentIndex != value)
                {
                    _backlogCurrentIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int DoneCurrentIndex
        {
            get { return _doneCurrentIndex; }
            set
            {
                if (_doneCurrentIndex != value)
                {
                    _doneCurrentIndex = value;
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
}
