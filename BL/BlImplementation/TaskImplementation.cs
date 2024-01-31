using BlApi;
using System.Security.Cryptography;

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
        List<DO.Dependency> dependlist = (List<DO.Dependency>)_dal.Dependency.ReadAll(null);
        bool check = true;
        foreach (DO.Dependency dep in dependlist) 
        {
            if (dep.DependsOnTask == task.Id)
                check = false;
        }
        if (check == true) 
        {
            //למחוק אותו
            _dal.Task.Delete(idTask);
        }
        else;
            //לזרוק שגיאה
        


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

    public void startDateTimeManagement(int IdTask, DateTime dateTime)
    {
        //נביא את הרשימה של המשימות מהדל
        var depenList = _dal.Dependency.ReadAll();
        foreach (var depen in depenList) 
        {
            //נחפש את המשימה הנוכחית ברשימת התלויות
            if (depen.DependentTask == IdTask) 
            {
                //ואחרי שמצאנו אותה נבדוק האם למשימה התלותית בה יש תאריך התחלה
                if (Read(depen.DependsOnTask.Value).ScheduledDate != null)
                {
                    //במידה והתאריך הנתון קטן מהתאריך של המשימה התלותית נזרוק שגיאה
                    if(dateTime.CompareTo(Read(depen.DependsOnTask.Value).ScheduledDate) == -1)
                    {
                        //זרוק שגיאה
                    }
                }
                else
                {
                    //זרוק חריגה
                }
            }
        }

    }

    public void Update(BO.Task task)
    {
        //בדיקת נתונים
        if (task.Id >= 0 || task.Alias != "")
            return;//לזרוק שגיאה   
        //נבדוק אם קיים ברשימה
        if (Read(task.Id) != null)
        {
            Delete(task.Id);
            Create(task);
        }
        //else
        //{
        //    //לזרוק שגיאה
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

