using BlApi;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;

    public int Create(BO.Task task)
    {
        if (task.Id >= 0 || task.Alias != "")
            return -1;//לזרוק שגיאה
        //לבדוק מה הכוונה להוסיף תלויות

        //ניצור את המשימה ונעביר אותה לדאל
        DO.Task newTask = new DO.Task
        (task.Id, task.Description, task.Alias, false, DateTime.Now, task.RequiredEffortTime,
        (DO.EngineerExperience)task.Copmlexity!, task.StartDate, task.ScheduledDate, task.DeadlineDate, task.CompleteDate,
        task.Deliverables, task.Remarks, task.Engineer!.Id);
        try
        {
            _dal.Task.Create(newTask);
            return task.Id;
        }
        catch(DO.DalAlreadyExistsException ex)
        {
            //throw new BO.BlAlreadyExistsException(); לחכות שיהיה חריגות
        }
    }
    

    public void Delete(int idTask)
    {
        BO.Task task = Read(idTask);
        //לבדוק שאין משימות שתלויות במשימה זו
        for (int i = 0;i < task.)

    }
    public BO.Task Read(int idTask)
    {
        //נוציא את הנתון מהדל
        DO.Task? task = _dal.Task.Read(idTask);
        EngineerImplementation lookForNameOfEngineer = new EngineerImplementation();
        string name = lookForNameOfEngineer.Read(idTask).Name;
        BO.EngineerInTask engineerInTask = new BO.EngineerInTask() { Id = task.Id, Name = name };

        BO.Task taskToRead = new BO.Task()
        {
            Id = task.Id,
            Description = task.Description,
            Alias = task.Alias,
            CreatedAtDate = task.CreatedAtDate,
            Status = (BO.Status)status(task),
            Dependencies = new List<BO.TaskInList>(),//לבדוק מה להכניס פה
            RequiredEffortTime = task.RequiredEffortTime,
            StartDate = task.StartDate,
            ScheduledDate = task.ScheduledDate,
            ForecastDate = null,//לבדוק מה להכניס פה
            DeadlineDate = task.DeadlineDate,
            CompleteDate = task.CompleteDate,
            Deliverables = task.Deliverables,
            Remarks = task.Remarks,
            Engineer = engineerInTask,//לבדוק מה להכניס פה
            Copmlexity = (BO.EngineerExperience)task.Copmlexity
        };
        return taskToRead;
    }


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

    public void startDateTimeManagement(BO.Task task)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Task task)
    {
        throw new NotImplementedException();
    }




    //פונקצית עזר למציאת הסטטסוס של המשימה מדאל
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
        //לבדוק איך לבדוק את הסטטוס האחרון
        return 0;
    }
}







