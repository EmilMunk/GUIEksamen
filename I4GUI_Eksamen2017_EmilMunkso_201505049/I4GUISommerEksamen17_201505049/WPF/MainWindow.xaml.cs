using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
using WPF.Controllere;
using WPF.Modeller;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SolidBrush BackgroundBrush { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //BackgroundBrush = new SolidBrush((System.Drawing.Color)System.Drawing.ColorConverter.ConvertFromString(Properties.Settings.Default.Color));

        }

        // each of the blow events will bind to each lsitbox in the view. This is to call the ViewDetails, 
        //that is the "Command" in the controller, that will get the current data for a item, and give the possibility to change some or all of it.
        private void ToDoList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BackLogController context = (BackLogController)this.DataContext;
            string name = ((ListBox) sender).Name;

            context.ViewDetails(name);
        }

        private void DoingList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BackLogController context = (BackLogController)this.DataContext;
            string name = ((ListBox)sender).Name;

            context.ViewDetails(name);
        }

        private void ReviewList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BackLogController context = (BackLogController)this.DataContext;
            string name = ((ListBox)sender).Name;

            context.ViewDetails(name);
        }

        private void DoneList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BackLogController context = (BackLogController)this.DataContext;
            string name = ((ListBox)sender).Name;

            context.ViewDetails(name);
        }

       
    }
}
