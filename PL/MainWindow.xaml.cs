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
            DependencyProperty.Register("Date", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));

        //Ctor
        public MainWindow()
        {
            InitializeComponent();
            Date = s_bl.Clock;
            User = new BO.User() { UserId = 0, Password = "", IsAdmin = false };
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

        //LogIn and SighInSystem


        //יצירת משתמש של המנדס
        public BO.User User
        {
            get { return (BO.User)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("User", typeof(BO.User), typeof(MainWindow), new PropertyMetadata(null));
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
        }
    }
}