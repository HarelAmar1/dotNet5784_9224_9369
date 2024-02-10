using PL.Engineer;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnEngineer_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }

        private void InitDB(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxButton buttons = MessageBoxButton.OKCancel;
            MessageBoxResult a = MessageBox.Show("Would you like to create Initial data?", "cho", buttons);
            if (a == MessageBoxResult.OK)
            {
                DalTest.Initialization.deleteXMLFile();
                DalTest.Initialization.Do();
            }
        }
    }
}