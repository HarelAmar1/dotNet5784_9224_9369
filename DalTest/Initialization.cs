namespace DalTest;
using DalApi;
using DO;
using System;
using System.Data;
using System.Threading.Tasks;

//Initialization class
public static class Initialization
{
    private static ITask? s_dalTask;
    private static IEngineer? s_dalEngineer;
    private static IDependency? s_dalDependency;

    private static readonly Random s_rand = new();
    private static void createEngineer()
    {
        Random random = new Random();
        int[] idForEngineer = { 987782, 664508, 710873, 158197, 429867 };
        string[] names = new string[] { "Dan", "Sam", "Jordan", "Taylor", "Morgan" };
        for (int i = 0; i < 5; i++)
        {
            int id = idForEngineer[i]; // 6-digit ID
            string email = names[i] + "@gmail.com";
            string name = names[i];
            EngineerExperience level = (EngineerExperience)i;
            double cost = 5 * random.Next(4, 6);
            bool active = true;

            Engineer engineer = new Engineer(id, email, cost, name, level, active);

            // Add engineer to your collection or process them as needed
            s_dalEngineer!.Create(engineer);
        }
    }
    private static void createTask()
    {
        //alias
        string[] engineeringTasks =
        {"Review Settings","Collect Requirements","Analyze Data","Design Seed Robot","Develop Planting Software",
        "Robot Testing","Plan Management System","Develop UI","System-Robot Integration","Field Trials",
        "Pest Control Robot","Build Growth Models","Integrate Models","Pest Monitoring","Monitoring Tests","Communication Interfaces",
        "Image Analysis Dev","Image-Robot Integration","Water Management Dev","Water System Tests","Harvesting Robot Dev",
        "Harvesting Tests","Automate Management","Advanced Analysis Impl","System Field Testing"};

        //Descriptions
        string[] taskDescriptions =
        {"Review Settings","Collect Requirements","Analyze Data","Design Seed Robot","Develop Planting Software","Robot Testing",
        "Plan Management System","Develop UI","System-Robot Integration","Field Trials","Pest Control Robot","Build Growth Models",
        "Integrate Models","Pest Monitoring","Monitoring Tests","Communication Interfaces",
        "Image Analysis Dev","Image-Robot Integration","Water Management Dev","Water System Tests","Harvesting Robot Dev",
        "Harvesting Tests","Automate Management","Advanced Analysis Impl","System Field Testing" };


        //task deliverables
        string[] taskDeliverables =
        {"Project Plan Document","Requirements Report","Data Analysis Summary","Seed Robot Design",
        "Planting Software Code","Robot Test Results","Management System Plan","UI Design Layout","Integration Report",
        "Field Trial Data","Pest Control Robot Design","Growth Model Algorithms","Model Integration Report","Monitoring System Blueprint",
        "System Test Report","Communication Protocol","Image Processing Software","Integration Test Report","Water Management System Plan",
        "Field Test Data for Water System","Harvesting Robot Design","Harvesting Field Test Data","Automated Management Workflow",
        "Advanced Analysis Implementation Report","Complete System Field Test Report"};

        //remarks of task
        string[] taskRemarks =
        {"Verify with project goals","Include all stakeholder inputs","Ensure data accuracy","Focus on efficiency and reliability",
        "Adhere to coding standards","Check for robustness under different conditions","Outline clear management objectives","User-friendly and intuitive",
        "Ensure seamless integration","Analyze and document trial outcomes","Innovative design to address common pests","Accuracy and predictive reliability","Synchronize models with real-time data",
        "Optimize for early detection","Document system effectiveness","Ensure compatibility and security","Optimize for speed and accuracy",
        "Thorough testing with different robot models","Eco-friendly and cost-effective","Record performance in varying conditions","Balance efficiency with gentle handling",
        "Emphasize on safety and productivity","Automate routine tasks","Leverage cutting-edge analytical techniques","Comprehensive testing across all modules"};

        //level of engineer
        int[] taskLevels = { 1, 1, 1, 2, 3, 2, 3, 3, 4, 2, 3, 4, 4, 4, 2, 3, 4, 4, 3, 2, 3, 2, 3, 5, 2 };//כל מספר מסמן את הרמה שצריך באותו האינדקס

        //array for engineer id
        int[] idForEngineer = { 987782, 664508, 710873, 158197, 429867 };

        Random random = new Random();
        //init Task with ID, Alias, Descriptions,MaileStone, Level
        TimeSpan effortDuration = new TimeSpan(random.Next(7, 21), 0, 0, 0); //3 days for requiredEffortTime
        TimeSpan sevenDays = new TimeSpan(7, 0, 0, 0);
        for (int i = 0; i < 25; i++)
        {
            string alias = engineeringTasks[i];
            string descriptions = taskDescriptions[i];
            DateTime? createTask = DateTime.Now;
            DateTime? requiredEffortTime = createTask?.Add(effortDuration);
            bool isMilestone = random.Next(2) == 0 ? true : false;
            EngineerExperience level = (EngineerExperience)taskLevels[i];
            DateTime? startDate = createTask?.Add(effortDuration);
            DateTime? scheduledDate = createTask?.Add(sevenDays);
            DateTime? deadlineDate = createTask?.Add(effortDuration);
            DateTime? completeDate = null;
            string? deliverables = taskDeliverables[i];
            string? remarks = taskRemarks[i];
            int engineerId = idForEngineer[taskLevels[i] - 1];
            DO.Task task = new DO.Task(i, alias, descriptions, createTask, requiredEffortTime, isMilestone,
            level, startDate, scheduledDate, deadlineDate, completeDate, deliverables, remarks, engineerId);
            s_dalTask!.Create(task);
        }
    }
    private static void createDependency()
    {
        Random random = new Random();
        for (int i = 0; i < 40; i++)
        {
            int id = i; // Current task ID
            //Current task number
            int[] arrayOfDependentTask = { 2, 3, 3, 5, 5, 6, 6, 8, 9, 9, 9, 9, 10, 10, 10, 13, 14, 15,
             15, 18, 20, 22, 23, 24, 24, 24, 24, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25 };

            //The current task depends on this task
            int[] arrayOfDependsOnTask = { 1, 2, 1, 4, 1, 5, 4, 7, 5, 8, 4, 7, 6, 5, 4, 12, 11, 14, 11,
             17, 19, 21, 7, 13, 23, 7, 12, 10, 6, 5, 4, 15, 14, 11, 18, 17, 20, 19, 22, 21 };

            Dependency dependency = new Dependency(id, arrayOfDependentTask[i], arrayOfDependsOnTask[i]);
            s_dalDependency!.Create(dependency);
        }

    }

    public static void Do(IDependency? dalDependency, IEngineer? dalEngineer, ITask? dalTask)//Check that it doesn't return anything
    {
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");


        //Calling the methods
        createDependency();
        createEngineer();
        createTask();
    }

}