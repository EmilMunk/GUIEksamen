using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using MvvmFoundation.Wpf;

namespace PhotoRename
{
    public class Model : INotifyPropertyChanged
    {
        private string _appTitle = "PhotoRename";
        private List<string> _imageSource = new List<string>();
        private List<BitmapImage> _images = new List<BitmapImage>();
        private List<string> _toBeDeleted = new List<string>();
        private string _imageNames = "";
        private string _shownPhoto = "";
        private string _copiedPhoto = "";
        private bool _started = false;
        private int _openedcounter = 1;


        ICommand _loadPhotos;
        ICommand _saveCommand;

        public ICommand LoadPhotosCommand
        {
            get { return _loadPhotos ?? (_loadPhotos = new RelayCommand(LoadPhotosCommand_Execute)); }
        }

        private void LoadPhotosCommand_Execute()
        {
            if (!_started)
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(@"Temp\");

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
                _started = true;
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select photo to rename";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog(App.Current.MainWindow) == true)
            {
                int count = 0;
                foreach (var item in openFileDialog.FileNames)
                {
                    //_images.Add(LoadBitmapImage(item));
                    _imageSource.Add(item);

                    if (count == 0)
                    {
                        _copiedPhoto = @"Temp\TempImage"+_openedcounter+".png";
                        File.Copy(item, _copiedPhoto);
                        Photo = System.IO.Path.GetFullPath(_copiedPhoto);
                    }
                    _openedcounter++;
                    count++;
                }
            }
        }

        public BitmapImage LoadBitmapImage(string fileName)
        {
            _imageSource.Add(fileName);
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }

        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(SaveCommand_Execute)); }
        }

        private void SaveCommand_Execute() // =0
        {
            int index = 1;
            string firstFile = "";
            foreach (var item in _imageSource) // =O :D :O :S :'(
            {
                string filePath = item.Substring(0, item.LastIndexOf(@"\") + 1);
                string extension = item.Substring(item.LastIndexOf('.'), item.Length - item.LastIndexOf('.'));
                filePath = filePath + _imageNames + " - " + index + extension;
                
                System.IO.File.Copy(item, filePath);

                if (index == 1)
                {
                    Photo = "";
                    Photo = filePath;
                }

                _toBeDeleted.Add(item);
                index++;
            }
            foreach (var i in _toBeDeleted)
            {
                DeleteOldPhoto(i);
            }
            _imageSource.Clear();
            _images.Clear();
            _toBeDeleted.Clear();
        }

        private void DeleteOldPhoto(string filePath)
        {
            File.Delete(filePath);
        }

        #region Properties

        public string ImageNames
        {
            get
            {
                return _imageNames;
            }
            set
            {
                if (value != _imageNames)
                {
                    _imageNames = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Photo
        {
            get
            {
                return _shownPhoto;
            }
            set
            {
                _shownPhoto = value;
                NotifyPropertyChanged();
            }
        }

        public List<string> Photos
        {
            get
            {
                return _imageSource;
            }
            set
            {
                _imageSource = value;
                NotifyPropertyChanged();
                
            }
        }

        public string AppTitle
        {
            get
            {
                return _appTitle;               
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
