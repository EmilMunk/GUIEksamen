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
using System.Windows.Shapes;

namespace ReEksamenVinter16_17
{

    public class ChartClass
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }

    }
    /// <summary>
    /// Interaction logic for ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {
        private bool StartChoosen = false;
        private bool EndChoosen = false;

        private List<IndtagAfKalorier> AllData = new List<IndtagAfKalorier>();
        public ChartWindow(List<IndtagAfKalorier> liste)
        {
            InitializeComponent();
            AllData = liste;
        }

        private void DisplayButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (StartChoosen == true && EndChoosen == true)
            {
                StartChoosen = false;
                StartChoosen = false;

                var ToDisplay = (from d in AllData
                                where (d.Day >= this.DatePickerStart.SelectedDate && d.Day <= this.DatePickerSlut.SelectedDate)
                                select d).ToList();

                List<ChartClass> toDisplayList = new List<ChartClass>();

                foreach (var i in ToDisplay)
                    toDisplayList.Add(new ChartClass() {Date = i.Day, Count = i.Indtag});

                this.Chart.ItemsSource = toDisplayList;
            }
 
        }

        private void DatePickerStart_CalendarClosed(object sender, RoutedEventArgs e)
        {
            StartChoosen = true;
        }

        private void DatePickerSlut_CalendarClosed(object sender, RoutedEventArgs e)
        {
            EndChoosen = true;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();
        }
    }
}
