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

namespace PL.EngineerForEngineer
{
    /// <summary>
    /// Interaction logic for EngineerLogInWindow.xaml
    /// </summary>
    public partial class EngineerLogInWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        //תעודת זהות של המנדס
        public BO.User User
        {
            get { return (BO.User)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("User", typeof(BO.User), typeof(EngineerLogInWindow), new PropertyMetadata(null));


        public EngineerLogInWindow()
        {
            InitializeComponent();
            User = new BO.User() { IsAdmin = false };
        }



        private void bcLogIn(object sender, RoutedEventArgs e)
        {
            try
            {
                if (s_bl.User.Read(User.UserId).Password == User.Password)
                {
                    if (s_bl.User.Read(User.UserId).IsAdmin)
                    {
                        new ManagerWindow().Show();
                        Close();
                    }
                    else
                    {
                        new EngineerWindow(User.UserId).Show();
                        Close();
                    }
                }
                else
                {
                    throw new Exception("Your password is incorrect");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void bcSignIn(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.User.Create(User);
                MessageBox.Show("Login Successful!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Close();
        }
    }
}
