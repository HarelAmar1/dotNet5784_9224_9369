using PL.Task;
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
using BO;

namespace PL.EngineerForEngineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        //מהנדס
        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Engineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));


        //Task Property
        public List<BO.TaskInEngineer> TaskInEngineer
        {
            get { return (List<BO.TaskInEngineer>)GetValue(TaskInEngineerProperty); }
            set { SetValue(TaskInEngineerProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Engineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskInEngineerProperty =
            DependencyProperty.Register("TaskInEngineer", typeof(List<BO.TaskInEngineer>), typeof(EngineerWindow), new PropertyMetadata(null));


        //Date Property
        public DateTime CompleteDate
        {
            get { return (DateTime)GetValue(CompleteDateProperty); }
            set { SetValue(CompleteDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CompleteDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CompleteDateProperty =
            DependencyProperty.Register("CompleteDate", typeof(DateTime), typeof(EngineerWindow), new PropertyMetadata(DateTime.Now));

        //Ctor
        public EngineerWindow(int id)
        {
            InitializeComponent();
            Engineer = s_bl.Engineer.Read(id);//אתחול המהנדס
            TaskInEngineer = new List<TaskInEngineer>();//אתחול הרשימה
            //אתחול רשימת המשימות האפשריות למהנדס
            foreach (var task in s_bl.Task.ReadAll())
            {
                BO.Task fullTask = s_bl.Task.Read(task.Id);
                bool done = true;
                foreach (var depend in fullTask.Dependencies)
                {
                    if (depend.Status != Status.Done)
                    {
                        done = false;
                        break;
                    }
                }
                //בדיקת כל התנאים שהמשימה צריכה לקיים
                if (fullTask.Engineer == null && done == true && fullTask.Copmlexity <= s_bl.Engineer.Read(id).Level)
                    TaskInEngineer.Add(new BO.TaskInEngineer(task.Id, task.Alias));
            }
            //להכניס לאופציות אפשרות ריקה
            TaskInEngineer.Insert(0, new TaskInEngineer(-1, "None"));
            if (Engineer.Task == null)//אם המהנדס נאל אז נאתחל אותו בערך ברירת מחדל שהוספנו
                Engineer.Task = TaskInEngineer.FirstOrDefault();

        }

        private void ComboBoxTask_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (Engineer.Task != null)
            {
                //המשימה הנוכחית לא נכנסה כי המהנדס עבד עליה ולכן נוסיף אותה כעת
                TaskInEngineer.Add(Engineer.Task);
                comboBox.SelectedItem = TaskInEngineer.FirstOrDefault(e => e.Id == Engineer.Task.Id);
            }
            else
                comboBox.SelectedItem = Engineer.Task;
        }

        private void ComboBoxTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTask = (sender as ComboBox).SelectedItem as TaskInEngineer;
            if (selectedTask.Id != -1)
                Engineer.Task = selectedTask;
            else
                Engineer.Task = null;
        }




        private void bcUpdate(object sender, RoutedEventArgs e)
        {

            //לבדוק שהוגדר משימה למהנדס אחרת לא נעדכן את התאריך סיום שלה
            if (Engineer.Task != null)
            {
                BO.Task oldTask = s_bl.Task.Read(Engineer.Task.Id);
                BO.Task newTask = new BO.Task()
                {
                    Id = oldTask.Id,
                    Description = oldTask.Description,
                    Alias = oldTask.Alias,
                    CreatedAtDate = oldTask.CreatedAtDate,
                    Status = oldTask.Status,
                    Dependencies = oldTask.Dependencies,
                    RequiredEffortTime = oldTask.RequiredEffortTime,
                    StartDate = oldTask.StartDate,
                    ScheduledDate = oldTask.ScheduledDate,
                    ForecastDate = oldTask?.ForecastDate,
                    DeadlineDate = oldTask?.DeadlineDate,
                    CompleteDate = CompleteDate,
                    Deliverables = oldTask?.Deliverables,
                    Remarks = oldTask?.Remarks,
                    Engineer = oldTask?.Engineer,
                    Copmlexity = oldTask?.Copmlexity
                };
                s_bl.Task.Update(newTask);//מעדכנים את התאריך סיום
            }
            //נעדכן את המהנדס למשימה החדשה
            s_bl.Engineer.Update(Engineer);
            
            
        }
    }
}
