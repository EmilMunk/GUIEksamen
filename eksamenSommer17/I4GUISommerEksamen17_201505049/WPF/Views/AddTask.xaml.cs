using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using WPF.Modeller;

namespace WPF.Views
{
    /// <summary>
    /// Interaction logic for AddTask.xaml
    /// </summary>
    public partial class AddTask : Window
    {
        public ObservableCollection<int> cmbContent2 { get; set; }

        public AddTask()
        {
            InitializeComponent();


            cmbContent2 = new ObservableCollection<int>();
            cmbContent2.Add(1);
            cmbContent2.Add(2);
            cmbContent2.Add(3);
            cmbContent2.Add(4);
            ComboPrio.ItemsSource = cmbContent2;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if(this.Description.Text != "" && this.EstimatedTime.Text != "" && this.Responsible.Text != "")
                DialogResult = true;
            else
            {
                MessageBox.Show("You did it wrong, try again.");
            }
        }
    }
}
