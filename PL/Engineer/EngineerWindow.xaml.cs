using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerWindowProperty); }
            set { SetValue(EngineerWindowProperty, value); }
        }

        public static readonly DependencyProperty EngineerWindowProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));
        public EngineerWindow(int Id=0)//לזכור לתפוס חריגות
        {
            InitializeComponent();

            if (Id != 0)//update
            {
                Engineer  = s_bl.Engineer.Read(Id);
            }
            else//add
            {
                TaskInEngineer temp = new TaskInEngineer(0,"");
                Engineer = new BO.Engineer() { Id = 0, Name = "", Cost = 0, Task = temp, Email = "", Level = 0 };
            }
        }

        private void bcADD(object sender, RoutedEventArgs e)
        {
            
            BO.TaskInEngineer temp = new BO.TaskInEngineer(s_bl.Task.Read(Engineer.Task.Id).Id, s_bl.Task.Read(Engineer.Task.Id).Alias);
            BO.Engineer engineer = new BO.Engineer()
            {
                Id = Engineer.Id,
                Name = Engineer.Name,
                Email = Engineer.Email,
                Level = Engineer.Level,
                Cost = Engineer.Cost,
                Task = temp
        };
            s_bl.Engineer.Create(engineer);
            Close();
            new EngineerListWindow().Show();
        }

        private void bcUPDATE(object sender, RoutedEventArgs e)
        {
            var a = s_bl.Task.Read(Engineer.Task.Id).Id;
            BO.TaskInEngineer temp = new BO.TaskInEngineer(s_bl.Task.Read(Engineer.Task.Id).Id, s_bl.Task.Read(Engineer.Task.Id).Alias);
            BO.Engineer engineer = new BO.Engineer()
            {
                Id = Engineer.Id,
                Name = Engineer.Name,
                Email = Engineer.Email,
                Level = Engineer.Level,
                Cost = Engineer.Cost,
                Task = temp
            };
            s_bl.Engineer.Update(engineer);

            Close();
            new EngineerListWindow().Show();
        }
    }
}