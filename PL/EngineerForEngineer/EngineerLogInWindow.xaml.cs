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
            DependencyProperty.Register("Id", typeof(BO.User), typeof(EngineerLogInWindow), new PropertyMetadata(null));


        public EngineerLogInWindow()
        {
            InitializeComponent();
        }



        private void bcLogIn(object sender, RoutedEventArgs e)
        {
            try
            {
               
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void bcSignIn(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
