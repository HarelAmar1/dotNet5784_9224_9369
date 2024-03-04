using PL.Engineer;
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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for MiniEngineerWindow.xaml
    /// </summary>
    public partial class MiniEngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public MiniEngineerWindow(int id)
        {
            InitializeComponent();
            if (id != -1)//אם קיים מהנדס אז תכניס אותו
                MiniEngineer = s_bl.Engineer.Read(id);
            else
                MiniEngineer = new BO.Engineer() { Id = 0 };
        }
        public BO.Engineer MiniEngineer
        {
            get { return (BO.Engineer)GetValue(MiniEngineerWindowProperty); }
            set { SetValue(MiniEngineerWindowProperty, value); }
        }
        public static readonly DependencyProperty MiniEngineerWindowProperty =
            DependencyProperty.Register("MiniEngineer", typeof(BO.Engineer), typeof(MiniEngineerWindow), new PropertyMetadata(null));
    }
}
