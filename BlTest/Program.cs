using BO;
using BlApi;
using DalApi;
using BlImplementation;
using System.Threading.Channels;

namespace BlTest;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static void Main(string[] args)
    {
        //Data Initialization
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
            DalTest.Initialization.Do();


        // Get user input from the main menu
        int userInput = menu();

        while (userInput != 0)
        {
            switch (userInput)
            {
                case 1: // Task operations
                    int OpForTask = optionsForTask(); // Display Task options
                    while (OpForTask != 0)
                    {
                        switch (OpForTask)
                        {
                            case 1:
                                createBoTask();
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                            case 4:
                                break;
                            case 5:
                                break;
                            case 6:
                                break;
                        }
                        OpForTask = optionsForTask(); // Show Task options again
                    }//End While
                    break;
                case 2:
                    int OpForEngineer = optionsForEngineer(); // Display Task options
                    while (OpForEngineer != 0)
                    {
                        switch (OpForEngineer)
                        {
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                            case 4:
                                break;
                            case 5:
                                break;
                        }
                        OpForEngineer = optionsForEngineer(); // Show Task options again
                    }//End While
                    break;
            }
        }
    }

    // Menu function to display the main menu and capture user's choice
    public static int menu()
    {
        Console.WriteLine("Choose from next list");
        Console.WriteLine("1 - Choose Task");
        Console.WriteLine("2 - Choose Engineer");
        Console.WriteLine("0 - exit");
        int firstMenu = int.Parse(Console.ReadLine()!);
        return firstMenu;
    }
    public static int optionsForTask()
    {
        // Displaying options for CRUD operations
        Console.WriteLine("Your Options");
        Console.WriteLine("1 - Create");
        Console.WriteLine("2 - Read");
        Console.WriteLine("3 - Read All");
        Console.WriteLine("4 - Update");
        Console.WriteLine("5 - Delete");
        Console.WriteLine("6 - Date Time Management");
        Console.WriteLine("0 - Back");
        int op = int.Parse(Console.ReadLine()!);
        return op;
    }
    public static int optionsForEngineer()
    {
        // Displaying options for CRUD operations
        Console.WriteLine("Your Options");
        Console.WriteLine("1 - Create");
        Console.WriteLine("2 - Read");
        Console.WriteLine("3 - Read All");
        Console.WriteLine("4 - Update");
        Console.WriteLine("5 - Delete");
        Console.WriteLine("0 - Back");
        int op = int.Parse(Console.ReadLine()!);
        return op;
    }

    public static void createBoTask()
    {
        Console.WriteLine("Please enter a Description");
        string description = Console.ReadLine()!;
        Console.WriteLine("Please enter an Alias");
        string alias = Console.ReadLine()!;
        DateTime createdAtDate = DateTime.Now;
        //Console.WriteLine("Please enter the Task Status - (0-4)");    לבדוק אם צריך את המצב של המשימה
        //int intStatus = int.Parse(Console.ReadLine()!);
        //BO.Status status = (BO.Status)intStatus;

        //למשימה יש רשימה שבה כתובה כל המשימות התלויות שלה
        //לכן נבקש מהמשתמש את התעודת זהות של המשימה ואז נקח את שאר הנתונים ונכניס אותם לרשימה
        Console.WriteLine("Insert dependent tasks (end with -1)"); 
        Console.WriteLine("Please enter the ID of the dependent task");
        List <BO.TaskInList> dependencies = new List<BO.TaskInList>();
        int IDOfDependTask = int.Parse(Console.ReadLine()!);
        while (IDOfDependTask != -1) 
        {
            BO.Task findTask = s_bl.Task.Read(IDOfDependTask);
            BO.TaskInList newTaskInLis = new BO.TaskInList()
            {
                Id = findTask.Id,
                Description = findTask.Description,
                Alias = findTask.Alias,
                Status = findTask.Status
            };
            dependencies.Add(newTaskInLis);
        }

        Console.WriteLine("RequiredEffortTime ");
        int requiredEffortTime = int.Parse(Console.ReadLine()!);
        TimeSpan days = new TimeSpan(requiredEffortTime, 0, 0, 0);
        Console.WriteLine("StartDate ");
        DateTime? startDate = DateTime.TryParse(Console.ReadLine(), out DateTime result) ? result : (DateTime?)null;
        Console.WriteLine("ScheduledDate ");
        DateTime? scheduledDate = DateTime.TryParse(Console.ReadLine(), out DateTime result1) ? result1 : (DateTime?)null;
        Console.WriteLine("ForecastDate ");
        DateTime? forecastDate = DateTime.TryParse(Console.ReadLine(), out DateTime result2) ? result2 : (DateTime?)null;
        Console.WriteLine("DeadlineDate ");
        DateTime? deadlineDate = DateTime.TryParse(Console.ReadLine(), out DateTime result3) ? result3 : (DateTime?)null;
        Console.WriteLine("CompleteDate ");
        DateTime? completeDate = DateTime.TryParse(Console.ReadLine(), out DateTime result4) ? result4 : (DateTime?)null;
        Console.WriteLine("Deliverables ");
        string? deliverables = Console.ReadLine();
        Console.WriteLine("Remarks ");
        string? remarks = Console.ReadLine();
        Console.WriteLine("Please enter the ID of the engineer working on this task");
        int engineerInTaskId = int.Parse(Console.ReadLine()!);
        BO.Engineer findTheEngineer = s_bl.Engineer.Read(engineerInTaskId);//נחפש את המהנדס לפי התעודת זהות וניצור מופע שלו
        BO.EngineerInTask? engineerInTask = new EngineerInTask() { Id = findTheEngineer.Id, Name = findTheEngineer.Name };
        Console.WriteLine("Please enter the difficulty level of the task (0-5)");
        EngineerExperience? copmlexity = (EngineerExperience?)int.Parse(Console.ReadLine()!);

        BO.Task task = new BO.Task()
        {
            Description = description,
            Alias = alias,
            CreatedAtDate = createdAtDate,
            Dependencies = dependencies,
            RequiredEffortTime = days,
            StartDate = startDate,
            ScheduledDate = scheduledDate,
            ForecastDate = forecastDate,
            DeadlineDate = deadlineDate,
            CompleteDate = completeDate,
            Deliverables =  deliverables,
            Remarks = remarks,
            Engineer = engineerInTask,
            Copmlexity = copmlexity
        };
        //נכניס את המשימה לרשימה
        s_bl.Task.Create(task);
    }

}















