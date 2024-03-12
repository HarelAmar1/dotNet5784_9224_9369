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
using PL.Engineer;

namespace PL.EngineerForEngineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        //Engineer Property
        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }
        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));


        //Task Property
        public List<BO.TaskInEngineer> TaskInEngineer
        {
            get { return (List<BO.TaskInEngineer>)GetValue(TaskInEngineerProperty); }
            set { SetValue(TaskInEngineerProperty, value); }
        }
        public static readonly DependencyProperty TaskInEngineerProperty =
            DependencyProperty.Register("TaskInEngineer", typeof(List<BO.TaskInEngineer>), typeof(EngineerWindow), new PropertyMetadata(null));


        //Date Property
        public DateTime CompleteDate
        {
            get { return (DateTime)GetValue(CompleteDateProperty); }
            set { SetValue(CompleteDateProperty, value); }
        }
        public static readonly DependencyProperty CompleteDateProperty =
            DependencyProperty.Register("CompleteDate", typeof(DateTime), typeof(EngineerWindow), new PropertyMetadata(s_bl.Clock));

        //Ctor
        public EngineerWindow(int id)
        {
            InitializeComponent();
            Engineer = s_bl.Engineer.Read(id);//אתחול המהנדס
            if (Engineer.Task == null)
                Engineer.Task = new BO.TaskInEngineer(-1, "");//אם אין משימה נכניס ערך זמני כך שנדע שאין משימה

            TaskInEngineer = new List<TaskInEngineer>();//אתחול הרשימה
            //אתחול רשימת המשימות האפשריות למהנדס
            foreach (var task in s_bl.Task.ReadAll())
            {
                BO.Task fullTask = s_bl.Task.Read(task.Id);
                bool done = true;
                //נבדוק שהמשימות התלויות לא הסתיימו
                foreach (var depend in fullTask.Dependencies)
                {
                    if (depend.Status != Status.Done)
                    {
                        done = false;
                        break;
                    }
                }
                //בדיקת כל התנאים שהמשימה צריכה לקיים
                if (fullTask.Engineer == null && done == true && fullTask.Status != (Status)4 && fullTask.Copmlexity <= s_bl.Engineer.Read(id).Level)
                    TaskInEngineer.Add(new BO.TaskInEngineer(task.Id, task.Alias));
            }
        }

        private void ComboBoxTask_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (Engineer.Task.Id != -1)//אם הוגדר משימה למהנדס ולא קיים שם הערך הזמני ששמנו שם
            {
                //נציב בתצוגה את המשימה הנוכחית שהמשתמש עובד עליה
                TaskInEngineer.Add(Engineer.Task);
                comboBox.SelectedItem = TaskInEngineer.FirstOrDefault(e => e.Id == Engineer.Task.Id);
            }
        }
        private void ComboBoxTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //המשתמש הגדיר משימה למהנדס ולכן נציב אותה במהנדס את המשימה
            var selectedTask = (sender as ComboBox).SelectedItem as TaskInEngineer;
            Engineer.Task = selectedTask;
        }

        private void bcStartTask(object sender, RoutedEventArgs e)
        {
            //במידה ולא הוגדר משימה נודיע למשתמש
            if (Engineer.Task.Id == -1)
            {
                MessageBox.Show("Select a task or click Cancel");
                return;
            }
            //המהנדס התחיל את המשימה לכן נכניס תאריך התחלה למשימה בשכבת הנתונים
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
                StartDate = s_bl.Clock, //New StartDate
                ScheduledDate = oldTask.ScheduledDate,
                ForecastDate = oldTask?.ForecastDate,
                DeadlineDate = oldTask?.DeadlineDate,
                CompleteDate = oldTask.CompleteDate,
                Deliverables = oldTask?.Deliverables,
                Remarks = oldTask?.Remarks,
                Engineer = oldTask?.Engineer,
                Copmlexity = oldTask?.Copmlexity
            };
            s_bl.Task.Update(newTask);//מעדכנים את התאריך סיום

            //נעדכן את המהנדס לשכבת נתונים כי המשימה שלו היא משימה חדשה
            s_bl.Engineer.Update(Engineer);
            
            Close();
        }

        private void bcDoneTask(object sender, RoutedEventArgs e)
        {
            //המהנדס סיים את המשימה לכן נכניס תאריך סיום למשימה בשכבת הנתונים
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
                CompleteDate = CompleteDate, //New CompleteDate
                Deliverables = oldTask?.Deliverables,
                Remarks = oldTask?.Remarks,
                Engineer = oldTask?.Engineer,
                Copmlexity = oldTask?.Copmlexity
            };
            s_bl.Task.Update(newTask);//מעדכנים את התאריך סיום

            //המהנדס סיים עם המשימה לכן נגדיר לו שאין לו משימה לבצע
            Engineer.Task = null;
            s_bl.Engineer.Update(Engineer);//לבדוק למה לא מתעדכן המשימה
            var a = s_bl.Engineer.Read(Engineer.Id);

            
            Close();
        }

        private void bcCancel(object sender, RoutedEventArgs e)
        {
            Close();

        }


    }
}
