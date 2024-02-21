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

namespace PL
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

        private void btnEngineer_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }

        private void InitDB(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }

        //Init DB func
        private void bcInitDB(object sender, RoutedEventArgs e)
        {
            MessageBoxButton buttons = MessageBoxButton.OKCancel;
            MessageBoxResult a = MessageBox.Show("Would you like to create Initial data?", "Data Initial", buttons);
            if (a == MessageBoxResult.OK)
            {
                BlApi.Factory.Get().ResetDB();
                BlApi.Factory.Get().InitializeDB();
            }
        }

        //Reset DB func
        private void bcResetDB(object sender, RoutedEventArgs e)
        {
            MessageBoxButton buttons = MessageBoxButton.OKCancel;
            MessageBoxResult a = MessageBox.Show("Would you like to Reset the DB?", "Data Reset", buttons);
            if (a == MessageBoxResult.OK)
            {
                BlApi.Factory.Get().ResetDB();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ManagerWindow().Show();
        }

        private void bcTask(object sender, RoutedEventArgs e)
        {
            new TaskForListWindow().Show();
        }
        private void bcInitSchedule(object sender, RoutedEventArgs e)
        {
            
            new ScheduleWindow().Show();
        }
    }
}