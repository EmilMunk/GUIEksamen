using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
        }

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
