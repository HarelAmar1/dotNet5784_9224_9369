﻿using PL.Task;
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
        public DateTime? CompleteDate
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
            CompleteDate=s_bl.Clock;
            Engineer = s_bl.Engineer.Read(id);//initialize the engineer
            if (Engineer.Task == null)
                Engineer.Task = new BO.TaskInEngineer(-1, "");//If there is no task we will insert a temporary value so that we know there is no task

            TaskInEngineer = new List<TaskInEngineer>();//Initialize the list

            //Initialize the list of possible tasks for the engineer
            foreach (var task in s_bl.Task.ReadAll())
            {
                BO.Task fullTask = s_bl.Task.Read(task.Id);
                bool done = true;
                //we will check that the dependent tasks have not finished
                foreach (var depend in fullTask.Dependencies)
                {
                    if (depend.Status != Status.Done)
                    {
                        done = false;
                        break;
                    }
                }
                //Checking all the conditions that the task needs to meet
                if (fullTask.Engineer == null && done == true && fullTask.Status != (Status)4 && fullTask.Copmlexity <= s_bl.Engineer.Read(id).Level)
                    TaskInEngineer.Add(new BO.TaskInEngineer(task.Id, task.Alias));
            }
        }

        private void ComboBoxTask_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (Engineer.Task.Id != -1) //If a task was defined for an engineer and the name of the temporary value we put there does not exist
            {
                //Display the current task the user is working on
                TaskInEngineer.Add(Engineer.Task);
                comboBox.SelectedItem = TaskInEngineer.FirstOrDefault(e => e.Id == Engineer.Task.Id);
            }
        }
        private void ComboBoxTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //The user defined a task for the engineer so we will place it in the engineer the task
            var selectedTask = (sender as ComboBox).SelectedItem as TaskInEngineer;
            Engineer.Task = selectedTask;
        }

        private void bcStartTask(object sender, RoutedEventArgs e)
        {
            //If no task has been defined, we will notify the user
            if (Engineer.Task.Id == -1)
            {
                MessageBox.Show("Select a task or click Cancel");
                return;
            }
            //The engineer started the task so we will enter a start date for the task in the data layer
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
            s_bl.Task.Update(newTask);//Update the end date

            //We will update the data layer engineer because his task is a new task
            s_bl.Engineer.Update(Engineer);

            Close();
        }

        private void bcDoneTask(object sender, RoutedEventArgs e)
        {
            //The engineer finished the task so we will enter an end date for the task in the data layer
            BO.Task oldTask = s_bl.Task.Read(Engineer.Task.Id);
            BO.Task newTask = new BO.Task()
            {
                Id = oldTask.Id,
                Description = oldTask.Description,
                Alias = oldTask.Alias,
                CreatedAtDate = oldTask.CreatedAtDate,
                Status = BO.Status.Done,
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
            s_bl.Task.Update(newTask);//Update the end date

            //The engineer is done with the task so we will define that he has no task to perform
            Engineer.Task = null;
            s_bl.Engineer.Update(Engineer);//Check why the task is not updated
            var a = s_bl.Engineer.Read(Engineer.Id);


            Close();
        }

        private void bcCancel(object sender, RoutedEventArgs e)
        {
            Close();

        }


    }
}
