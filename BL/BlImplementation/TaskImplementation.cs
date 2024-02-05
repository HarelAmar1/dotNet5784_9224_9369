using BlApi;
using BO;
using DO;
using System.Xml.Linq;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;


    //Create
    public int Create(BO.Task task)
    {   
        //נבדוק תקינות של האיי די והכינוי
        checkData(task);

        //להוסיף תלויות
        foreach (var item in task.Dependencies)
        {
            //בתלות יש משימה אחת ומשימה שהאחת תלויה בה 
            //לכן ניצור תלות חדשה עבור כל איבר שברשימה של המשימה הנוכחית ונכניס אותה לרשימת התלויות בנתונים
            Dependency depend = new Dependency(0, task.Id, item.Id);
            _dal.Dependency.Create(depend);
        }
        
        //ניצור את המשימה ונעביר אותה לדאל
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

        //נעשה ניסיון להכניס אותו לנתונים
        try
        {
            _dal.Task.Create(newTask);
            return task.Id;
        }
        catch(DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={task.Id} already exist", ex);
        }
    }


    //Delete
    public void Delete(int idTask)
    {
        //נמצא את המשימה שאנחנו רוצים למחוק
        BO.Task? task = Read(idTask);
        if (task == null)
            throw new BO.BlDoesNotExistException($"Task with ID ={idTask} is not exists");

        //נבדוק שאין משימות שתלויות במשימה זו
        List<DO.Dependency> dependlist = (List<DO.Dependency>)_dal.Dependency.ReadAll(null);
        bool check = true;
        foreach (DO.Dependency dep in dependlist)
        {
            if (dep.DependsOnTask == task.Id)
                check = false;
        }

        //למחוק אותו
        if (check == true)
        {
            _dal.Task.Delete(idTask);

            //למחוק מהתלויות שבדל את המשימות הקשורות אליו
            foreach (DO.Dependency dep in dependlist)
            {
                if (dep.DependentTask == task.Id)
                    _dal.Dependency.Delete(dep.Id);
            }

        }
        //אחרת נזרוק שגיאה
        else
            throw new BlDeletionImpossible("The task depends on other tasks");

    }

    //Read
    public BO.Task Read(int idTask)
    {
        try
        {
            //נוציא את הנתון מהדל
            DO.Task? task = _dal.Task.Read(idTask);
            if(task == null)
                throw new BO.BlDoesNotExistException($"Task with ID ={idTask} is not exists");
            //ניצור את השדה מהנדס במשימה
            BO.EngineerInTask engineerInTask = null;
            EngineerImplementation lookForNameOfEngineer = new EngineerImplementation();
            IEnumerable<BO.Engineer> engineers = lookForNameOfEngineer.ReadAll();
            foreach(var E in engineers)
            {
                if (E.Task.Id == idTask)
                {
                    engineerInTask = new BO.EngineerInTask() { Id = E.Id, Name = E.Name };
                }
            }

            //ניצור את השדה של כל התלויות של המשימה הנתונה
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
                ForecastDate = null,//לבדוק מה להכניס פה
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
    public void startDateTimeManagement(int idTask, DateTime dateTime)
    {
        //נביא את הרשימה של המשימות מהדל
        var depenList = _dal.Dependency.ReadAll();
        foreach (var depen in depenList) 
        {
            //נחפש את המשימה הנוכחית ברשימת התלויות
            if (depen.DependentTask == idTask) 
            {
                //ואחרי שמצאנו אותה נבדוק האם למשימה התלותית בה יש תאריך התחלה
                if (Read(depen.DependsOnTask.Value).ScheduledDate != null)
                {
                    //במידה והתאריך הנתון קטן מהתאריך של המשימה התלותית נזרוק שגיאה
                    if(dateTime.CompareTo(Read(depen.DependsOnTask.Value).ScheduledDate) == -1)
                        throw new BlInvalidDatesException("Invalid Dates");   
                }
                else
                    throw new BlInvalidDatesException("Invalid Dates");
                
            }
            //אין בעיות עם התאריכים ולכן אפשר לעדכן את התאריך התחלה במשימה
            DO.Task taskWithNewDate = _dal.Task.Read(idTask) with { CreatedAtDate = dateTime };
            _dal.Task.Update(taskWithNewDate);
        }

    }

    //Update
    public void Update(BO.Task task)
    {
        //נבדוק תקינות של האיי די והכינוי
        checkData(task);
           
        //נמחק את כל התלויות שקשורות למשימה שקיבלנו
        foreach(var depend in _dal.Dependency.ReadAll())
        {
            if (depend.DependentTask == task.Id)
                _dal.Dependency.Delete(depend.Id);
        }

        //נוסיף אותם חזרה מעודכנים
        foreach (var depend in task.Dependencies)
        {
            DO.Dependency insertUpdate = new DO.Dependency(0, task.Id, depend.Id);
            _dal.Dependency.Create(insertUpdate);
        }

        //נמלא את שאר השדות למשימה של דאל ונחזיר מעודכן
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

        //נעדכן בדאל
        _dal.Task.Update(newTask);
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

    //בדיקת תקינות
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
}

