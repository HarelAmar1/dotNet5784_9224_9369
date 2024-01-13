namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        Dependency newItem = item with { Id = DataSource.Config.NextstartDependencyId };
        DataSource.Dependencies.Add(newItem);
        return newItem.Id;
    }

    public void Delete(int id)//Check what inactive entities are
    {
        if (DataSource.Dependencies.Any(dependency => dependency.Id == id))//if exist in the list
        {   //delete it
            DataSource.Dependencies.RemoveAll(D => D.Id == id);
        }
        else
            throw new Exception($"ID: {id}, not exist");
    }

    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.FirstOrDefault(D => D.Id == id);
    }

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

    public void Update(Dependency item)
    {
        //if exist in the list
        if (DataSource.Dependencies.Any(dependency => dependency.Id == item.Id))
        {
            Delete(item.Id); //delete it and add a new item
            DataSource.Dependencies.Add(item);
        }
        else
            throw new Exception($"Dependency with ID={item.Id} is not exists");

    }
}