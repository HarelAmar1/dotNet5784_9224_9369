using DalApi;
using BlApi;
using BlImplementation;
using BO;
using System.Reflection.Emit;
using DO;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                                createEngineer(); // Create a new Engineer
                                break;
                            case 2:
                                Console.WriteLine("Enter the engineer's ID:");
                                int id = int.Parse(Console.ReadLine()!);
                                PrintTheReadFunctionOfEngineer(s_bl.Engineer.Read(id));
                                break;
                            case 3:
                                PrintTheReadAllFunctionOfEngineer(s_bl.Engineer.ReadAll()); // Display all Engineers
                                break;
                            case 4:
                                s_bl!.Engineer.Update(UpdateHelperForEngineer()); // Update an Engineer
                                break;
                            case 5:
                                Console.WriteLine("Enter the ID of the Engineer you want to delete:");
                                id = int.Parse(Console.ReadLine()!);
                                s_bl!.Engineer.Delete(id);
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
        Console.WriteLine("Please enter your id and alias of the task:");
        BO.TaskInEngineer task = new BO.TaskInEngineer(int.Parse(Console.ReadLine()!), Console.ReadLine()!);
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
        Console.WriteLine(ToPrint.Task.Id);
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