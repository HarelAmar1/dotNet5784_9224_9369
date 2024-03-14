using BlApi;
using BO;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    internal TaskImplementation(IBl bl) => _bl = bl;

    private readonly IBl _bl;

    private DalApi.IDal _dal = Factory.Get;

    //Create
    /// <param name="task">A task that needs to be added.</param>
    /// <returns>Returns the ID of the assignment.</returns>
    /// <exception cref="BO.BlAlreadyExistsException">An exception that indicates an assignment with the same ID already exists.</exception>
    public int Create(BO.Task task)
    {
        // Check the correctness of the ID and nickname
        checkData(task);

        // Create the task and transfer it to Dal
        DO.Task newTask = new DO.Task
        (
            task.Id,
            task.Description,
            task.Alias,
            false,
            _bl.Clock,
            task.RequiredEffortTime,
            (DO.EngineerExperience)task.Copmlexity!,
            task.StartDate,
            task.ScheduledDate,
            task.DeadlineDate,
            task.CompleteDate,
            task.Deliverables,
            task.Remarks,
            task.Engineer?.Id
        );

        // Attempt to insert it into the data
        try
        {
            int newTaskId = _dal.Task.Create(newTask);
            // Add dependencies
            IEnumerable<DO.Dependency> dependencies = from item in task.Dependencies
                                                      let depend = new Dependency(0, newTaskId, item.Id)
                                                      select depend;
            // Insert dependencies
            foreach (var depend in dependencies)
            {
                _dal.Dependency.Create(depend);
            }
            return newTaskId;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID: {task.Id} already exists", ex);
        }
    }


    //Delete
    /// <param name="idTask">ID of a task that needs to be deleted.</param>
    /// <exception cref="BO.BlDoesNotExistException">Thrown when the task to be deleted does not exist.</exception>
    /// <exception cref="BlDeletionImpossible">Thrown when the task cannot be deleted.</exception>
    public void Delete(int idTask)
    {

        // Find the task we want to delete
        BO.Task? task = Read(idTask);
        if (task == null)
            throw new BO.BlDoesNotExistException($"Task with ID: {idTask} does not exist");
        // Check that there are no tasks that depend on this task
        IEnumerable<DO.Dependency> dependlist = _dal.Dependency.ReadAll();
        bool hasDependentTasks = dependlist.Any(dep => dep.DependsOnTask == task.Id);

        // Delete the task if there are no dependent tasks
        if (!hasDependentTasks)
        {
            _dal.Task.Delete(idTask);

            // Delete dependencies related to the task
            IEnumerable<DO.Dependency> dependenciesToDelete = dependlist.Where(dep => dep.DependentTask == task.Id);
            foreach (var dep in dependenciesToDelete)
            {
                _dal.Dependency.Delete(dep.Id);
            }
        }
        else
        {
            // Otherwise, throw an error
            throw new BlDeletionImpossible("The task depends on other tasks");
        }
    }

    // Read
    /// <param name="idTask">Task selection by ID.</param>
    /// <returns>Returns the task with the received ID.</returns>
    /// <exception cref="BO.BlDoesNotExistException">Thrown when the requested task does not exist.</exception>
    public BO.Task Read(int idTask)
    {
        try
        {
            // Get the data from the DAL
            DO.Task? task = _dal.Task.Read(idTask);
            if (task == null)
                throw new BO.BlDoesNotExistException($"Task with ID: {idTask} does not exist");

            // Create the engineer field in the task

            BO.EngineerInTask? engineerInTask = (
                from E in new EngineerImplementation().ReadAll()
                where E.Task?.Id != null && E.Task.Id == idTask
                select new BO.EngineerInTask { Id = E.Id, Name = E.Name }
            ).FirstOrDefault();

            // Create the field of all dependencies of the given task
            IEnumerable<DO.Dependency?> depenFromDal = _dal.Dependency.ReadAll();
            List<BO.TaskInList> newForDepend = (
                from item in depenFromDal
                where item.DependentTask == idTask
                let findDependTask = _dal.Task.Read(item.DependsOnTask.Value)
                select new BO.TaskInList
                {
                    Id = findDependTask.Id,
                    Description = findDependTask.Description,
                    Alias = findDependTask.Alias,
                    Status = (BO.Status)status(findDependTask)
                }
            ).ToList();

            BO.Task? taskToRead = new BO.Task
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
            throw new BO.BlDoesNotExistException($"Task with ID: {idTask} does not exist", ex);
        }
    }


    //ReadAll

    /// <param name="func">Task filtering function.</param>
    /// <returns>Returns a list of tasks of type TaskInList.</returns>
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


    // Update

    /// <param name="task">A task that needs to be updated in the data layer.</param>
    public void Update(BO.Task task)
    {
        // Check the correctness of the ID and nickname
        checkData(task);

        // Delete all the dependencies related to the task we received
        IEnumerable<Dependency> dependenciesToDelete = _dal.Dependency.ReadAll().Where(depend => depend.DependentTask == task.Id);
        foreach (var depend in dependenciesToDelete)
        {
            _dal.Dependency.Delete(depend.Id);
        }

        // Add them back updated
        var newDependencies = task.Dependencies.Select(depend => new DO.Dependency(0, task.Id, depend.Id));
        foreach (var insertUpdate in newDependencies)
        {
            _dal.Dependency.Create(insertUpdate);
        }


        // Fill in the rest of the fields for the DAL task and return updated
        DO.Task newTask = new DO.Task
        (task.Id,
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
        task.Engineer?.Id);

        // Update the DAL
        _dal.Task.Update(newTask);
    }



    //Helper function to find the status of the task from DAL
    /// <summary>
    /// Determines the status of the task based on its properties.
    /// </summary>
    /// <param name="task">The task to evaluate.</param>
    /// <returns>The status code representing the task's status.</returns>
    private int status(DO.Task task)
    {
        if (task.ScheduledDate == null)
            return 0;
        if (task.StartDate == null)
            return 1;
        if (task.CompleteDate == null)
            return 2;
        if (task.CompleteDate != null)
            return 4;
        // If no condition is met, consider it in some unknown status
        return 0;
    }

    //startDateTimeManagement
    /// <summary>
    /// Updates the scheduled date of a task and its deadline based on provided data.
    /// </summary>
    /// <param name="idTask">The ID of the task.</param>
    /// <param name="scheduleDateTime">The new scheduled date for the task.</param>
    public void startDateTimeManagement(int idTask, DateTime scheduleDateTime)
    {
        DO.Task taskWithNewDate = _dal.Task.Read(idTask) with { ScheduledDate = scheduleDateTime, DeadlineDate = scheduleDateTime + _dal.Task.Read(idTask).RequiredEffortTime };
        _dal.Task.Update(taskWithNewDate);
        return;
    }

    //validity check
    /// <summary>
    /// Checks the validity of task data and throws an exception if invalid.
    /// </summary>
    /// <param name="task">The task data to check.</param>
    /// <exception cref="BlIncorrectInputException">Thrown when the input data is incorrect.</exception>
    private void checkData(BO.Task task)
    {
        string error = "";
        if (task.Id < 0)
            error = $"Id: {task.Id}";
        else if (task.Alias == "")
            error = $"Alias: {task.Alias}";
        else if (task.Engineer != null)
        {
            if ((int)task.Copmlexity > (int)_dal.Engineer.Read(task.Engineer.Id).level)
                error = "Task Complexity";
        }
        if (error != "")
            throw new BlIncorrectInputException($"{error}, is incorrect input");
    }

    /// <summary>
    /// Generates scheduled dates for all tasks in the project.
    /// </summary>
    /// <param name="startOfProject">The start date of the project.</param>
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
        // Enter a start and end date
        foreach (var task in tasksWithOutDependency)
        {
            //put the dates in dal
            startDateTimeManagement(task.Id, startOfProject);
        }

        //Finds everyone who has a dependency
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
    /// <summary>
    /// Initializes scheduled dates recursively for tasks with dependencies.
    /// </summary>
    /// <param name="task">The task to process.</param>
    /// <returns>The deadline date of the task.</returns>
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
                task.ScheduledDate = (DateTime.Compare(task.ScheduledDate.GetValueOrDefault(), DeadlineFromDepend.GetValueOrDefault()) > 0) ? task.ScheduledDate : DeadlineFromDepend;
                startDateTimeManagement(task.Id, task.ScheduledDate.GetValueOrDefault());
                task.DeadlineDate = task.ScheduledDate + task.RequiredEffortTime;//supporting function
            }
        }
        return null;
    }

    /// <summary>
    /// Reads all tasks with a specific status.
    /// </summary>
    /// <param name="status">The status to filter tasks by.</param>
    /// <returns>A list of tasks with the specified status.</returns>
    public List<BO.TaskInList>? ReadAllNew(BO.Status status)
    {
        IEnumerable<BO.TaskInList> tasks = ReadAll();

        List<BO.TaskInList> tasks1 = new List<TaskInList>();
        List<BO.TaskInList> tasks2 = new List<TaskInList>();
        foreach (BO.TaskInList item in tasks)
        {
            item.Status = (BO.Status)(Status(Read(item.Id)));
            tasks2.Add(item);
            if (item.Status == status)
                tasks1.Add(item);
        }
        if (status == 0)
            return tasks2;
        return tasks1;
    }

    /// <summary>
    /// Determines the status of a task.
    /// </summary>
    /// <param name="task">The task to evaluate.</param>
    /// <returns>The status code representing the task's status.</returns>
    public int Status(BO.Task task)
    {
        if (task.StartDate == null)
            return 1;
        if (task.CompleteDate == null)
            return 2;
        if (task.CompleteDate != null)
            return 4;
        if (task.ScheduledDate == null)
            return 0;
        // If no condition is met, consider it in some unknown status
        return 5;
    }

}
