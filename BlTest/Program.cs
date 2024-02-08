using BO;
using BlApi;
using DalApi;
using BlImplementation;
using System.Reflection.Emit;
using DO;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalTest;

namespace BlTest;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static void Main(string[] args)
    {
        try
        {
            //Data Initialization
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
            {
                DalTest.Initialization.deleteXMLFile();//We will initialize the Xml files
                DalTest.Initialization.Do();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message); // Exception handling
        }

        //Temporary variable of the start date of the entire projectproject
        DateTime? startProject = null; 

        // Get user input from the main menu
        int userInput = menu();

        while (userInput != 0)
        {
            try
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
                                    Console.WriteLine("Enter the Task's ID:");
                                    int idForReadTask = int.Parse(Console.ReadLine()!);
                                    PrintTheReadFunctionOfTask(s_bl.Task.Read(idForReadTask));
                                    break;
                                case 3:
                                    PrintTheReadAllFunctionOfTask(s_bl.Task.ReadAll()); // Display all Tasks
                                    break;
                                case 4:
                                    BO.Task updateTaskTask = UpdateHelperTask();
                                    s_bl!.Task.Update(updateTaskTask); // Update an Task
                                    break;
                                case 5:
                                    Console.WriteLine("Enter the ID of the Task you want to delete:");
                                    int idForDeleteTask = int.Parse(Console.ReadLine()!);
                                    s_bl!.Task.Delete(idForDeleteTask);
                                    break;
                                case 0:
                                    OpForTask = 0; // Exit Task operations
                                    break;
                            }
                            OpForTask = optionsForTask(); // Show Task options again
                        }//End While
                        break;
                    case 2:
                        int OpForEngineer = optionsForEngineer(); // Display Engineer options
                        while (OpForEngineer != 0)
                        {
                            switch (OpForEngineer)
                            {
                                case 1:
                                    createEngineer(); // Create a new Engineer
                                    break;
                                case 2:
                                    Console.WriteLine("Enter the engineer's ID:");
                                    int idForReadEngineer = int.Parse(Console.ReadLine()!);
                                    PrintTheReadFunctionOfEngineer(s_bl.Engineer.Read(idForReadEngineer));
                                    break;
                                case 3:
                                    PrintTheReadAllFunctionOfEngineer(s_bl.Engineer.ReadAll()); // Display all Engineers
                                    break;
                                case 4:
                                    s_bl!.Engineer.Update(UpdateHelperForEngineer()); // Update an Engineer
                                    break;
                                case 5:
                                    Console.WriteLine("Enter the ID of the Engineer you want to delete:");
                                    int idForDeleteEngineer = int.Parse(Console.ReadLine()!);
                                    s_bl!.Engineer.Delete(idForDeleteEngineer);
                                    break;
                            }
                            OpForEngineer = optionsForEngineer(); // Show Engineer options again
                        }//End While
                        break;
                    case 3:
                        //Ask the manager to enter the project start date
                        Console.WriteLine("Enter A Project Start Date");
                        startProject = DateTime.TryParse(Console.ReadLine(), out DateTime result) ? result : (DateTime?)null;
                        //We will activate the function that generates the start dates of all the tasks
                        s_bl.Task.dateGeneratorOfAllTasks(startProject.GetValueOrDefault());
                        break;


                }
                userInput = menu();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Exception handling
            }

        }
    }

    // Menu function to display the main menu and capture user's choice
    public static int menu()
    {
        Console.WriteLine("Choose from next list");
        Console.WriteLine("1 - Choose Task");
        Console.WriteLine("2 - Choose Engineer");
        Console.WriteLine("3 - Done, Go from the planning stage to the execution stage - (Inserting a project start date)");
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
        //We will create the task in the task generator
        //and put the task in the list
        s_bl.Task.Create(taskGenerator("Create"));
    }
    public static void PrintTheReadFunctionOfTask(BO.Task toPrint)
    {
        Console.Write("ID: ");
        Console.WriteLine(toPrint.Id);
        Console.Write("Description: ");
        Console.WriteLine(toPrint.Description);
        Console.Write("Alias: ");
        Console.WriteLine(toPrint.Alias);
        Console.Write("Created At Date: ");
        Console.WriteLine(toPrint.CreatedAtDate);
        Console.Write("Status: ");
        Console.WriteLine(toPrint.Status);
        //print the dependencies
        if (toPrint.Dependencies.Count != 0) 
            Console.Write("Dependencies: ");
        foreach (var depen in toPrint.Dependencies)
        {
            Console.Write(depen.Id);
            Console.Write(" ");
        }
        Console.Write("\nRequired Effort Time: ");
        Console.WriteLine(toPrint.RequiredEffortTime);
        Console.Write("Start Date: ");
        Console.WriteLine(toPrint.StartDate);
        Console.Write("Scheduled Date: ");
        Console.WriteLine(toPrint.ScheduledDate);
        Console.Write("Forecast Date: ");
        Console.WriteLine(toPrint.ForecastDate);
        Console.Write("Deadline Date: ");
        Console.WriteLine(toPrint.DeadlineDate);
        Console.Write("Complete Date: ");
        Console.WriteLine(toPrint.CompleteDate);
        Console.Write("Deliverables: ");
        Console.WriteLine(toPrint.Deliverables);
        Console.Write("Remarks: ");
        Console.WriteLine(toPrint.Remarks);
        if (toPrint.Engineer != null) //If there is no engineer yet then there is nothing to print
            Console.Write($"Engineer - Id: {toPrint.Engineer.Id}, Name: {toPrint.Engineer.Name}\n");
        Console.Write("Complexity: ");
        Console.WriteLine(toPrint.Copmlexity);
    }
    public static void PrintTheReadAllFunctionOfTask(IEnumerable<BO.TaskInList> list)
    {
        foreach (var task in list)
        {
            Console.Write("ID: ");
            Console.WriteLine(task.Id);
            Console.Write("Description: ");
            Console.WriteLine(task.Description);
            Console.Write("Alias: ");
            Console.WriteLine(task.Alias);
            Console.Write("Status: ");
            Console.WriteLine(task.Status);
            
        }
    }

    public static BO.Task UpdateHelperTask()//func to create item for Update Task
    {
        return taskGenerator("Update");
    }


    public static void DateTimeManagement()
    {
        Console.WriteLine("Enter the Task's ID:");
        int id = int.Parse(Console.ReadLine()!);
        Console.WriteLine("please enter the ScheduledDate:");
        DateTime ScheduledDate = DateTime.Parse(Console.ReadLine());
        s_bl.Task.startDateTimeManagement(id, ScheduledDate);
    }

    private static BO.Task taskGenerator(string from)
    {
        int idForUpdate = 0;
        if (from == "Update")
        {
            Console.WriteLine("Enter the Task's ID:");
            idForUpdate = int.Parse(Console.ReadLine()!);
        }
        Console.WriteLine("Please enter a Description");
        string description = Console.ReadLine()!;
        Console.WriteLine("Please enter an Alias");
        string alias = Console.ReadLine()!;
        DateTime createdAtDate = DateTime.Now;
        //A task has a list with all its dependent tasks
        //therefore we will ask the user for the task's ID and then take the rest of the data and put it in the list
        Console.WriteLine("Insert dependent tasks - (click -1 to skip)");
        Console.WriteLine("Please enter the ID of the dependent task");
        List<BO.TaskInList> dependencies = new List<BO.TaskInList>();
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
            IDOfDependTask = int.Parse(Console.ReadLine()!);
        }
        Console.WriteLine("RequiredEffortTime ");
        int requiredEffortTime = int.Parse(Console.ReadLine()!);
        TimeSpan days = new TimeSpan(requiredEffortTime, 0, 0, 0);
        Console.WriteLine("Deliverables ");
        string? deliverables = Console.ReadLine();
        Console.WriteLine("Remarks ");
        string? remarks = Console.ReadLine();
        Console.WriteLine("Enter Engineer's ID - (click -1 to skip)");
        int engineerInTaskId = int.Parse(Console.ReadLine()!);
        BO.Engineer? findTheEngineer = null;
        BO.EngineerInTask? engineerInTask = null;
        if (engineerInTaskId != -1) 
        {
            findTheEngineer = s_bl.Engineer.Read(engineerInTaskId);//We will search for the engineer by ID and create an instance of him
            engineerInTask = new EngineerInTask() { Id = findTheEngineer.Id, Name = findTheEngineer.Name };
        }
        Console.WriteLine("Please enter the difficulty level of the task (0-5)");
        BO.EngineerExperience? copmlexity = (BO.EngineerExperience?)int.Parse(Console.ReadLine()!);

        BO.Task task = new BO.Task()
        {
            Id = idForUpdate,
            Description = description,
            Alias = alias,
            CreatedAtDate = createdAtDate,
            Dependencies = dependencies,
            RequiredEffortTime = days,
            StartDate = null,
            ScheduledDate = null,
            ForecastDate = null,
            DeadlineDate = null,
            CompleteDate = null,
            Deliverables = deliverables,
            Remarks = remarks,
            Engineer = engineerInTask,
            Copmlexity = copmlexity
        };
        return task;
    }


    public static void createEngineer()
    {
        Console.WriteLine("Please enter your ID:");
        int id = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Please enter your email:");
        string email = Console.ReadLine()!;
        Console.WriteLine("Please enter your hourly cost:");
        double cost = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Please enter your name:");
        string name = Console.ReadLine()!;
        Console.WriteLine("Please enter your experience level (1-5)");
        int levelFromUser = int.Parse(Console.ReadLine()!);
        BO.EngineerExperience level = (BO.EngineerExperience)levelFromUser;
        Console.WriteLine("Please enter your id of the task:");
        int idOfTask = int.Parse(Console.ReadLine());
        string aliasOfTask = s_bl.Task.Read(idOfTask).Alias;
        BO.TaskInEngineer task = new BO.TaskInEngineer(idOfTask, aliasOfTask);
        BO.Engineer engineer = new BO.Engineer()
        {
            Id = id,
            Name = name,
            Email = email,
            Level = level,
            Cost = cost,
            Task = task
        };
        s_bl.Engineer.Create(engineer);
    }
    public static void PrintTheReadFunctionOfEngineer(BO.Engineer ToPrint)
    {
        Console.Write("ID: ");
        Console.WriteLine(ToPrint.Id);
        Console.Write("Email: ");
        Console.WriteLine(ToPrint.Email);
        Console.Write("Cost: ");
        Console.WriteLine(ToPrint.Cost);
        Console.Write("Name: ");
        Console.WriteLine(ToPrint.Name);
        Console.Write("level: ");
        Console.WriteLine(ToPrint.Level);
        Console.Write("Id for Task: ");
        Console.WriteLine(ToPrint?.Task.Id);
        Console.Write("Alias for Task: ");
        Console.WriteLine(ToPrint.Task.Alias);
    }
    public static void PrintTheReadAllFunctionOfEngineer(IEnumerable<BO.Engineer> toPrint)
    {
        foreach (BO.Engineer engineer in toPrint)
        {
            PrintTheReadFunctionOfEngineer(engineer);
        }
    }
    public static BO.Engineer UpdateHelperForEngineer() //func to create item for Update Engineer
    {
        Console.WriteLine("Please enter your ID:");
        int id = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Please enter your email:");
        string email = Console.ReadLine();
        Console.WriteLine("Please enter your hourly cost:");
        double cost = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Please enter your name:");
        string name = Console.ReadLine();
        Console.WriteLine("Please enter your experience level (1-5)");
        int levelFromUser = int.Parse(Console.ReadLine()!);
        BO.EngineerExperience level = (BO.EngineerExperience)levelFromUser;
        Console.WriteLine("Please enter your id and alias of the task:");
        BO.TaskInEngineer task = new BO.TaskInEngineer(int.Parse(Console.ReadLine()!), Console.ReadLine()!);
        BO.Engineer temp = new BO.Engineer()
        {
            Id = id,
            Name = name,
            Email = email,
            Level = level,
            Cost = cost,
            Task = task
        };
        s_bl.Engineer.Update(temp);
        return temp;
    }
}