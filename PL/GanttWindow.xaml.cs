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

        //רשימת המשימות עם תאריכי התחלה ומשך זמן
        public List<BO.Task> ListOfTask = new List<BO.Task>();

        public GanttWindow()
        {
            //לבדקו שיש תאריך התחלה של פרויקט
            if (s_bl.Schedule.getStartDateOfProject() == null)
                throw new Exception("PROJECT START DATE REQUIRED.");

            if (s_bl.Task == null || s_bl.Task.ReadAll().Count() == 0) 
                throw new Exception("There is no Data");


            InitializeComponent();
            //ניצור רשימה של המשימות עם התאריך התחלה
            foreach (var task in s_bl.Task.ReadAll())
            {
                var taskFromDal = s_bl.Task.Read(task.Id);
                //var stringOfDay = taskFromDal.RequiredEffortTime.ToString();
                BO.Task newTaskForGantt = new BO.Task { Id = taskFromDal.Id, Alias = taskFromDal.Alias, RequiredEffortTime = taskFromDal.RequiredEffortTime, ScheduledDate = taskFromDal.ScheduledDate, CompleteDate = taskFromDal.CompleteDate};
                ListOfTask.Add(newTaskForGantt);
            }
            //נמיין את הרשימה
            ListOfTask = ListOfTask.OrderBy(task => task.ScheduledDate).ToList();

            this.Loaded += Window_Loaded;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Canvas canvas = FindVisualChild<Canvas>(this);
            if (canvas == null) return;
            DateTime minStartDate = ListOfTask.Min(task => task.ScheduledDate ?? DateTime.MaxValue);
            DateTime maxEndDate = ListOfTask.Max(task => (task.ScheduledDate ?? DateTime.MinValue).AddDays(task.RequiredEffortTime.Value.Days));
            double maxAliasWidth = GetMaxAliasWidth() + 40; // קביעת הרוחב המקסימלי של הכינויים + שם התעודת זהות

            double maxWidth = 40 + ListOfTask.Max(task => ((task.ScheduledDate ?? DateTime.Today) - minStartDate).TotalDays * 10 + task.RequiredEffortTime.Value.Days * 10) + maxAliasWidth + 20; // הוספת רווח קצת בסוף
            canvas.Width = maxWidth;
            canvas.Height = ListOfTask.Count * 30 + 60; // הוספת רווח לסרגל התאריכים ולמשימות

            double topPosition = 40; // מתחילים מ-40 פיקסלים למעלה כדי לתת מקום לסרגל התאריכים

            foreach (var task in ListOfTask)
            {
                double offsetDays = ((task.ScheduledDate ?? DateTime.Today) - minStartDate).TotalDays;
                double leftPosition = offsetDays * 10 + maxAliasWidth; // המלבנים מתחילים לאחר הטקסט הארוך ביותר

                // הוספת תווית של שם המשימה
                TextBlock aliasLabel = new TextBlock
                {
                    Text = $"ID: {task.Id}, {task.Alias}",
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold
                };
                canvas.Children.Add(aliasLabel);
                Canvas.SetLeft(aliasLabel, 5); // קצת רווח מהשוליים השמאליים
                Canvas.SetTop(aliasLabel, topPosition);


                //צבע המלבן תלוי במשימה שמתעכבת או שהסתיימה

                //כחול כהה - משימה שטרם בוצעה
                SolidColorBrush rectangleColor = new SolidColorBrush(Color.FromArgb(0xFF, 0x3F, 0x5B, 0x77));
                //ירוק כהה - משימה שבוצעה
                if (task.CompleteDate != null)
                    rectangleColor = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x8B, 0x00));
                //אדום כהה - משימה שמתעכבת (מהנדס לא דיווח על סיום בזמן) ה
                if (task.CompleteDate == null && task.ScheduledDate + task.RequiredEffortTime < s_bl.Clock || task.CompleteDate != null && task.CompleteDate > s_bl.Clock) 
                    rectangleColor = new SolidColorBrush(Color.FromArgb(0xFF, 0x8B, 0x00, 0x00));
                Rectangle rectangle = new Rectangle
                {
                    Fill = rectangleColor,
                    Width = task.RequiredEffortTime.Value.Days * 10, // כפל המשך המשימה ב-10 לדוגמה
                    Height = 20,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 1,
                    RadiusX = 5, // רדיוס העיגול של הקצוות בציר X
                    RadiusY = 5 // רדיוס העיגול של הקצוות בציר Y
                };

                canvas.Children.Add(rectangle);
                Canvas.SetLeft(rectangle, leftPosition);
                Canvas.SetTop(rectangle, topPosition);

                // הוספת תווית תאריך ומשך זמן למלבן
                TextBlock dateLabel = new TextBlock
                {
                    Text = $"ID: {task.Id}, {task.ScheduledDate?.ToString("dd/MM")} + {task.RequiredEffortTime.Value.Days}",
                    Foreground = new SolidColorBrush(Colors.White),
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center
                };
                canvas.Children.Add(dateLabel);
                Canvas.SetLeft(dateLabel, leftPosition + 2); // כוונון עדין למיקום התווית
                Canvas.SetTop(dateLabel, topPosition + 2); // כוונון עדין למיקום התווית

                topPosition += 30; // עדכון המיקום האנכי למשימה הבאה
            }

            // הוספת סרגל התאריכים
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
            return maxAliasWidth + 10; // נוסף רווח של 10 פיקסלים
        }


        private void AddMonthLabels(Canvas canvas, DateTime minStartDate, DateTime maxEndDate)
        {
            double pixelsPerDay = 10; // נניח שכל יום מיוצג על ידי 10 פיקסלים

            // חישוב הרוחב המקסימלי של הכינויים
            double maxAliasWidth = GetMaxAliasWidth();

            DateTime currentMonth = new DateTime(minStartDate.Year, minStartDate.Month, 1);
            while (currentMonth <= maxEndDate)
            {
                DateTime nextMonth = currentMonth.AddMonths(1);
                bool isMonthStartVisible = ListOfTask.Any(task => task.ScheduledDate.HasValue && task.ScheduledDate.Value.Month == currentMonth.Month && task.ScheduledDate.Value.Year == currentMonth.Year);

                double leftPosition;
                if (isMonthStartVisible)
                {
                    // אם יש מלבן שמתחיל בחודש זה, בדוק את המיקום המדויק של התחלת המלבן
                    DateTime firstTaskStart = ListOfTask.Where(task => task.ScheduledDate.HasValue && task.ScheduledDate.Value.Month == currentMonth.Month && task.ScheduledDate.Value.Year == currentMonth.Year)
                                                         .Min(task => task.ScheduledDate.Value);

                    if (firstTaskStart.Day > 1)
                    {
                        // אם המלבן הראשון מתחיל אחרי ה-1 בחודש, מקם את תווית החודש משמאל למלבן
                        leftPosition = ((firstTaskStart - minStartDate).TotalDays - firstTaskStart.Day + 1) * pixelsPerDay + maxAliasWidth;
                    }
                    else
                    {
                        // אם המלבן מתחיל ב-1 בחודש, מקם את תווית החודש בהתאם למיקום הסטנדרטי
                        leftPosition = ((currentMonth - minStartDate).TotalDays) * pixelsPerDay + maxAliasWidth;
                    }
                }
                else
                {
                    // אם אין מלבן שמתחיל בחודש זה, מקם את תווית החודש בהתאם למיקום הסטנדרטי
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
                Canvas.SetTop(monthLabel, 0); // אתה יכול להתאים את הגובה כמו שאתה רוצה

                canvas.Children.Add(monthLabel);

                currentMonth = nextMonth;
            }
        }


        // מתודה עזר לחיפוש רכיב בעץ הוויזואלי לפי סוג
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    var childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}