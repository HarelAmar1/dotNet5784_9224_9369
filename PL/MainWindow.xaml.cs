using PL.Engineer;
using BlApi;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PL.Task;
using PL.EngineerForEngineer;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Date.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(s_bl.Clock));

        public MainWindow()
        {
            InitializeComponent();
            Date = s_bl.Clock;
        }

        private void bcEngineer(object sender, RoutedEventArgs e)
        {
            new EngineerLogInWindow().ShowDialog();
        }

        private void bcManager(object sender, RoutedEventArgs e)
        {
            new ManagerWindow().ShowDialog();
        }

        private void AddedInAnHour(object sender, RoutedEventArgs e)
        {
            s_bl.AddedInAnHour();
            Date = s_bl.Clock;
        }


        private void AddedInAnDay(object sender, RoutedEventArgs e)
        {
            s_bl.AddedInAnDay();
            Date = s_bl.Clock;
        }

        private void AddedInAnYear(object sender, RoutedEventArgs e)
        {
            s_bl.AddedInAnYear();
            Date = s_bl.Clock;
        }

        private void InitADate(object sender, RoutedEventArgs e)
        {
            s_bl.TimeReset();
            Date = s_bl.Clock;
        }
    }
}