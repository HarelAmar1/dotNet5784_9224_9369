using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PL
{
    /// <summary>
    /// Interaction logic for GanttWindow.xaml
    /// </summary>
    public partial class GanttWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        //List of tasks with start dates and duration
        public List<BO.Task> ListOfTask = new List<BO.Task>();

        public GanttWindow()
        {
            InitializeComponent();

            try
            {

                // Create a list of tasks with a start date
                foreach (var task in s_bl.Task.ReadAll())
                {
                    var taskFromDal = s_bl.Task.Read(task.Id);
                    BO.Task newTaskForGantt = new BO.Task { Id = taskFromDal.Id, Alias = taskFromDal.Alias, RequiredEffortTime = taskFromDal.RequiredEffortTime, ScheduledDate = taskFromDal.ScheduledDate, CompleteDate = taskFromDal.CompleteDate };
                    ListOfTask.Add(newTaskForGantt);
                }
                //sort the list
                ListOfTask = ListOfTask.OrderBy(task => task.ScheduledDate).ToList();

                this.Loaded += Window_Loaded;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Canvas canvas = FindVisualChild(this);
            if (canvas == null) return;
            DateTime minStartDate = ListOfTask.Min(task => task.ScheduledDate ?? DateTime.MaxValue);
            DateTime maxEndDate = ListOfTask.Max(task => (task.ScheduledDate ?? DateTime.MinValue).AddDays(task.RequiredEffortTime.Value.Days));
            double maxAliasWidth = GetMaxAliasWidth() + 40; // Determining the maximum width of the aliases + name of the identity certificate

            double maxWidth = 40 + ListOfTask.Max(task => ((task.ScheduledDate ?? DateTime.Today) - minStartDate).TotalDays * 10 + task.RequiredEffortTime.Value.Days * 10) + maxAliasWidth + 20; // Added a little space at the end
            canvas.Width = maxWidth;
            canvas.Height = ListOfTask.Count * 30 + 60; // Adding a space to the date bar and tasks

            double topPosition = 40; // start from 40px up to make room for the date bar

          

            foreach (var task in ListOfTask)
            {
                //Finding the dependent tasks
                string dependTask = "dependent tasks: ";
                foreach (var DTask in s_bl.Task.Read(task.Id).Dependencies) 
                {
                    dependTask += $"{DTask.Id},";
                }
                dependTask = dependTask.Substring(0, dependTask.Length - 1);//We will delete the last character because of the comma



                double offsetDays = ((task.ScheduledDate ?? DateTime.Today) - minStartDate).TotalDays;
                double leftPosition = offsetDays * 10 + maxAliasWidth; // The rectangles start after the longest text

                // Adding a label of the task name
                TextBlock aliasLabel = new TextBlock
                {
                    Text = $"ID: {task.Id}, {task.Alias}",
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold
                };
                canvas.Children.Add(aliasLabel);
                Canvas.SetLeft(aliasLabel, 5); // some space from the left margin
                Canvas.SetTop(aliasLabel, topPosition);


                //The color of the rectangle depends on the task that is delayed or completed

                //dark blue - task not yet done
                SolidColorBrush rectangleColor = new SolidColorBrush(Color.FromArgb(0xFF, 0x3F, 0x5B, 0x77));
                //dark green - task done
                if (task.CompleteDate != null)
                    rectangleColor = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x8B, 0x00));
                //dark red - a task that is delayed (the engineer did not report completion on time)
                if (task.CompleteDate == null && task.ScheduledDate + task.RequiredEffortTime < s_bl.Clock || task.CompleteDate != null && task.CompleteDate > s_bl.Clock)
                    rectangleColor = new SolidColorBrush(Color.FromArgb(0xFF, 0x8B, 0x00, 0x00));
                Rectangle rectangle = new Rectangle
                {
                    Fill = rectangleColor,
                    Width = task.RequiredEffortTime.Value.Days * 10, // Multiply the continuation of the task by 10 for example
                    Height = 20,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 1,
                    RadiusX = 5,// The circle radius of the edges in the X axis
                    RadiusY = 5 // The circle radius of the edges in the Y axis
                };


                // Adding Tooltip
                ToolTip tooltip = new ToolTip();
                tooltip.Content = dependTask; // The content of the tooltip
                tooltip.Background = Brushes.LightBlue; // Background
                tooltip.BorderBrush = Brushes.Black; // Border color
                tooltip.BorderThickness = new Thickness(1); // Border thickness
                tooltip.Foreground = Brushes.Black; // Text color
                tooltip.FontSize = 12; // Font size
                tooltip.FontFamily = new FontFamily("Arial"); // Font family
                tooltip.Padding = new Thickness(5); // Padding within the tooltips

                ToolTipService.SetToolTip(rectangle, tooltip); // Linking the tooltip to the shape


                canvas.Children.Add(rectangle);
                Canvas.SetLeft(rectangle, leftPosition);
                Canvas.SetTop(rectangle, topPosition);

                // Adding a date and duration label to the rectangle
                TextBlock dateLabel = new TextBlock
                {
                    Text = $" {task.ScheduledDate?.ToString("dd/MM")} + {task.RequiredEffortTime.Value.Days} D",
                    Foreground = new SolidColorBrush(Colors.White),
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center
                };
                canvas.Children.Add(dateLabel);
                Canvas.SetLeft(dateLabel, leftPosition + 2);// fine-tuning for label position
                Canvas.SetTop(dateLabel, topPosition + 2);  // fine-tuning for label position 
                topPosition += 30; // Update the vertical position for the next task
            }

            // Adding the date bar
            AddMonthLabels(canvas, minStartDate, maxEndDate);
        }

        private double GetMaxAliasWidth()
        {
            double maxAliasWidth = 0;
            foreach (var task in ListOfTask)
            {
                TextBlock tempTextBlock = new TextBlock
                {
                    Text = task.Alias,
                    FontWeight = FontWeights.Bold
                };
                tempTextBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                double width = tempTextBlock.DesiredSize.Width;
                if (width > maxAliasWidth)
                {
                    maxAliasWidth = width;
                }
            }
            return maxAliasWidth + 10; // Added 10px margin
        }


        private void AddMonthLabels(Canvas canvas, DateTime minStartDate, DateTime maxEndDate)
        {
            double pixelsPerDay = 10; // Assume each day is represented by 10 pixels

            // Calculation of the maximum width of the aliases
            double maxAliasWidth = GetMaxAliasWidth();

            DateTime currentMonth = new DateTime(minStartDate.Year, minStartDate.Month, 1);
            while (currentMonth <= maxEndDate)
            {
                DateTime nextMonth = currentMonth.AddMonths(1);
                bool isMonthStartVisible = ListOfTask.Any(task => task.ScheduledDate.HasValue && task.ScheduledDate.Value.Month == currentMonth.Month && task.ScheduledDate.Value.Year == currentMonth.Year);

                double leftPosition;
                if (isMonthStartVisible)
                {
                    // If there is a rectangle starting in this month, check the exact location of the start of the rectangle
                    DateTime firstTaskStart = ListOfTask.Where(task => task.ScheduledDate.HasValue && task.ScheduledDate.Value.Month == currentMonth.Month && task.ScheduledDate.Value.Year == currentMonth.Year)
                                                         .Min(task => task.ScheduledDate.Value);

                    if (firstTaskStart.Day > 1)
                    {
                        // If the first rectangle starts after the 1st of the month, place the month label to the left of the rectangle
                        leftPosition = ((firstTaskStart - minStartDate).TotalDays - firstTaskStart.Day + 1) * pixelsPerDay + maxAliasWidth;
                    }
                    else
                    {
                        // If the rectangle starts with the 1st of the month, position the month label according to the standard position
                        leftPosition = ((currentMonth - minStartDate).TotalDays) * pixelsPerDay + maxAliasWidth;
                    }
                }
                else
                {
                    // If there is no rectangle starting with this month, position the month label according to the standard position
                    leftPosition = ((currentMonth - minStartDate).TotalDays) * pixelsPerDay + maxAliasWidth;
                }

                TextBlock monthLabel = new TextBlock
                {
                    Text = currentMonth.ToString("MMM yyyy"),
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.White
                };

                Canvas.SetLeft(monthLabel, leftPosition);
                Canvas.SetTop(monthLabel, 0); // You can adjust the height as you want

                canvas.Children.Add(monthLabel);

                currentMonth = nextMonth;
            }
        }


        // A helper method to search for a component in the visual tree by Canvas type
        private Canvas FindVisualChild(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is Canvas)
                    return (Canvas)child;
                else
                {
                    var childOfChild = FindVisualChild(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}