using BlApi;
using BO;
using DalApi;
namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = Factory.Get;
    public int Create(BO.Engineer engineerToAdd)
    {
        //Converts the list of engineers from DO to BO

        IEnumerable<BO.Engineer> engineers = (from item in _dal.Engineer.ReadAll().ToList()
                                              select new BO.Engineer()
                                              {
                                                  Id = item.Id,
                                                  Name = item.Name,
                                                  Cost = item.Cost,
                                                  Email = item.Email
                                              });

        //Validity checks that ID is a positive number and that NAME is a non-empty string and that COST is a positive number

        string error = "";
        if (engineerToAdd.Id >= 0)
            error = $"Id={engineerToAdd.Id}";
        else
             if (engineerToAdd.Name != "")
            error = $"Name={engineerToAdd.Name}";
        else
             if (engineerToAdd.Cost >= 0)
            error = $"Cost={engineerToAdd.Cost}";
        else
        if (engineers.Any(engineer => engineer?.Id == engineerToAdd.Id) != null)
        {

            //convert the engineerToAdd from BO to DO

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

        IEnumerable<BO.Engineer?> engineers = (from item in _dal.Engineer.ReadAll().ToList()
                                               select new BO.Engineer()
                                               {
                                                   Id = item.Id,
                                                   Name = item.Name,
                                                   Cost = item.Cost,
                                                   Email = item.Email
                                               });

        // Brings the engineer with the matching ID
        BO.Engineer? EngineerToGet = (from item in engineers
                                      where (item.Id == id)
                                      select item).FirstOrDefault();

        // Checks that the engineer is not empty


        if (EngineerToGet == null)
            throw new BlDoesNotExistException($"Engineer with ID={EngineerToGet.Id} Does Not exists");
        else
        {

            //Adds the methods Task

            TaskImplementation ExtractTheTask = new TaskImplementation();
            BO.TaskInEngineer TaskForEngineerToGet = new BO.TaskInEngineer(ExtractTheTask.Read(EngineerToGet.Id).Engineer.Id, ExtractTheTask.Read(EngineerToGet.Id).Engineer.Name);
            EngineerToGet.Task = TaskForEngineerToGet;
        }
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
        if (AnUpdatedEngineer.Id >= 0)
            error = $"Id={AnUpdatedEngineer.Id}";
        else
             if (AnUpdatedEngineer.Name != "")
            error = $"Name={AnUpdatedEngineer.Name}";
        else
             if (AnUpdatedEngineer.Cost >= 0)
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

                throw new BlDoesNotExistException(error);
            }
            else
            {
                //Uses the TaskImplementation instance to update the task in Dal
                IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();

                DO.Task taskToUpdate = (from item in tasks
                                        where item.Id == EngineerToUp.Task.Id
                                        select item).FirstOrDefault();


                //Checks whether the level of the existing engineer is greater than the updated one

                BO.EngineerExperience changeUpTheLevel = (int)EngineerToUp.Level > (int)AnUpdatedEngineer.Level ? EngineerToUp.Level : AnUpdatedEngineer.Level;

                DO.Engineer becomeDO = new DO.Engineer(AnUpdatedEngineer.Id, AnUpdatedEngineer.Email, AnUpdatedEngineer.Cost, AnUpdatedEngineer.Name, false, (DO.EngineerExperience)(int)changeUpTheLevel);

                //Save the updated engineer and task in the DAL layer

                _dal.Engineer.Update(becomeDO);
                _dal.Task.Update(taskToUpdate);
            }

        }
        throw new BlIncorrectInputException($"{error}, is incorrect input");

    }

    public void Delete(int id)
    {
        //bring the list of engineers from DO
        IEnumerable<BO.Engineer?> engineers = (from item in _dal.Engineer.ReadAll().ToList()
                                               select new BO.Engineer()
                                               {
                                                   Id = item.Id,
                                                   Name = item.Name,
                                                   Cost = item.Cost,
                                                   Email = item.Email
                                               });
        //Checks if the ID is positive

        if (id >= 0)
        {
            //Checks if the engineer with the ID exists

            if (engineers.Any(engineer => engineer?.Id != id)) ;
            //  throw new DalAlreadyExistsException($"Engineer with ID={id} already exists");
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

                    _dal.Engineer.Delete(toDelete.Id);
                }
                else
                  throw new Bl;
            }
        }
        else;
        //  throw new DalAlreadyExistsException($"Engineer with ID={id} already exists");
    }
    public IEnumerable<BO.EngineerInTask> ReadAll(Func<DO.Engineer?, bool>? filter = null)
    {

        EngineerImplementation toCheckTask = new EngineerImplementation();

        //Converts the list of engineers from DO to BO by filter

        IEnumerable<BO.EngineerInTask?> engineers = (from item in _dal.Engineer.ReadAll().ToList()
                                                     where filter(item)
                                                     select new BO.EngineerInTask()
                                                     {
                                                         Id = item.Id,
                                                         Name = item.Name,

                                                     });
        return engineers;
    }

}