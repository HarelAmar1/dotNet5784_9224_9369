namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// Create
    /// </summary>
    /// <param name="item " Engineer to add to the DAL layer></param>
    /// <returns engineerToAdd.Id Returns the id of the added engineer></returns>
    /// <exception cref="DalAlreadyExistsException" Exception of already existing engineer with ID></exception>
    public int Create(Engineer item)
    {
        //We will check if there is an engineer
        if (DataSource.Engineers.Any(engineer => engineer.Id == item.Id))//if exist in the list
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");

        else
            DataSource.Engineers.Add(item);
        return item.Id;
    }
    /// Delete
    /// 
    /// <param name="id" Deletes an engineer by ID></param>
    /// <exception cref="DalDoesNotExistException" Exception of already existing engineer with ID></exception>
    public void Delete(int id)
    {
        if (DataSource.Engineers.Any(engineer => engineer.Id == id))//if exist in the list
        {   //delete it
            DataSource.Engineers.RemoveAll(E => E.Id == id);
        }
        else
            throw new DalDoesNotExistException($"ID: {id}, not exist");
        
    }

    /// <Read>
    /// 
    /// </summary>
    /// <param name="id" From the ID of an engineer  ></param>
    /// <returns Returns an engineer></returns>
    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(E => E.Id == id);
    }

    /// <Read>
    /// 
    /// <param name="filter" filtering by which></param>
    /// <returns Returns an engineer by filter></returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        if (filter != null)
        {
            return (Engineer?)(from item in DataSource.Engineers
                   where filter(item)
                   select item);
        }
        return null;
    }

    /// ReadAll
    ///
    /// <param name="filter" filtering by which></param>
    /// <returns Returns an engineer List by filter></returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Engineers
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Engineers
               select item;
    }

    /// <Update>
    /// 
    /// <param name="item" Getting an engineer to update></param>
    /// <exception cref="DalDoesNotExistException" An exception does not exist></exception>
    public void Update(Engineer item)
    {
        //if exist in the list
        if (DataSource.Engineers.Any(engineers => engineers.Id == item.Id))
        {
            Delete(item.Id);
            DataSource.Engineers.Add(item);
        }
        else
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} is not exists");

    }
}