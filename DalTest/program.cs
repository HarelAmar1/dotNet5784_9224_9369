namespace DalTest;
using Dal;
using DalApi;
using DO;
using System;
using System.Collections.Specialized;

public class Program
{
    //static readonly IDal s_dal = new DalXml();
    static readonly IDal s_dal = new DalList();
    
    // Main method - the entry point of the application
    private static void Main(string[] args)
    {
        
        // Get user input from the main menu
        int userInput = menu();
        int id; // Variable to store IDs entered by the user


        // Main loop to handle user choices
        while (userInput != 0)
        {
            try
            {
                switch (userInput)
                {
                    case 1: // Task operations
                        int OpForTask = options(); // Display Task options
                        while (OpForTask != 0)
                        {
                            switch (OpForTask)
                            {
                                case 1:
                                    createTask(); // Create a new Task
                                    break;
                                case 2:
                                    Console.WriteLine("Enter the Task's ID:");
                                    id = int.Parse(Console.ReadLine()!);
                                    PrintTheReadFunctionOfTask(s_dal.Task.Read(id)); // Display a specific Task
                                    break;
                                case 3:
                                    PrintTheReadAllFunctionOfTask(s_dal!.Task.ReadAll()); // Display all Tasks
                                    break;
                                case 4:
                                    s_dal!.Task.Update(UpdateHelperTask()); // Update a Task
                                    break;
                                case 5:
                                    Console.WriteLine("Enter the ID of the Task you want to delete:");
                                    id = int.Parse(Console.ReadLine()!);
                                    s_dal!.Task.Delete(id); // Delete a Task
                                    break;
                                case 0:
                                    OpForTask = 0; // Exit Task operations
                                    break;
                            }
                            OpForTask = options(); // Show Task options again
                        }
                        break;
                    case 2: // Dependency operations
                        int OpForDependency = options(); // Display Dependency options
                        while (OpForDependency != 0)
                        {
                            switch (OpForDependency)
                            {
                                case 1:
                                    createDependency(); // Create a new Dependency
                                    break;
                                case 2:
                                    Console.WriteLine("Enter the Dependency's ID:");
                                    id = int.Parse(Console.ReadLine()!);
                                    PrintTheReadfunctionOfDependency(s_dal!.Dependency.Read(id)); // Display a specific Dependency
                                    break;
                                case 3:
                                    PrintTheReadAllFunctionOfDependency(s_dal!.Dependency.ReadAll()); // Display all Dependencies
                                    break;
                                case 4:
                                    s_dal!.Dependency.Update(UpdateHelperForDepend()); // Update a Dependency
                                    break;
                                case 5:
                                    Console.WriteLine("Enter the ID of the Dependency you want to delete:");
                                    id = int.Parse(Console.ReadLine()!);
                                    s_dal!.Dependency.Delete(id); // Delete a Dependency
                                    break;
                                case 0:
                                    OpForDependency = 0; // Exit Dependency operations
                                    break;
                            }
                            OpForDependency = options(); // Show Dependency options again
                        }
                        break;
                    case 3: // Engineer operations
                        int OpForEngineer = options(); // Display Engineer options
                        while (OpForEngineer != 0)
                        {
                            switch (OpForEngineer)
                            {
                                case 1:
                                    createEngineer(); // Create a new Engineer
                                    break;
                                case 2:
                                    Console.WriteLine("Enter the engineer's ID:");
                                    id = int.Parse(Console.ReadLine()!);
                                    PrintTheReadfunctionOfEngineer(s_dal!.Engineer.Read(id)); // Display a specific Engineer
                                    break;
                                case 3:
                                    PrintTheReadAllFunctionOfEngineer(s_dal!.Engineer.ReadAll()); // Display all Engineers
                                    break;
                                case 4:
                                    s_dal!.Engineer.Update(UpdateHelperForEngineer()); // Update an Engineer
                                    break;
                                case 5:
                                    Console.WriteLine("Enter the ID of the Engineer you want to delete:");
                                    id = int.Parse(Console.ReadLine()!);
                                    s_dal!.Engineer.Delete(id); // Delete an Engineer
                                    break;
                                default:
                                    OpForEngineer = 0; // Exit Engineer operations
                                    break;
                            }
                            OpForEngineer = options(); // Show Engineer options again
                        }
                        break;
                    case 4:
                        initTheXml(); // Init the data
                        break;
                    default:
                        Console.WriteLine("Enter a Valid value"); // Handle invalid input
                        break;
                } // End of switch statement
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Exception handling
            }

            userInput = menu(); // Display the main menu again
        }//End while


    }


    public static void initTheXml()
    {
        Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
        if (ans == "Y") //stage 3
        {
            try
            {
                Initialization.Do(s_dal); 
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
        Console.WriteLine("2 - Choose Dependency");
        Console.WriteLine("3 - Choose Engineer");
        Console.WriteLine("4 - Choose Initial Data");//if the user want initial data from Stage 3
        Console.WriteLine("0 - exit");
        int firstMenu = int.Parse(Console.ReadLine()!);
        return firstMenu;
    }
    // Function to display a submenu for CRUD operations and capture user's choice
    public static int options()
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

    // Function to create a new Dependency
    public static void createDependency()
    {
        Console.WriteLine("Please enter your ID of Depenency");
        int dependencyNow = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Please enter your ID of most previose Depenency");
        int dependencyDep = int.Parse(Console.ReadLine()!);
        Dependency dependency = new Dependency(0, dependencyNow, dependencyDep);
        s_dal!.Dependency.Create(dependency);
    }

    // Function to create a new Engineer
    public static void createEngineer()
    {
        Console.WriteLine("Please enter your ID:");
        int id = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Please enter your email:");
        string? email = Console.ReadLine();
        Console.WriteLine("Please enter your hourly cost:");
        double cost = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Please enter your name:");
        string? name = Console.ReadLine();
        Console.WriteLine("Please enter your experience level (1-5)");
        int l = int.Parse(Console.ReadLine()!);
        DO.EngineerExperience level = (EngineerExperience)l;
        Engineer engineer = new Engineer(id, email, cost, name, level, true);
        s_dal!.Engineer.Create(engineer);
    }

    // Function to create a new Task
    public static void createTask()
    {
        Console.WriteLine("Please enter an Alias");
        string alias = Console.ReadLine()!;
        Console.WriteLine("Please enter a Description");
        string description = Console.ReadLine()!;
        DateTime? createdAtDate = DateTime.Now;

        Console.WriteLine("Is this task a Milestone? (yes/no):");
        bool isMilestone = Console.ReadLine()!.ToLower() == "yes";

        Console.WriteLine("Please enter Complexity Level (0 for Beginner, 1 for AdvancedBeginner, etc.):");
        EngineerExperience complexity = (EngineerExperience)int.Parse(Console.ReadLine()!);
        Console.WriteLine("Please enter Deliverables");
        string deliverables = Console.ReadLine()!;
        Console.WriteLine("Please enter any Remarks");
        string remarks = Console.ReadLine()!;
        Console.WriteLine("Please enter the Engineer ID:");
        int engineerId = int.Parse(Console.ReadLine()!);
        Task task = new Task(0, alias, description, createdAtDate, null, isMilestone, complexity, null, null, null, null, deliverables, remarks, engineerId);
        s_dal!.Task.Create(task);
    }
    public static DO.Task UpdateHelperTask()//func to create item for Update Task
    {
        Console.WriteLine("Please enter the Task ID:");
        int id = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Please enter an Alias");
        string alias = Console.ReadLine()!;
        Console.WriteLine("Please enter a Description");
        string description = Console.ReadLine()!;
        DateTime? createdAtDate = DateTime.Now;

        Console.WriteLine("Is this task a Milestone? (yes/no):");
        bool isMilestone = Console.ReadLine()!.ToLower() == "yes";

        Console.WriteLine("Please enter Complexity Level (0 for Beginner, 1 for AdvancedBeginner, etc.):");
        EngineerExperience complexity = (EngineerExperience)int.Parse(Console.ReadLine()!);
        Console.WriteLine("Please enter Deliverables");
        string deliverables = Console.ReadLine()!;
        Console.WriteLine("Please enter any Remarks");
        string remarks = Console.ReadLine()!;
        Console.WriteLine("Please enter the Engineer ID:");
        int engineerId = int.Parse(Console.ReadLine()!);
        DO.Task temp = new Task(id, alias, description, createdAtDate, null, isMilestone, complexity, null, null, null, null, deliverables, remarks, engineerId);
        return temp;
    }
    public static DO.Dependency UpdateHelperForDepend()//func to create item for Update Dependency
    {
        Console.WriteLine("Please enter your ID");
        int id = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Please enter your ID of Depenency");
        int dependencyNow = Console.Read();
        Console.WriteLine("Please enter your ID of most previose Depenency");
        int dependencyDep = int.Parse(Console.ReadLine()!);
        DO.Dependency temp = new Dependency(id, dependencyNow, dependencyDep);
        return temp;
    }
    public static DO.Engineer UpdateHelperForEngineer()//func to create item for Update Engineer
    {
        Console.WriteLine("Please enter your ID:");
        int id = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Please enter your email:");
        string? email = Console.ReadLine();
        Console.WriteLine("Please enter your hourly cost:");
        double cost = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Please enter your name:");
        string? name = Console.ReadLine();
        Console.WriteLine("Please enter your experience level (1-5)");
        int l = int.Parse(Console.ReadLine()!);
        DO.EngineerExperience level = (EngineerExperience)l;
        DO.Engineer temp = new Engineer(id, email, cost, name, level, true);
        return temp;
    }
    // Prints detailed information of a single Task object.
    public static void PrintTheReadFunctionOfTask(DO.Task ToPrint)
    {
        Console.Write("ID: ");
        Console.WriteLine(ToPrint.Id);
        Console.Write("Alias: ");
        Console.WriteLine(ToPrint.Alias);
        Console.Write("Description: ");
        Console.WriteLine(ToPrint.Description);
        Console.Write("Created At Date: ");
        Console.WriteLine(ToPrint.CreatedAtDate);
        Console.Write("Required Effort Time: ");
        Console.WriteLine(ToPrint.RequiredEffortTime);
        Console.Write("Is Milestone: ");
        Console.WriteLine(ToPrint.IsMilestone);
        Console.Write("Copmlexity: ");
        Console.WriteLine(ToPrint.Copmlexity);
        Console.Write("Start Date: ");
        Console.WriteLine(ToPrint.StartDate);
        Console.Write("Scheduled Date: ");
        Console.WriteLine(ToPrint.ScheduledDate);
        Console.Write("Deadline Date: ");
        Console.WriteLine(ToPrint.DeadlineDate);
        Console.Write("Complete Date: ");
        Console.WriteLine(ToPrint.CompleteDate);
        Console.Write("Deliverables: ");
        Console.WriteLine(ToPrint.Deliverables);
        Console.Write("Remarks: ");
        Console.WriteLine(ToPrint.Remarks);
        Console.Write("Engineer Id: ");
        Console.WriteLine(ToPrint.EngineerId + "\n");
    }
    // Displays details of a single Dependency object, including its current and dependent tasks.
    public static void PrintTheReadfunctionOfDependency(DO.Dependency ToPrint)
    {
        Console.Write("ID: ");
        Console.WriteLine(ToPrint.Id);
        Console.Write("Current Task: ");
        Console.WriteLine(ToPrint.DependentTask);
        Console.Write("The current Task depends on the task: ");
        Console.WriteLine(ToPrint.DependsOnTask + "\n");
    }
    // Outputs the information of a single Engineer object, including ID, email, cost, name, level, and active status.
    public static void PrintTheReadfunctionOfEngineer(DO.Engineer ToPrint)
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
        Console.WriteLine(ToPrint.level);
        Console.Write("Active: ");
        Console.WriteLine(ToPrint.Active + "\n");
    }

    // Prints details of each Task object in the provided list
    public static void PrintTheReadAllFunctionOfTask(IEnumerable<DO.Task> toPrint)
    {
        foreach (DO.Task task in toPrint)
        {
            PrintTheReadFunctionOfTask(task);
        }
    }

    // Prints details of each Dependency object in the provided list
    public static void PrintTheReadAllFunctionOfDependency(IEnumerable<DO.Dependency> toPrint)
    {
        foreach (DO.Dependency dependency in toPrint)
        {
            PrintTheReadfunctionOfDependency(dependency);
        }
    }

    // Prints details of each Engineer object in the provided list
    public static void PrintTheReadAllFunctionOfEngineer(IEnumerable<DO.Engineer> toPrint)
    {
        foreach (DO.Engineer engineer in toPrint)
        {
            PrintTheReadfunctionOfEngineer(engineer);
        }
    }

}