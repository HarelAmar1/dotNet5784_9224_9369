using BlApi;
using BO;
using DO;
using System;
using System.Diagnostics.Metrics;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;


    //Create
    public int Create(BO.Task task)
    {
        //we will check the correctness of the ID and nickname
        checkData(task);

        //We will create the task and transfer it to Dal
        DO.Task newTask = new DO.Task
        (task.Id,
        task.Description,
        task.Alias,
        false,
        DateTime.Now,
        task.RequiredEffortTime,
        (DO.EngineerExperience)task.Copmlexity!,
        task.StartDate,
        task.ScheduledDate,
        task.DeadlineDate,
        task.CompleteDate,
        task.Deliverables,
        task.Remarks,
        task.Engineer?.Id);

        // an attempt will be made to insert it into the data
        try
        {
            int newTaskId = _dal.Task.Create(newTask);

            //add dependencies in dal
            task.Dependencies.ForEach(item =>
            {
                Dependency depend = new Dependency(0, newTaskId, item.Id);
                _dal.Dependency.Create(depend);
            });

            return newTaskId;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={task.Id} already exist", ex);
        }
    }


    //Delete
    public void Delete(int idTask)
    {
        //Find the task we want to delete
        BO.Task? task = Read(idTask);
        if (task == null)
            throw new BO.BlDoesNotExistException($"Task with ID ={idTask} is not exists");

        //we will check that there are no tasks that depend on this task
        List<DO.Dependency> dependlist = (List<DO.Dependency>)_dal.Dependency.ReadAll(null);
        bool check = !dependlist.Any(dep => dep.DependsOnTask == task.Id);

        //delete it
        if (check)
        {
            _dal.Task.Delete(idTask);

            //delete from the dependencies that separate the tasks related to it
            dependlist.Where(dep => dep.DependentTask == task.Id)
                      .ToList()
                      .ForEach(dep => _dal.Dependency.Delete(dep.Id));
        }
        //Otherwise we will throw an error
        else
            throw new BlDeletionImpossible("The task depends on other tasks");


    }

// Read
public BO.Task Read(int idTask)
{
    try
    {
        // Get the data from the DAL
        DO.Task? task = _dal.Task.Read(idTask);
        if (task == null)
            throw new BO.BlDoesNotExistException($"Task with ID ={idTask} does not exist");
        
        // We will create the engineer field in the task
        EngineerImplementation lookForNameOfEngineer = new EngineerImplementation();
        IEnumerable<BO.Engineer> engineers = lookForNameOfEngineer.ReadAll();
        BO.EngineerInTask engineerInTask = engineers
            .Where(E => E.Task.Id == idTask)
            .Select(E => new BO.EngineerInTask { Id = E.Id, Name = E.Name })
            .FirstOrDefault();

        // We will create the field of all dependencies of the given task
        List<DO.Dependency?> depenFromDal = (List<DO.Dependency?>)_dal.Dependency.ReadAll();
        List<BO.TaskInList> newForDepend = depenFromDal
            .Where(item => item.DependentTask == idTask)
            .Select(item =>
            {
                DO.Task findDependTask = _dal.Task.Read(item.DependsOnTask.Value); // Find the task from DAL
                return new BO.TaskInList // Create taskInList
                {
                    Id = findDependTask.Id,
                    Description = findDependTask.Description,
                    Alias = findDependTask.Alias,
                    Status = (BO.Status)status(findDependTask)
                };
            }).ToList();

        BO.Task taskToRead = new BO.Task
        {
            Id = task.Id,
            Description = task.Description,
            Alias = task.Alias,
            CreatedAtDate = task.CreatedAtDate,
            Status = (BO.Status)status(task),
            Dependencies = newForDepend,
            RequiredEffortTime = task.RequiredEffortTime,
            StartDate = task.StartDate,
            ScheduledDate = task.ScheduledDate,
            ForecastDate = null,
            DeadlineDate = task.DeadlineDate,
            CompleteDate = task.CompleteDate,
            Deliverables = task.Deliverables,
            Remarks = task.Remarks,
            Engineer = engineerInTask,
            Copmlexity = (BO.EngineerExperience)task.Copmlexity
        };
        return taskToRead;
    }
    catch (DO.DalDoesNotExistException ex)
    {
        throw new BO.BlDoesNotExistException($"Task with ID ={idTask} does not exist", ex);
    }
}



    //ReadAll
    public IEnumerable<BO.TaskInList> ReadAll(Func<DO.Task?, bool>? func = null)
    {
        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll(func).ToList();

        IEnumerable<BO.TaskInList> BOTasks =
        from task in tasks
        select new BO.TaskInList()
        {
            Id = task.Id,
            Description = task.Description,
            Alias = task.Alias,
            Status = (BO.Status)status(task)
        };
        return BOTasks;
    }

    //startDateTimeManagement
    public void startDateTimeManagement(int idTask, DateTime scheduleDateTime)
    {
        DO.Task taskWithNewDate = _dal.Task.Read(idTask) with { ScheduledDate = scheduleDateTime, DeadlineDate = scheduleDateTime + _dal.Task.Read(idTask).RequiredEffortTime };
        _dal.Task.Update(taskWithNewDate);
        return;

    }

    // Update
    public void Update(BO.Task task)
    {
        // We will check the correctness of the ID and nickname
        checkData(task);

        // We will delete all the dependencies related to the task we received
        _dal.Dependency.ReadAll()
            .Where(depend => depend.DependentTask == task.Id)
            .ToList()
            .ForEach(depend => _dal.Dependency.Delete(depend.Id));

        // We will add them back updated
        task.Dependencies.ForEach(depend =>
        {
            DO.Dependency insertUpdate = new DO.Dependency(0, task.Id, depend.Id);
            _dal.Dependency.Create(insertUpdate);
        });

        // Fill in the rest of the fields for the DAL task and return updated
        DO.Task newTask = new DO.Task
        (
            task.Id,
            task.Description,
            task.Alias,
            false,
            task.CreatedAtDate,
            task.RequiredEffortTime,
            (DO.EngineerExperience)task.Copmlexity!,
            task.StartDate,
            task.ScheduledDate,
            task.DeadlineDate,
            task.CompleteDate,
            task.Deliverables,
            task.Remarks,
            task.Engineer!.Id
        );

        // We will update the DAL
        _dal.Task.Update(newTask);
    }




    //Helper function to find the status of the task from DAL
    private int status(DO.Task task)
    {
        if (task.ScheduledDate == null)
            return 0;
        if (task.StartDate == null)
            return 1;
        if (task.CompleteDate == null)
            return 2;
        if (task.CompleteDate != null)
            return 3;
        // check how to check the last status
        return 0;
    }

    //validity check
    private void checkData(BO.Task task)
    {
        string error = "";
        if (task.Id < 0)
            error = $"Id: {task.Id}";
        else if (task.Alias == "")
            error = $"Alias: {task.Alias}";
        else if (task.Engineer != null) 
        {
            if ((int)task.Copmlexity > (int)Read(task.Engineer.Id).Copmlexity)
                error = "Task Complexity";
        }
        if (error != "")
            throw new BlIncorrectInputException($"{error}, is incorrect input");
    }

    
    public void dateGeneratorOfAllTasks(DateTime startOfProject)
    {
        IEnumerable<BO.TaskInList> tasksBO = ReadAll();
        //Finds everyone without dependencies
        List<BO.Task> tasksWithOutDependency = new List<BO.Task>();
        foreach (var task in tasksBO)
        {
            if (Read(task.Id).Dependencies.Count == 0)
            {
                tasksWithOutDependency.Add(Read(task.Id));
            }
        }
        //    Enter a start and end date
        foreach (var task in tasksWithOutDependency)
        {
            //put the dates in dal
            startDateTimeManagement(task.Id, startOfProject);
            task.ScheduledDate = startOfProject; //supporting function
            task.DeadlineDate = startOfProject + task.RequiredEffortTime;//supporting function

        }

        //Finds everyone who has an addiction
        List<BO.Task> tasksWithDependency = new List<BO.Task>();
        foreach (var task in tasksBO)
        {
            if (Read(task.Id).Dependencies.Count != 0)
            {
                tasksWithDependency.Add(Read(task.Id));
            }
        }

        foreach (var task in tasksWithDependency)
        {
            initScheduledDateRecursive(task);
        }

    }
    //recursive supporting function
    private DateTime? initScheduledDateRecursive(BO.Task task)
    {
        if (task.DeadlineDate != null)
            return task.DeadlineDate;
        foreach (var dep in task.Dependencies)
        {
            var findTheTaskofDependInList = Read(dep.Id);
            DateTime? DeadlineFromDepend = initScheduledDateRecursive(findTheTaskofDependInList);
            if (task.ScheduledDate == null)
            {
                startDateTimeManagement(task.Id, DeadlineFromDepend.GetValueOrDefault());
                task.ScheduledDate = DeadlineFromDepend;//supporting function
                task.DeadlineDate = DeadlineFromDepend + task.RequiredEffortTime;//supporting function
            }
            else
            {
                task.ScheduledDate = (DateTime.Compare(task.DeadlineDate.GetValueOrDefault(), DeadlineFromDepend.GetValueOrDefault()) > 0) ? task.ScheduledDate : DeadlineFromDepend;
                startDateTimeManagement(task.Id, task.ScheduledDate.GetValueOrDefault());
                task.DeadlineDate = task.ScheduledDate + task.RequiredEffortTime;//supporting function
            }
        }
        return null;
    }
}
