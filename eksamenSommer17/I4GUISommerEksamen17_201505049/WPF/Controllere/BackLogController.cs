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
    public class Chartclass
    {
        public string Sprint { get; set; }
        public double Count { get; set; }
    }

    /// <summary>
    /// Min observable collection. Den binder sig op af min model.
    /// </summary>
    public class BackLogController : ObservableCollection<BackLog>, INotifyPropertyChanged
    {
        private static HttpClient _client = new HttpClient();

        public ObservableCollection<BackLog> _toDo = new ObservableCollection<BackLog>();
        public ObservableCollection<BackLog> _doing = new ObservableCollection<BackLog>();
        public ObservableCollection<BackLog> _backlog = new ObservableCollection<BackLog>();
        public ObservableCollection<BackLog> _done = new ObservableCollection<BackLog>();

        private SolidColorBrush _backgroundColor;

        private ObservableCollection<Chartclass> _toDisplayList = new ObservableCollection<Chartclass>();

        public ObservableCollection<Chartclass> ToDisplayList
        {
            get
            {
                return _toDisplayList;
            }
            set
            {
                _toDisplayList = value;
                NotifyPropertyChanged();
            }
        }



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

        /// <summary>
        /// Ctor. Need to read all data from WebAPI (Database), and populate the above ObservableCollections. 
        /// These bind to 4 different listbox in View.
        /// </summary>
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

            //Here I read settings file for background color of main window.
            string color = Properties.Settings.Default.Color;
            var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            BackgroundColor = brush;

            NotifyPropertyChanged();

            UpdateChart();


        }

        /// <summary>
        /// Method that will update the observablecollection of the charts context.
        /// This is bound to the view Databinding on the observablecollection
        /// </summary>
        public void UpdateChart()
        {
            ToDisplayList.Clear();
            double added = 0;
            double addedTotal = 0;
            foreach (var index in ToDo)
            {
                added += index.EstimatedTime;
            }
            foreach (var index in Doing)
            {
                added += index.EstimatedTime;
            }
            foreach (var index in Backlog)
            {
                added += index.EstimatedTime;
            }

            addedTotal = added;

            foreach (var index in Done)
            {
                addedTotal += index.EstimatedTime;
            }

            var toInsert = new ObservableCollection<Chartclass>();
            toInsert.Add(new Chartclass() { Count = added, Sprint = "Tid tilbage" });
            toInsert.Add(new Chartclass() { Count = addedTotal, Sprint = "Tid totalt" });
            ToDisplayList = toInsert;
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

        private ICommand _refresh;
        public ICommand Refresh
        {
            get { return _refresh ?? (_refresh = new RelayCommand(RefreshMainWindow)); }
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
        }

        /// <summary>
        /// Method that will refresh all the data in the view. Specifically it will add a backlog item to the new correct Observable list
        /// This is called from the edit Command, so we after deleted the item from the prvious view, add it to the new view.
        /// </summary>
        /// <param name="item">Specific item you want to change to a new listbox in the view</param>
        public void RefreshMainWindow(BackLog item)
        {
                if (item.States == BackLog.State.IsToDo)
                    ToDo.Add(item);
                else if (item.States == BackLog.State.IsDoing)
                    Doing.Add(item);
                else if (item.States == BackLog.State.Backlog)
                    Backlog.Add(item);
                else
                    Done.Add(item);
        }

      

        #region WebAPI 

        /// <summary>
        /// Method that will get JSON from WebAPI, and deserialize the string to the object.
        /// </summary>
        /// <returns>The Deserialized list</returns>
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
        /// <summary>
        /// GetAsync all objects in BackLog
        /// </summary>
        /// <returns></returns>
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
            string json = JsonConvert.SerializeObject(toInsert);

            var response = await _client.PostAsync("api/BackLogAPI", new StringContent(json, Encoding.UTF8, "application/json"));
        }

        static async void PutTask(BackLog toInsert)
        {
            string json = JsonConvert.SerializeObject(toInsert);

            var response = await _client.PutAsync($"api/BackLogAPI/{toInsert.BackLogId}", new StringContent(json, Encoding.UTF8, "application/json"));
        }
        static async void DeleteTask(BackLog toInsert)
        {
            string json = JsonConvert.SerializeObject(toInsert);
            var response = await _client.DeleteAsync($"api/BackLogAPI/{toInsert.BackLogId}");
        }

    #endregion

        #region Commands

        private ICommand _newScrumCommand;

        public ICommand NewScrumCommand
        {
            get { return _newScrumCommand ?? (_newScrumCommand = new RelayCommand(NewScrumCommandExecute)); }
        }
        /// <summary>
        /// Execution of new scrum that will dete tasks from Done, and put all current task to BackLog.
        /// </summary>
        public void NewScrumCommandExecute()
        {
            foreach (var i in Done)
            {
                DeleteTask(i);
                
            }

            Done.Clear();

            foreach (var i in Doing)
            {
                i.States = BackLog.State.Backlog;
                PutTask(i);
                Backlog.Add(i);
            }
            foreach (var i in ToDo)
            {
                i.States = BackLog.State.Backlog;
                PutTask(i);
                Backlog.Add(i);
            }

            Doing.Clear();
            ToDo.Clear();


        }

        ICommand _addCommand;
        public ICommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new RelayCommand(AddTask)); }
        }

        /// <summary>
        /// Execeute that will add a new task. This will show a dialog with the AddTask.xaml window.
        /// </summary>
        private void AddTask()
        {

            BackLog newItem = new BackLog();
            var dlg = new AddTask();
            dlg.Title = "Add new task";
            BackLog newTask = new BackLog();
            dlg.DataContext = newTask;
            // Here we have selected done, and we are ready to push the data to the database, and update the main window view.
            if (dlg.ShowDialog() == true)
            {
                newTask.States = BackLog.State.Backlog;
                
                NotifyPropertyChanged();
                Backlog.Add(newTask);

                AddTask(newTask);

            }
            UpdateChart();
        }

        ICommand _showGraph;
        public ICommand ShowGraph
        {
            get { return _showGraph ?? (_showGraph = new RelayCommand(ShowGraphExecute)); }
        }

        /// <summary>
        /// Show the graph in a modeless window, so it can be displayed alon side the main scrum board view.
        /// </summary>
        private void ShowGraphExecute()
        {
            var view = new Chart();
            view.DataContext = this;
            view.Show();
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

        /// <summary>
        /// Settings command, to change background color to aquamarine. This will be saved in settings.settings as a string.
        /// </summary>
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

        /// <summary>
        /// Settings command, to change background color to white. This will be saved in settings.settings as a string.
        /// </summary>
        private void ChangeColorToDefaultExecute()
        {
            Properties.Settings.Default.Color = "White";
            Properties.Settings.Default.Save();
            string color = Properties.Settings.Default.Color;
            var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            BackgroundColor = brush;
        }

        /// <summary>
        /// The edit execute command. This will be called from mainwindow code behind, since it is a double click method.
        /// </summary>
        /// <param name="name">A string that define which Listbox the method was called from</param>
        public void ViewDetails(string name)
        {
            var dlg = new EditView();
            BackLog toEdit = new BackLog();
            //Each switch case will open a window see above, and will set the datacontext of the view to the current selected object in the specified listbox
            //When this window is succesfully completed, it will sync the possible new data with the database, and remove it from the possible old list.
            //After this it wil add it to the possible new list (if we have changed status or not).
            switch (name)
            {
                case "Backlog":
                    toEdit = Backlog[BackLogCurrentIndex];
                    dlg.DataContext = toEdit;

                    if (dlg.ShowDialog() == true)
                    {
                        PutTask(toEdit);
                        Backlog.Remove(toEdit);
                        RefreshMainWindow(toEdit);
                    }
                    break;
                case "IsToDo":
                    toEdit = ToDo[ToDoCurrentIndex];
                    dlg.DataContext = toEdit;

                    if (dlg.ShowDialog() == true)
                    {
                        PutTask(toEdit);
                        ToDo.Remove(toEdit);
                        RefreshMainWindow(toEdit);
                    }
                    break;
                case "Doing":
                    toEdit = Doing[DoingCurrentIndex];
                    dlg.DataContext = toEdit;

                    if (dlg.ShowDialog() == true)
                    {
                        PutTask(toEdit);
                        Doing.Remove(toEdit);
                        RefreshMainWindow(toEdit);
                    }
                    break;
                case "Done":
                    toEdit = Done[DoneCurrentIndex];
                    dlg.DataContext = toEdit;

                    if (dlg.ShowDialog() == true)
                    {
                        PutTask(toEdit);
                        Done.Remove(toEdit);
                        RefreshMainWindow(toEdit);
                    }
                    break;
                default:
                    break;
            }
            UpdateChart();
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
