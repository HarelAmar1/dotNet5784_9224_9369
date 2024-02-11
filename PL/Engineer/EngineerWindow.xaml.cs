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
        public EngineerWindow(int windowId = 0)
        {
            

            InitializeComponent();
            if (windowId == 0)
            {
                s_bl.Engineer.Create(new BO.Engineer());
            }
            else
            {
                BO.Engineer engineer = new BO.Engineer() { }
            }

        }

    }
}