using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Drawing;

namespace Vinter16_17
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> privateList;

        private string ChoosenPath = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fdb = new FolderBrowserDialog();
            fdb.ShowNewFolderButton = false;
            var result = fdb.ShowDialog();
            while (true)
            {
                if ((fdb.SelectedPath) != "")
                {
                    ChoosenPath = fdb.SelectedPath;
                    System.Windows.MessageBox.Show(ChoosenPath);

                    addItems();

                    break;
                }

            }

        }

        public void addItems()
        {
            this.List.Items.Clear();
            tbxFileName.Text = ChoosenPath;
            //var ext = new List<string> { "jpg", "gif", "png" };
            privateList = System.IO.Directory.GetFiles(ChoosenPath, "*.*", System.IO.SearchOption.TopDirectoryOnly)
               .Where(file => new string[] { ".jpg", ".gif", ".png" }
                .Contains(System.IO.Path.GetExtension(file)))
                .ToList();

            foreach (var file in privateList)
                this.List.Items.Add(System.IO.Path.GetFileName(file));
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var image = new BitmapImage(new Uri(this.tbxFileName.Text + "\\" + this.List.SelectedItem.ToString()));

                this.CurrentImage.Source = image;
                this.CurrentImage.Stretch = Stretch.Uniform;
            }
            catch
            {
                
            }
            finally
            {
                
            }
        }

        private void NewName_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentImage.Source = null;
            copyAndMove(this.tbxFileName.Text + "\\", this.NewFileName.Text);
            addItems();
        }



        public void copyAndMove(string oldName, string newName)
        {
            var index = 0;
            //DirectoryInfo d = new DirectoryInfo(oldName);
            //FileInfo[] infos = d.GetFiles();
            foreach (var f in privateList)
            {
                File.Move(f, this.tbxFileName.Text + "\\"+ newName + $" - {++index}" + ".jpg");
            }
        }

        private void DoNotShow_OnClick(object sender, RoutedEventArgs e)
        {
            var image = new BitmapImage();

            this.CurrentImage.Source = image;
            this.CurrentImage.Stretch = Stretch.Uniform;

            this.CurrentImage.Source = null;

        }
    }
}
