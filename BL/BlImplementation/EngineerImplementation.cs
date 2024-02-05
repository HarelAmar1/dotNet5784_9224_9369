using BlApi;
using BO;
using DO;
namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = Factory.Get;
    public int Create(BO.Engineer engineerToAdd)
    {
        //Converts the list of engineers from DO to BO

        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();

        IEnumerable<BO.Engineer> engineers = (from item in _dal.Engineer.ReadAll().ToList()
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

        string error = "";
        if (engineerToAdd.Id < 0)
            error = $"Id={engineerToAdd.Id}";
        else
             if (engineerToAdd.Name == "")
            error = $"Name={engineerToAdd.Name}";
        else
             if (engineerToAdd.Cost < 0)
            error = $"Cost={engineerToAdd.Cost}";
        else
        if (engineers.Any(engineer => engineer?.Id == engineerToAdd.Id) != null)
        {

            //convert the engineerToAdd from BO to DO
            DO.Task taskToChangeInDal = (from task in tasks
                                         where (task.Id == engineerToAdd.Task.Id)
                                         select (task)).FirstOrDefault();

            DO.Task taskToChangeInDalrecord = taskToChangeInDal with { EngineerId = engineerToAdd.Id };
            _dal.Task.Update(taskToChangeInDalrecord);
            DO.Engineer becomeDO = new DO.Engineer(engineerToAdd.Id, engineerToAdd.Email, engineerToAdd.Cost, engineerToAdd.Name, false, (DO.EngineerExperience)(int)engineerToAdd.Level);
            _dal.Engineer.Create(becomeDO);
        }
        else
            throw new BlDoesNotExistException($"Engineer with ID={engineerToAdd.Id} Does Not exists");

        if (error != "")
            throw new BlIncorrectInputException($"{error}, is incorrect input");

        return engineerToAdd.Id;

    }
    public BO.Engineer Read(int id)
    {
        //Converts the list of engineers from DO to BO
        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();

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
            throw new BlDoesNotExistException($"Engineer with ID={EngineerToGet.Id} Does Not exists");
        return EngineerToGet;

    }
    public void Update(BO.Engineer AnUpdatedEngineer)
    {


        //Converts the list of engineers from DO to BO

        IEnumerable<BO.Engineer?> engineers = (from item in _dal.Engineer.ReadAll().ToList()
                                               select new BO.Engineer()
                                               {
                                                   Id = item.Id,
                                                   Name = item.Name,
                                                   Cost = item.Cost,
                                                   Email = item.Email
                                               });

        //Validity checks that ID is a positive number 


        string error = "";
        if (AnUpdatedEngineer.Id < 0)
            error = $"Id={AnUpdatedEngineer.Id}";
        else
             if (AnUpdatedEngineer.Name == "")
            error = $"Name={AnUpdatedEngineer.Name}";
        else
             if (AnUpdatedEngineer.Cost < 0)
            error = $"Cost={AnUpdatedEngineer.Cost}";
        else
        {

            // Brings the engineer with the matching ID

            BO.Engineer? EngineerToUp = (from item in engineers
                                         where (item.Id == AnUpdatedEngineer.Id)
                                         select item).FirstOrDefault();
            if (EngineerToUp == null)
            {
                error = $"Id={AnUpdatedEngineer.Id}";
                throw new BlDoesNotExistException($"{error} Does Not Exist");
            }
            else
            {

                //Uses the TaskImplementation instance to update the task in Dal
                if (EngineerToUp.Task != AnUpdatedEngineer.Task)
                {
                    IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();

                    DO.Task taskToremoveInDal = (from task in tasks
                                                 where (task.EngineerId == AnUpdatedEngineer.Id)
                                                 select (task)).FirstOrDefault() with
                                                                                     { EngineerId = null };

                    _dal.Task.Update(taskToremoveInDal);

                    DO.Task taskToChangeInDal = (from task in tasks
                                                 where (task.Id == AnUpdatedEngineer.Task.Id)
                                                 select (task)).FirstOrDefault() with
                                                                                      { EngineerId = AnUpdatedEngineer.Id };

                    _dal.Task.Update(taskToChangeInDal);
                }
                //Checks whether the level of the existing engineer is greater than the updated one

                BO.EngineerExperience changeUpTheLevel = (int)EngineerToUp.Level > (int)AnUpdatedEngineer.Level ? EngineerToUp.Level : AnUpdatedEngineer.Level;

                DO.Engineer becomeDO = new DO.Engineer(AnUpdatedEngineer.Id, AnUpdatedEngineer.Email, AnUpdatedEngineer.Cost, AnUpdatedEngineer.Name, true, (DO.EngineerExperience)(int)changeUpTheLevel);

                //Save the updated engineer and task in the DAL layer

                _dal.Engineer.Update(becomeDO);
            }

        }
        if (error != "")
            throw new BlIncorrectInputException($"{error}, is incorrect input");
    }

    public void Delete(int id)
    {
        //bring the list of engineers from DO
        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();

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

        string error = "";


        //Checks if the ID is positive

        if (id >= 0)
        {
            //Checks if the engineer with the ID exists

            if (engineers.Any(engineer => engineer?.Id == id) == null)
                throw new BlDoesNotExistException($"Engineer with ID={id} already exists");
            else
            {
                //Brings the engineer with the same ID that I need to delete

                BO.Engineer? toDelete = (from item in engineers
                                         where (item.Id == id)
                                         select item).FirstOrDefault();

                TaskImplementation toCheckStatus = new TaskImplementation();

                //Checks if the engineer has already finished performing a task or is actively performing a task

                if ((BO.Status)(toCheckStatus.Read(toDelete.Id).Status) != BO.Status.Done && (BO.Status)(toCheckStatus.Read(toDelete.Id).Status) != BO.Status.OnTrack)
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
                    throw new BlCanNotBeDeletedException($"Id={toDelete.Id} Can Not Be Deleted");
            }
        }
        else
            error = $"Id={id}";
        if (error != "")
            throw new BlIncorrectInputException($"{error}, is incorrect input");


    }
    public IEnumerable<BO.Engineer> ReadAll(Func<DO.Engineer?, bool>? filter = null)
    {
        //Converts the list of engineers from DO to BO by filter

        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();

        IEnumerable<BO.Engineer?> engineers = (from item in _dal.Engineer.ReadAll(filter).ToList()
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
        return engineers;
    }
}
