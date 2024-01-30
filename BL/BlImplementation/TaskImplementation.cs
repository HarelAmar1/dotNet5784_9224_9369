using BlApi;
using System.Xml.Linq;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;

    public void Create(BO.Task task)
    {
        if (task.Id >= 0 || task.Alias != "")
            return;

        //לבדוק מה הכוונה להוסיף תלויות

        DO.Task newTask = new DO.Task(task.Id,task.Description,task.Alias,false,DateTime.Now,task.RequiredEffortTime,
        (DO.EngineerExperience)task.Copmlexity, 
            );
        _dal.Task.Create()

    }

    public void Delete(int idTask)
    {
        throw new NotImplementedException();
    }

    public Task Read(int idTask)
    {
        throw new NotImplementedException();
    }

    //פונקצית עזר למציאת הסטטסוס של המשימה
    private BO.Status status(DO.Task task)
    {
        if (task.ScheduledDate == null)
            return (BO.Status)0;
        if (task.StartDate == null)
            return (BO.Status)1;
        if (task.CompleteDate == null)
            return (BO.Status)2;
        if (task.CompleteDate != null) 
            return (BO.Status)3;
        //לבדוק איך לבדוק את הסטטוס האחרון
        return 0;
    }

    public IEnumerable<BO.TaskInList> ReadAll(Func<DO.Task?, bool>? func = null)
    {
        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll(func).ToList();

        IEnumerable<BO.TaskInList> boTasks =
        from task in tasks
        select new BO.TaskInList()
        {
            Id = task.Id,
            Description = task.Description,
            Alias = task.Alias
            //Status = status()
        };
        return boTasks;
    }

    public void startDateTimeManagement(BO.Task task)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Task task)
    {
        throw new NotImplementedException();
    }
}

