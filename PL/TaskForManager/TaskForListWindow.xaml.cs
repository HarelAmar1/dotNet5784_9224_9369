using PL.Engineer;
using PL.Task;
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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskForListWindow.xaml
    /// </summary>
    public partial class TaskForListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public TaskForListWindow()
        {
            InitializeComponent();
            TaskForList = s_bl?.Task.ReadAll()!;
        }

        public IEnumerable<BO.TaskInList> TaskForList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskForListProperty); }
            set { SetValue(TaskForListProperty, value); }
        }
        public static readonly DependencyProperty TaskForListProperty =
        DependencyProperty.Register("TaskForList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskForListWindow), new PropertyMetadata(null));


        public BO.EngineerExperience Copmlexity { get; set; } = BO.EngineerExperience.All;

        private void cbTaskDataFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskForList = (Copmlexity == BO.EngineerExperience.All) ?
            s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => (int)item.Copmlexity == (int)Copmlexity)!;
        }

        private void bcPreesToUpdate(object sender, MouseButtonEventArgs e)
        {
            BO.TaskInList selectedTask = (BO.TaskInList)((ListView)sender).SelectedItem;
            new TaskWindow(selectedTask.Id).Show();
            Close();
        }

        private void bcPreesToAdd(object sender, RoutedEventArgs e)
        {
            new TaskWindow(0).Show();
            Close();
        }
    }
}

