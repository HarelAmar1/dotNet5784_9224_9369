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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList = s_bl?.Engineer.ReadAll()!;
        }

        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.All;
        private void cbEngineerDataFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = (Experience == BO.EngineerExperience.All) ?
            s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => (int)item.level == (int)Experience)!;
        }

        private void bcPreesToAdd(object sender, RoutedEventArgs e)
        {
            new EngineerForManagerWindow().Show();
            Close();
        }

        private void bcPreesToUpdate(object sender, MouseButtonEventArgs e)
        {
            BO.Engineer selectedEngineer = (BO.Engineer)((ListView)sender).SelectedItem;
            int selectedId = selectedEngineer.Id;
            new EngineerForManagerWindow(selectedId).Show();
            Close();
        }
    }
}