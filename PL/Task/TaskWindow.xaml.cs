using BO;
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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.Task Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }
        public static readonly DependencyProperty TaskProperty =
        DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

        public TaskWindow(int id)
        {
            InitializeComponent();
            if (s_bl.Task.Read(id) != null) //update
            {
                var taskFromDal = s_bl.Task.Read(id);
                Task = new BO.Task() { Id = taskFromDal.Id,Description=taskFromDal.Description,Alias=taskFromDal.Alias,Status=taskFromDal.Status};
            }
            else//add
            {
                Task = new BO.Task() { Id = 0, Description = "Description", Alias = "Alias", Status = 0 };
            }
        }

        private void bcAdd(object sender, RoutedEventArgs e)
        {
            BO.Task task = new BO.Task()
            {
                Id = Task.Id,
                Description = Task.Description,
                Alias = Task.Alias,
                Status = Task.Status,
            };
            try
            {
                s_bl.Task.Create(task);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } // Exception handling
            Close();
            new TaskForListWindow().Show();
        }

        private void bcUpdate(object sender, RoutedEventArgs e)
        {
            BO.Task task = new BO.Task()
            {
                Id = Task.Id,
                Description = Task.Description,
                Alias = Task.Alias,
                Status = Task.Status,
            };
            try
            {
                s_bl.Task.Update(task);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } // Exception handling
            Close();
            new TaskForListWindow().Show();
        }
    }
}
