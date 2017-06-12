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
    /// Interaction logic for EditView.xaml
    /// </summary>
    /// 
    public partial class EditView : Window
    {
        public ObservableCollection<BackLog.State> cmbContent1 { get; set; }
        public ObservableCollection<int> cmbContent2 { get; set; }
        public EditView()
        {
            InitializeComponent();
            cmbContent1 = new ObservableCollection<BackLog.State>();
            cmbContent1.Add(BackLog.State.Backlog);
            cmbContent1.Add(BackLog.State.IsToDo);
            cmbContent1.Add(BackLog.State.IsDoing);

            cmbContent1.Add(BackLog.State.Done);
            comboStates.ItemsSource = cmbContent1;

            cmbContent2 = new ObservableCollection<int>();
            cmbContent2.Add(1);
            cmbContent2.Add(2);
            cmbContent2.Add(3);
            cmbContent2.Add(4);
            ComboPrio.ItemsSource = cmbContent2;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void DoneButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
