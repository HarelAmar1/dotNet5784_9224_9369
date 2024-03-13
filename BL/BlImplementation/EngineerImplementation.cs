using BlApi;
using BO;
using DO;
using System.Net.Mail;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlImplementation;
internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = Factory.Get;


    /// <param name="engineerToAdd">Engineer to add to the DAL layer</param>
    /// <returns engineerToAdd.Id Returns the id of the added engineer ></returns>
    /// <exception cref="BlIncorrectInputException" Invalid input exception></exception>
    /// <exception cref="BlDoesNotExistException"  An exception does not exist></exception>
    public int Create(BO.Engineer engineerToAdd)
    {
        //Converts the list of engineers from DO to BO


        IEnumerable<BO.Engineer> engineers = (from item in _dal.Engineer.ReadAll().ToList()
                                              let tasks = _dal.Task.ReadAll()
                                              select new BO.Engineer()
                                              {
                                                  Id = item.Id,
                                                  Name = item.Name,
                                                  Cost = item.Cost,
                                                  Email = item.Email,
                                                  Level = (BO.EngineerExperience)(int)item.level,
                                                  Task = (from task in tasks
                                                          where (task.EngineerId == item.Id)
                                                          select new TaskInEngineer(task.Id, task.Alias)
                                                           ).FirstOrDefault()
                                              });

        //Validity checks that ID is a positive number and that NAME is a non-empty string and that COST is a positive number
        IEnumerable<DO.Task?> tasks1 = _dal.Task.ReadAll();

        //Correct email check
        try
        {
            var mailAddress = new MailAddress(engineerToAdd.Email);
        }
        catch (FormatException)
        {
            throw new BlIncorrectInputException($"Email={engineerToAdd.Email}, is incorrect input");
        }
        string error = "";
        if (engineerToAdd.Id < 0)
            error = $"Id: {engineerToAdd.Id}";
        else if (engineerToAdd.Name == "")
            error = $"Name: {engineerToAdd.Name}";
        else if (engineerToAdd.Cost < 0)
            error = $"Cost: {engineerToAdd.Cost}";
        else if ((int)engineerToAdd.Level >= 5)
            error = $"Level: {engineerToAdd.Level}";
        if (error != "")
            throw new BlIncorrectInputException($"{error}, is incorrect input");

        if (engineers.Any(engineer => engineer?.Id == engineerToAdd.Id) != null)
        {
            //convert the engineerToAdd from BO to DO
            if (engineerToAdd.Task != null)
            {
                DO.Task? taskToChangeInDal = (from task in tasks1
                                              where (task.Id == engineerToAdd.Task.Id)
                                              select (task)).FirstOrDefault();

                DO.Task taskToChangeInDalrecord = taskToChangeInDal with { EngineerId = engineerToAdd.Id };
                _dal.Task.Update(taskToChangeInDalrecord);
            }

            DO.Engineer becomeDO = new DO.Engineer(engineerToAdd.Id, engineerToAdd.Email, engineerToAdd.Cost, engineerToAdd.Name, (DO.EngineerExperience)(int)engineerToAdd.Level);
            _dal.Engineer.Create(becomeDO);
        }
        else
            throw new BlDoesNotExistException($"Engineer with ID: {engineerToAdd.Id} Does Not exists");



        return engineerToAdd.Id;

    }


    /// <param name="id" Get an engineer's id number ></param>
    /// <returns Returns an engineer according to the received id></returns>
    /// <exception cref="BlDoesNotExistException" An exception does not exist></exception>
    public BO.Engineer Read(int id)
    {
        //Converts the list of engineers from DO to BO


        List<DO.Task> tasks = _dal.Task.ReadAll().ToList();
        IEnumerable<BO.Engineer?> engineers = (from item in _dal.Engineer.ReadAll().ToList()
                                               select new BO.Engineer()
                                               {
                                                   Id = item.Id,
                                                   Name = item.Name,
                                                   Cost = item.Cost,
                                                   Email = item.Email,
                                                   Level = (BO.EngineerExperience)(int)item.level,
                                                   Task = (from task in tasks
                                                           where (task.EngineerId == item.Id)
                                                           select new TaskInEngineer(task.Id, task.Alias)
                                                           ).FirstOrDefault()
                                               });

        // Brings the engineer with the matching ID
        BO.Engineer? EngineerToGet = (from item in engineers
                                      where (item.Id == id)
                                      select item).FirstOrDefault();

        // Checks that the engineer is not empty


        if (EngineerToGet == null)
            throw new BlDoesNotExistException($"Engineer with ID: {id} Does Not exists");
        return EngineerToGet;

    }

    /// <param name="AnUpdatedEngineer" Gets an engineer who needs to change in the DAL layer></param>
    /// <exception cref="BlIncorrectInputException" Invalid input exception ></exception>
    /// <exception cref="BlDoesNotExistException"  An exception does not exist ></exception>
    public void Update(BO.Engineer AnUpdatedEngineer)
    {
        //We will check that the engineer does not have a Task that he has finished and now needs to be deleted
        if (AnUpdatedEngineer.Task == null)
        {
            //We will look for the engineer in the tasks
            var taskToUpdate = _dal.Task.ReadAll()
            .FirstOrDefault(T => T.EngineerId != null && T.EngineerId == AnUpdatedEngineer.Id);
            if (taskToUpdate != null)
            {
                var newTask = taskToUpdate with { EngineerId = null };
                _dal.Task.Update(newTask);
            }

        }

        //Converts the list of engineers from DO to BO
        IEnumerable<BO.Engineer?> engineers = (from item in _dal.Engineer.ReadAll().ToList()
                                               let tasks = _dal.Task.ReadAll()
                                               select new BO.Engineer()
                                               {
                                                   Id = item.Id,
                                                   Name = item.Name,
                                                   Cost = item.Cost,
                                                   Email = item.Email,
                                                   Level = (BO.EngineerExperience)(int)item.level,
                                                   Task = (from task in tasks
                                                           where (task.EngineerId == item.Id)
                                                           select new TaskInEngineer(task.Id, task.Alias)).FirstOrDefault()
                                               });

        BO.Engineer? EngineerToUp = (from item in engineers
                                     where (item.Id == AnUpdatedEngineer.Id)
                                     select item).FirstOrDefault();

        try
        {
            var mailAddress = new MailAddress(AnUpdatedEngineer.Email);
        }
        catch (FormatException)
        {
            throw new BlIncorrectInputException($"Email={AnUpdatedEngineer.Email}, is incorrect input");
        }
        string error = "";
        if (AnUpdatedEngineer.Id < 0)
            error = $"Id: {AnUpdatedEngineer.Id}";
        else if (AnUpdatedEngineer.Name == "")
            error = $"Name: {AnUpdatedEngineer.Name}";
        else if (AnUpdatedEngineer.Cost < 0)
            error = $"Cost: {AnUpdatedEngineer.Cost}";
        else if ((int)AnUpdatedEngineer.Level >= 5)
            error = $"Level: {AnUpdatedEngineer.Level}";
        if (error != "")
            throw new BlIncorrectInputException($"{error}, is incorrect input");

        if (AnUpdatedEngineer.Task != null)
        {

            // Brings the engineer with the matching ID


            if (EngineerToUp == null)
            {
                error = $"Id: {AnUpdatedEngineer.Id}";
                throw new BlDoesNotExistException($"{error} Does Not Exist");
            }

            //Uses the TaskImplementation instance to update the task in Dal

            IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();

            DO.Task taskToremoveInDal = (from task in tasks
                                         where (task.EngineerId == AnUpdatedEngineer.Id)
                                         select (task)).FirstOrDefault();
            if (taskToremoveInDal != null)
            {
                taskToremoveInDal = taskToremoveInDal with { EngineerId = null };
                _dal.Task.Update(taskToremoveInDal);
            }

            DO.Task taskToChangeInDal = (from task in tasks
                                         where (task.Id == AnUpdatedEngineer.Task.Id)
                                         select (task)).FirstOrDefault() with
            { EngineerId = AnUpdatedEngineer.Id };

            _dal.Task.Update(taskToChangeInDal);
        }
        //Checks whether the level of the existing engineer is greater than the updated one

        BO.EngineerExperience changeUpTheLevel = (int)EngineerToUp.Level > (int)AnUpdatedEngineer.Level ? EngineerToUp.Level : AnUpdatedEngineer.Level;

        DO.Engineer becomeDO = new DO.Engineer(AnUpdatedEngineer.Id, AnUpdatedEngineer.Email, AnUpdatedEngineer.Cost, AnUpdatedEngineer.Name, (DO.EngineerExperience)(int)changeUpTheLevel);

        //Save the updated engineer and task in the DAL layer

        _dal.Engineer.Update(becomeDO);
    }


   
    /// <param name="id" Gets an ID that needs to be deleted from the DAL layer></param>
    /// <exception cref="BlDoesNotExistException" An exception does not exist ></exception>
    /// <exception cref="BlCanNotBeDeletedException" An engineer's anomaly that cannot be erased ></exception>
    /// <exception cref="BlIncorrectInputException" Invalid input exception ></exception>
    public void Delete(int id)
    {
        //bring the list of engineers from DO

        IEnumerable<BO.Engineer?> engineers = (from item in _dal.Engineer.ReadAll().ToList()
                                               let tasks = _dal.Task.ReadAll()
                                               select new BO.Engineer()
                                               {
                                                   Id = item.Id,
                                                   Name = item.Name,
                                                   Cost = item.Cost,
                                                   Email = item.Email,
                                                   Level = (BO.EngineerExperience)(int)item.level,
                                                   Task = (from task in tasks
                                                           where (task.EngineerId == item.Id)
                                                           select new TaskInEngineer(task.Id, task.Alias)
                                                           ).FirstOrDefault()
                                               });

        string error = "";


        //Checks if the ID is positive

        if (id >= 0)
        {
            //Checks if the engineer with the ID exists

            if (engineers.Any(engineer => engineer?.Id == id) == false)
                throw new BlDoesNotExistException($"Engineer with ID: {id} Does Not Exist");
            else
            {
                //Brings the engineer with the same ID that I need to delete

                BO.Engineer? toDelete = (from item in engineers
                                         where (item.Id == id)
                                         select item).FirstOrDefault();

                IBl temp = new Bl();
                TaskImplementation toCheckStatus = new TaskImplementation(temp);

                //Checks if the engineer has already finished performing a task or is actively performing a task
                IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();

                if ((BO.Status)(toCheckStatus.Read(toDelete.Task.Id).Status) != BO.Status.Done && (BO.Status)(toCheckStatus.Read(toDelete.Task.Id).Status) != BO.Status.OnTrack)
                {

                    //delete from The Dal layer

                    DO.Task taskToremoveInDal = (from task in tasks
                                                 where (task.EngineerId == id)
                                                 select (task)).FirstOrDefault() with
                    { EngineerId = null };

                    _dal.Task.Update(taskToremoveInDal);
                    _dal.Engineer.Delete(toDelete.Id);
                }
                else
                    throw new BlCanNotBeDeletedException($"Id: {toDelete.Id} Can Not Be Deleted");
            }
        }
        else
            error = $"Id: {id}";
        if (error != "")
            throw new BlIncorrectInputException($"{error}, is incorrect input");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter" Selection of a list of engineers according to a certain filter></param>
    /// <returns Returns a list of engineers according to the filter></returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<DO.Engineer?, bool>? filter = null)
    {
        IEnumerable<BO.Engineer?> engineers = (from item in _dal.Engineer.ReadAll(filter).ToList()
                                               let tasks = _dal.Task.ReadAll()
                                               select new BO.Engineer()
                                               {
                                                   Id = item.Id,
                                                   Name = item.Name,
                                                   Cost = item.Cost,
                                                   Email = item.Email,
                                                   Level = (BO.EngineerExperience)(int)item.level,
                                                   Task = (from task in tasks
                                                           where (task.EngineerId == item.Id)
                                                           select new TaskInEngineer(task.Id, task.Alias)).FirstOrDefault()
                                               });
        return engineers;
    }
}