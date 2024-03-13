using BO;
using DO;
using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        //Task Property
        public BO.Task Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }
        public static readonly DependencyProperty TaskProperty =
        DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));


        //TaskInList Property
        public IEnumerable<TaskInList> TaskInList
        {
            get { return (IEnumerable<TaskInList>)GetValue(TaskInListProperty); }
            set { SetValue(TaskInListProperty, value); }
        }
        // Using a DependencyProperty as the backing store for taskInList.  and init with TaskInList from BL
        public static readonly DependencyProperty TaskInListProperty =
            DependencyProperty.Register("TaskInList", typeof(IEnumerable<TaskInList>), typeof(TaskWindow), new PropertyMetadata(s_bl.Task.ReadAll()));
        //פונקציות שמכניסות לרשימה את המשימות התליות
        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null && checkBox.Tag is int id && SelectedIds.Contains(id))
            {
                checkBox.IsChecked = true;
            }
        }
        public ObservableCollection<int> SelectedIds { get; set; }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null && checkBox.Tag is int id)
            {
                if (!SelectedIds.Contains(id))
                    SelectedIds.Add(id);
            }
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null && checkBox.Tag is int id)
            {
                SelectedIds.Remove(id);
            }
        }


        //Engineer Property
        public IEnumerable<BO.EngineerInTask> EngineerInTask
        {
            get { return (IEnumerable<BO.EngineerInTask>)GetValue(EngineerInTaskProperty); }
            set { SetValue(EngineerInTaskProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Engineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerInTaskProperty =
            DependencyProperty.Register("EngineerInTask", typeof(IEnumerable<BO.EngineerInTask>), typeof(TaskWindow), new PropertyMetadata(null));




        public TaskWindow(int id = 0)
        {
            InitializeComponent();
            //אתחול המהנדס
            EngineerInTask = s_bl.Engineer.ReadAll().Select(engineer => new EngineerInTask { Id = engineer.Id, Name = engineer.Name });
            var engineersInTask = EngineerInTask.ToList();
            engineersInTask.Insert(0, new EngineerInTask { Id = -1, Name = "None" });
            EngineerInTask = engineersInTask;



            //אתחול הרשימה של התלויות
            SelectedIds = new ObservableCollection<int>();

            if (id != 0)//for update 
            {
                //אתחול רשימת התלויות
                Task = s_bl.Task.Read(id);
                if (Task.Dependencies.Count != 0)
                    SelectedIds = new ObservableCollection<int>(Task.Dependencies.Select(d => d.Id));

            }
            else//for add
            {
                Task = new BO.Task()
                {
                    Id = 0,
                    Description = "Description",
                    Alias = "Alias",
                    Status = 0,
                    CreatedAtDate = s_bl.Clock,
                    RequiredEffortTime = new TimeSpan(0, 0, 0, 0),
                    StartDate = s_bl.Clock,
                    Copmlexity = 0,
                    Deliverables = "Deliverables",
                    Remarks = "Remarks"
                };
            }
            if (Task.Engineer == null)//אם המהנדס נאל אז נאתחל אותו בערך ברירת מחדל שהוספנו
                Task.Engineer = EngineerInTask.ToList().FirstOrDefault();

        }

        private void bcAdd(object sender, RoutedEventArgs e)
        {
            //אתחול הרשימה של התלויות
            List<TaskInList> tempDepend = new List<TaskInList>();
            foreach (var id in SelectedIds)
            {
                BO.Task taskOfDependency = s_bl.Task.Read(id);
                tempDepend.Add(new BO.TaskInList()
                {
                    Id = taskOfDependency.Id,
                    Description = taskOfDependency.Description,
                    Alias = taskOfDependency.Alias,
                    Status = taskOfDependency.Status
                });
            }//End Init

            //אתחול מהנדס
            if (Task.Engineer.Id == -1)
                Task.Engineer = null;

            BO.Task task = new BO.Task()
            {
                Id = Task.Id,
                Description = Task.Description,
                Alias = Task.Alias,
                Status = Task.Status,
                CreatedAtDate = Task.CreatedAtDate,
                RequiredEffortTime = Task.RequiredEffortTime,
                Dependencies = tempDepend,
                Engineer = Task.Engineer,
                Copmlexity = Task.Copmlexity,
                Deliverables = Task.Deliverables,
                Remarks = Task.Remarks
            };
            try
            {
                s_bl.Task.Create(task);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } // Exception handling
            Close();
            new TaskForListWindow().Show();
        }

        private void bcUpdate(object sender, RoutedEventArgs e)
        {
            //אתחול הרשימה של התלויות
            List<TaskInList> tempDepend = new List<TaskInList>();
            foreach (var id in SelectedIds)
            {
                BO.Task taskOfDependency = s_bl.Task.Read(id);
                tempDepend.Add(new BO.TaskInList()
                {
                    Id = taskOfDependency.Id,
                    Description = taskOfDependency.Description,
                    Alias = taskOfDependency.Alias,
                    Status = taskOfDependency.Status
                });
            }

            BO.Task task = new BO.Task()
            {
                Id = Task.Id,
                Description = Task.Description,
                Alias = Task.Alias,
                Status = Task.Status,
                CreatedAtDate = Task.CreatedAtDate,
                RequiredEffortTime = Task.RequiredEffortTime,
                Dependencies = tempDepend,
                Engineer = Task.Engineer,
                Copmlexity = Task.Copmlexity,
                Deliverables = Task.Deliverables,
                Remarks = Task.Remarks
            };
            try
            {
                s_bl.Task.Update(task);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } // Exception handling
            Close();
            new TaskForListWindow().Show();
        }

        //לבדוק איזה מהנדס המשתמש בחר
        private void ComboBoxEngineer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedEngineer = (sender as ComboBox).SelectedItem as EngineerInTask;
            if (selectedEngineer.Id != -1)
                Task.Engineer = selectedEngineer;
            else
                Task.Engineer = null;
        }
        private void ComboBoxEngineer_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (Task.Engineer.Id != -1)
                comboBox.SelectedItem = EngineerInTask.FirstOrDefault(e => e.Id == Task.Engineer.Id);
            else
                comboBox.SelectedItem = Task.Engineer;
        }

        private MiniEngineerWindow miniEngineerWindow; // שדה לשמירת החלון החדש
        private void OpenNewWindowButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Tag is int engineerId)
                {
                    miniEngineerWindow = new MiniEngineerWindow(engineerId); // יצירת חלון חדש אם אינו קיים או סגור
                    miniEngineerWindow.Show(); // הצגת החלון
                }
            }
        }

        private void OpenNewWindowButton_MouseLeave(object sender, MouseEventArgs e)
        {
            miniEngineerWindow.Close(); // סגירת החלון
            miniEngineerWindow = null; // איפוס ההפניה לחלון
        }
    }
}
