using BO;
using DO;
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
        public EngineerWindow(int Id=0)
        {
            InitializeComponent();

            if (Id != 0)//update
            {
                Engineer  = s_bl.Engineer.Read(Id);
            }
            else//add
            {
                Engineer = new BO.Engineer() { Id = 0, Name = "Default", Cost = 0, Email = "@gmail.com", Level = 0 };
            }
        }

        private void bcADD(object sender, RoutedEventArgs e)
        {
            BO.Engineer engineer = new BO.Engineer()
            {
                Id = Engineer.Id,
                Name = Engineer.Name,
                Email = Engineer.Email,
                Level = Engineer.Level,
                Cost = Engineer.Cost
                //Task = null
            };
            try
            {
                s_bl.Engineer.Create(engineer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } // Exception handling
            Close();
            new EngineerListWindow().Show();
        }

        private void bcUPDATE(object sender, RoutedEventArgs e)
        {
            BO.Engineer engineer = new BO.Engineer()
            {
                Id = Engineer.Id,
                Name = Engineer.Name,
                Email = Engineer.Email,
                Level = Engineer.Level,
                Cost = Engineer.Cost,
                //Task = null
            };
            try
            {
                s_bl.Engineer.Update(engineer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } // Exception handling
                Close();
            new EngineerListWindow().Show();
        }
    }
}