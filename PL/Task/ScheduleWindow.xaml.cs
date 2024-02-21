using PL.Engineer;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for ScheduleWindow.xaml
    /// </summary>


    public partial class ScheduleWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Date.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(ScheduleWindow), new PropertyMetadata(DateTime.Now));


        public ScheduleWindow()
        {
            InitializeComponent();
        }

        private void bcInsetTheSchedule(object sender, RoutedEventArgs e)
        {
            s_bl.Task.dateGeneratorOfAllTasks(Date);
            Close();
        }
    }
}