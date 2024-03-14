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
using PL.Engineer;
using PL.Task;

namespace PL
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public ManagerWindow()
        {
            InitializeComponent();
        }

        private void bcEngineer(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().ShowDialog();
        }
        private void bcTask(object sender, RoutedEventArgs e)
        {
            new TaskForListWindow().ShowDialog();
        }

        private void bcSchedule(object sender, RoutedEventArgs e)
        {
            new ScheduleWindow().ShowDialog();
        }

        private void bcGantt(object sender, RoutedEventArgs e)
        {
            try
            {
                // check that there is  a project start date
                if (s_bl.Schedule.getStartDateOfProject() == null)
                    throw new Exception("PROJECT START DATE REQUIRED.");


                if (s_bl.Task == null || s_bl.Task.ReadAll().Count() == 0)
                    throw new Exception("There is no Data");

                new GanttWindow().ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //Reset DB func
        private void bcResetDB(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxButton buttons = MessageBoxButton.OKCancel;
                MessageBoxResult a = MessageBox.Show("Would you like to Reset the DB?", "Data Reset", buttons);
                if (a == MessageBoxResult.OK)
                {
                    BlApi.Factory.Get().ResetDB();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Init DB func
        private void bcInitDB(object sender, RoutedEventArgs e)
        {

            try
            {
                MessageBoxButton buttons = MessageBoxButton.OKCancel;
                MessageBoxResult a = MessageBox.Show("Would you like to create Initial data?", "Data Initial", buttons);
                if (a == MessageBoxResult.OK)
                {
                    BlApi.Factory.Get().ResetDB();
                    BlApi.Factory.Get().InitializeDB();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bcReturnBack(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
