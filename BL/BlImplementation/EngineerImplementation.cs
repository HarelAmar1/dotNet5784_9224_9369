using BlApi;
using BO;
using DalApi;
using DO;
using System;
using System.Diagnostics.Contracts;
using System.Net.Security;
using System.Reflection.Emit;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

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

        // Checks that the engineer is not empty


        if (EngineerToGet == null) ;
        //  throw new DalAlreadyExistsException($"Engineer with ID={engineerToAdd.Id} already exists");
        else
        {

            //Adds the methods Task

            TaskImplementation ExtractTheTask = new TaskImplementation();
            BO.TaskInEngineer TaskForEngineerToGet = new BO.TaskInEngineer(ExtractTheTask.Read(EngineerToGet.Id).Engineer.Id, ExtractTheTask.Read(EngineerToGet.Id).Engineer.Name);
            EngineerToGet.Task = TaskForEngineerToGet;
            return EngineerToGet;
        }
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


        if (AnUpdatedEngineer.Id >= 0)
        {

            // Brings the engineer with the matching ID

            BO.Engineer? EngineerToUp = (from item in engineers
                                         where (item.Id == AnUpdatedEngineer.Id)
                                         select item).FirstOrDefault();
            if (EngineerToUp == null) ;
            //  throw new DalAlreadyExistsException($"Engineer with ID={engineerToAdd.Id} already exists");
            else
            {

                // Validity checks and that NAME is a non - empty string and that COST is a positive number

                if (AnUpdatedEngineer.Name != "" && AnUpdatedEngineer.Cost >= 0)
                {

                    //Uses the TaskImplementation instance to update the task in Dal

                    TaskImplementation ExtractTheTask = new TaskImplementation();
                    ExtractTheTask.Update(ExtractTheTask.Read(AnUpdatedEngineer.Id));

                    //Checks whether the level of the existing engineer is greater than the updated one

                    BO.EngineerExperience changeUpTheLevel = (int)EngineerToUp.Level > (int)AnUpdatedEngineer.Level ? EngineerToUp.Level : AnUpdatedEngineer.Level;

                    DO.Engineer becomeDO = new DO.Engineer(AnUpdatedEngineer.Id, AnUpdatedEngineer.Email, AnUpdatedEngineer.Cost, AnUpdatedEngineer.Name, false, (DO.EngineerExperience)(int)changeUpTheLevel);

                    //Save the updated engineer in the DAL layer

                    _dal.Engineer.Update(becomeDO);
                }
                else
                {
                    //  throw new DalAlreadyExistsException($"Engineer with ID={engineerToAdd.Id} already exists");
                }

            }
        }
        else;
        //  throw new DalAlreadyExistsException($"Engineer with ID={engineerToAdd.Id} already exists");


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
                else;
                //  throw new DalAlreadyExistsException($"Engineer with ID={id} already exists");
            }
        }
        else;
        //  throw new DalAlreadyExistsException($"Engineer with ID={id} already exists");
    }
    public IEnumerable<BO.EngineerInTask> ReadAll(Func<DO.Engineer?, bool>? func = null)
    {

        EngineerImplementation toCheckTask = new EngineerImplementation();


        IEnumerable<BO.EngineerInTask?> engineers = (from item in _dal.Engineer.ReadAll().ToList()
                                                     where func(item)
                                                     select new BO.EngineerInTask()
                                                     {
                                                         Id = item.Id,
                                                         Name = item.Name,
                                                     });
        return engineers;
    }
}