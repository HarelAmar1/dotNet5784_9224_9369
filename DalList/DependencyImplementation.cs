namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class DependencyImplementation : IDependency
{
    /// <Create>
    /// 
    /// <param name="item" creates dependency></param>
    /// <returns Returns the ID of the dependency></returns>
    public int Create(Dependency item)
    {
        Dependency newItem = item with { Id = DataSource.Config.NextstartDependencyId };
        DataSource.Dependencies.Add(newItem);
        return newItem.Id;
    }

    /// <Delete>
    /// 
    /// <param name="id" Gets a dependency ID to delete></param>
    /// <exception cref="DalDoesNotExistException" An exception does not exist></exception>
    public void Delete(int id)//Check what inactive entities are
    {
        if (DataSource.Dependencies.Any(dependency => dependency.Id == id))//if exist in the list
        {   //delete it
            DataSource.Dependencies.RemoveAll(D => D.Id == id);
        }
        else
            throw new DalDoesNotExistException($"ID: {id}, not exist");
    }

    /// Read
    /// 
    /// <param name="id" Dependency ID></param>
    /// <returns Returns the dependency by ID></returns>
    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.FirstOrDefault(D => D.Id == id);
    }

    /// Read
    /// 
    /// <param name="filter" filter function></param>
    /// <returns Returns depending on filter></returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        if (filter != null)
        {
            return (Dependency?)(from item in DataSource.Dependencies
                   where filter(item)
                   select item);
        }
        return null;
    }
    ///ReadAll
    /// 
    /// <param name="filter" Dependency list filter></param>
    /// <returns Returns a list of dependencies></returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Dependencies
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Dependencies
               select item;
    }
    /// Update
    /// 
    /// <param name="item" ></param>
    /// <exception cref="DalDoesNotExistException" An exception does not exist></exception>
    public void Update(Dependency item)
    {
        //if exist in the list
        if (DataSource.Dependencies.Any(dependency => dependency.Id == item.Id))
        {
            Delete(item.Id); //delete it and add a new item
            DataSource.Dependencies.Add(item);
        }
        else
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} is not exists");

    }
}