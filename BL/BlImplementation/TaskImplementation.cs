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

        //add dependencies
        foreach (var item in task.Dependencies)
        {
            //in a dependency there is one task and a task that that one depends on
            //therefore we will create a new dependency for each member in the list of the current task and insert it into the list of data dependencies
            Dependency depend = new Dependency(0, task.Id, item.Id);
            _dal.Dependency.Create(depend);
        }

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
        task.Engineer!.Id);

        // an attempt will be made to insert it into the data
        try
        {
            _dal.Task.Create(newTask);
            return task.Id;
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
        bool check = true;
        foreach (DO.Dependency dep in dependlist)
        {
            if (dep.DependsOnTask == task.Id)
                check = false;
        }

        //delete it
        if (check == true)
        {
            _dal.Task.Delete(idTask);

            //delete from the dependencies that separate the tasks related to it
            foreach (DO.Dependency dep in dependlist)
            {
                if (dep.DependentTask == task.Id)
                    _dal.Dependency.Delete(dep.Id);
            }

        }
        //Otherwise we will throw an error
        else
            throw new BlDeletionImpossible("The task depends on other tasks");

    }

    //Read
    public BO.Task Read(int idTask)
    {
        try
        {
            //get the data from the DAL
            DO.Task? task = _dal.Task.Read(idTask);
            if (task == null)
                throw new BO.BlDoesNotExistException($"Task with ID ={idTask} is not exists");
            //we will create the engineer field in the task
            BO.EngineerInTask engineerInTask = null;
            EngineerImplementation lookForNameOfEngineer = new EngineerImplementation();
            IEnumerable<BO.Engineer> engineers = lookForNameOfEngineer.ReadAll();
            foreach (var E in engineers)
            {
                if (E.Task.Id == idTask)
                {
                    engineerInTask = new BO.EngineerInTask() { Id = E.Id, Name = E.Name };
                }
            }

            //We will create the field of all dependencies of the given task
            List<DO.Dependency?> depenFromDal = (List<DO.Dependency?>)_dal.Dependency.ReadAll();
            List<BO.TaskInList> newForDepend = new List<BO.TaskInList>();
            foreach (var item in depenFromDal)
            {
                if (item.DependentTask == idTask)
                {
                    DO.Task findDependTask = _dal.Task.Read(item.DependsOnTask.Value); //find the task from dal 
                    BO.TaskInList taskInList = new BO.TaskInList()//create taskInList
                    {
                        Id = findDependTask.Id,
                        Description = findDependTask.Description,
                        Alias = findDependTask.Alias,
                        Status = (BO.Status)status(findDependTask)
                    };
                    newForDepend.Add(taskInList);
                }
            }

            BO.Task taskToRead = new BO.Task()
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
            throw new BO.BlDoesNotExistException($"Task with ID ={idTask} is not exists", ex);
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
        //get the list from the DAL
        var depenList = _dal.Dependency.ReadAll();
        bool check = false;
        foreach (var depen in depenList)
        {
            //we will look for the current task in the dependent list
            if (depen.DependentTask == idTask)
            {
                check = true;
                //And after we have found it, we will check whether the task dependent on it has a start date
                if (Read(depen.DependsOnTask.Value).ScheduledDate != null)
                {
                    //if the given date is less than the date of the dependent task we will throw an error
                    if (scheduleDateTime.CompareTo(Read(depen.DependsOnTask.Value).ScheduledDate) == -1)
                        throw new BlInvalidDatesException("Invalid Dates");
                }
                else
                    throw new BlInvalidDatesException("Invalid Dates");

            }
            //There are no problems with the dates, so you can update the ScheduledDate and DeadlineDate in the task
            if (check == true)
            {

            }
        }

    }

    //Update
    public void Update(BO.Task task)
    {
        //we will check the correctness of the ID and nickname
        checkData(task);

        // We will delete all the dependencies related to the task we received
        foreach (var depend in _dal.Dependency.ReadAll())
        {
            if (depend.DependentTask == task.Id)
                _dal.Dependency.Delete(depend.Id);
        }

        //we will add them back updated
        foreach (var depend in task.Dependencies)
        {
            DO.Dependency insertUpdate = new DO.Dependency(0, task.Id, depend.Id);
            _dal.Dependency.Create(insertUpdate);
        }

        //Fill in the rest of the fields for the DAL task and return updated
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
        task.Engineer!.Id);

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
        if (task.Id < 0 || task.Alias == "")
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
            var a = Read(dep.Id);
            DateTime? temp = initScheduledDateRecursive(a);
            if (task.ScheduledDate == null)
            {
                startDateTimeManagement(task.Id, temp.GetValueOrDefault());
                task.ScheduledDate = temp;//supporting function
                task.DeadlineDate = temp + task.RequiredEffortTime;//supporting function
            }
            else
            {
                task.ScheduledDate = (DateTime.Compare(task.DeadlineDate.GetValueOrDefault(), temp.GetValueOrDefault()) > 0) ? task.ScheduledDate : temp;
                startDateTimeManagement(task.Id, task.ScheduledDate.GetValueOrDefault());
                task.DeadlineDate = task.ScheduledDate + task.RequiredEffortTime;//supporting function
            }
        }
        return null;
    }
}
