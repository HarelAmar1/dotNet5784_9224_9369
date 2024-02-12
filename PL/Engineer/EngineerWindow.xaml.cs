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

        public static readonly DependencyProperty EngineerWindowProperty =
            DependencyProperty.Register("Engineer", typeof(IEnumerable<BO.Engineer>), typeof(EngineerWindow), new PropertyMetadata(null));
        public EngineerWindow(int windowId = 0)//לזכור לתפוס חריגות
        {   
            InitializeComponent();
            if (windowId != 0)//update
            {
                Add.Visibility = Visibility.Hidden;
                Add.IsEnabled = false;
                BO.Engineer temp = s_bl.Engineer.Read(windowId);
                Id.Text = temp.Id.ToString();
                Name.Text = temp.Name.ToString();
                Email.Text = temp.Email.ToString(); 
                Level.Text = temp.Level.ToString();
                Cost.Text = temp.Cost.ToString();
                EngineerTask.Text = temp.Task.Id.ToString();
            }
            else
            {
                Update.Visibility = Visibility.Hidden;
                Update.IsEnabled = false;
                Id.Text = "0";
                Name.Text = "";
                Email.Text = "";
                Level.Text = "";
                Cost.Text = "";
                EngineerTask.Text = "";
            }
        }

        private void bcADD(object sender, RoutedEventArgs e)
        {
            BO.TaskInEngineer temp = new BO.TaskInEngineer(int.Parse(EngineerTask.Text), s_bl.Task.Read(int.Parse(EngineerTask.Text)).Alias);
            var a = Enum.Parse<BO.EngineerExperience>(Level.Text);
            BO.Engineer engineer = new BO.Engineer()
            {
                Id = int.Parse(Id.Text),
                Name = Name.Text,
                Email = Email.Text,
                Level = Enum.Parse<BO.EngineerExperience>(Level.Text),
                Cost = double.Parse(Cost.Text),
                Task = temp
            };
            s_bl.Engineer.Create(engineer);
            this.Close();
        }

        private void bcUPDATE(object sender, RoutedEventArgs e)
        {
            BO.TaskInEngineer temp = new BO.TaskInEngineer(int.Parse(EngineerTask.Text), s_bl.Task.Read(int.Parse(EngineerTask.Text)).Alias);
            BO.Engineer engineer = new BO.Engineer()
            {
                Id = int.Parse(Id.Text),
                Name = Name.Text,
                Email = Email.Text,
                Level = (BO.EngineerExperience)int.Parse(Level.Text),
                Cost = double.Parse(Cost.Text),
                Task = temp
            };
            s_bl.Engineer.Update(engineer);
        }
    }
}