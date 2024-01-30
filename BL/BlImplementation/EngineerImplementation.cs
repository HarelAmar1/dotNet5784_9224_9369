using BlApi;
using BO;
using DO;
using System;
using System.Diagnostics.Contracts;
using System.Net.Security;
using System.Reflection.Emit;

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

        if (engineerToAdd.Id >= 0 && engineerToAdd.Name != "" && engineerToAdd.Cost >= 0)
        {

            //  Checks if the ID is already in the list

            if (engineers.Any(engineer => engineer?.Id == engineerToAdd.Id) != null)
            {

                //convert the engineerToAdd from BO to DO

                DO.Engineer becomeDO = new DO.Engineer(engineerToAdd.Id, engineerToAdd.Email, engineerToAdd.Cost, engineerToAdd.Name, false, (DO.EngineerExperience)(int)engineerToAdd.Level);
                _dal.Engineer.Create(becomeDO);
                return engineerToAdd.Id;
            }
            else
            {
                //  throw new DalAlreadyExistsException($"Engineer with ID={engineerToAdd.Id} already exists");
            }
        }
        else;
        //  throw new DalAlreadyExistsException($"Engineer with ID={engineerToAdd.Id} already exists");
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

        //Adds the methods Task

        TaskImplementation ExtractTheTask = new TaskImplementation();
        EngineerToGet.Task = ExtractTheTask.Read(EngineerToGet.Id);
        // Checks that the engineer is not empty

        if (EngineerToGet == null) ;
        //  throw new DalAlreadyExistsException($"Engineer with ID={engineerToAdd.Id} already exists");
        else
            return EngineerToGet;
    }


}
public void Update(BO.Engineer engineerToUpdate)
{
    IEnumerable<BO.Engineer> engineers = (from item in _dal.Engineer.ReadAll().ToList()
                                          select new BO.Engineer()
                                          {
                                              Id = item.Id,
                                              Name = item.Name,
                                              Cost = item.Cost,
                                              Email = item.Email
                                          });

    BO.Engineer? EngineerToUp = (from item in engineers
                                 where (item.Id == engineerToUpdate.Id)
                                 select item).FirstOrDefault();
    DO.Engineer becomeDO = new DO.Engineer(engineerToUpdate.Id, engineerToUpdate.Email, engineerToUpdate.Cost, engineerToUpdate.Name, false, (DO.EngineerExperience)(int)engineerToUpdate.Level);
    _dal.Engineer.Update(becomeDO);

}


public void Delete(int id)
{
    //bring the list of engineers from DO
    IEnumerable<DO.Engineer?> engineers = _dal.Engineer.ReadAll().ToList();

    //Checks if the ID is positive

    if (id >= 0)
    {
        //Checks if the engineer with the ID exists

        if (engineers.Any(engineer => engineer?.Id != id)) ;
        //  throw new DalAlreadyExistsException($"Engineer with ID={id} already exists");
        else
        {
            //Brings the engineer with the same ID that I need to delete

            DO.Engineer? toDelete = (from item in engineers where (item.Id == id) select item).FirstOrDefault();

            TaskImplementation toCheckStatus = new TaskImplementation();

            //Checks if the engineer has already finished performing a task or is actively performing a task

            if ((BO.Status)(toCheckStatus.Read(toDelete.Id).Status) != BO.Status.Done || (BO.Status)(toCheckStatus.Read(toDelete.Id).Status) != BO.Status.OnTrack)
            {
                engineers.ToList().Remove(toDelete);
            }
            else;
            //  throw new DalAlreadyExistsException($"Engineer with ID={id} already exists");

        }
    }
    //  throw new DalAlreadyExistsException($"Engineer with ID={id} already exists");
}
public IEnumerable<BO.EngineerInTask> ReadAll(Func<DO.Task?, bool>? func = null)
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

}
}