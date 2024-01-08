namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

public class DependencyImplementation : IDependency
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
        Dependency? find = DataSource.Dependencies.Find(D => D.Id == id);
        if (find != null)
            return find;

        return null;
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies);
    }

    public void Update(Dependency item)
    {
        //if exist in the list
        if (DataSource.Dependencies.Any(dependency => dependency.Id == item.Id))
        {
            Delete(item.Id); //delete it and add a new
            DataSource.Dependencies.Add(item);
        }
        else
            throw new Exception($"Dependency with ID={item.Id} is not exists");

    }
}