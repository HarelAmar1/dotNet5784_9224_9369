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
        public EngineerLogInWindow()
        {
            InitializeComponent();
        }


        //תעודת זהות של המנדס
        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(EngineerLogInWindow), new PropertyMetadata(0));



        private void bcLogIn(object sender, RoutedEventArgs e)
        {
            try//נסגור את החלון כניסה
            {
                s_bl.Engineer.Read(Id);//יחזיר שגיאה במידה וריק
                this.Close();
                new EngineerWindow(Id).ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
