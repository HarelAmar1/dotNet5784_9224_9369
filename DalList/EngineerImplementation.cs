namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        //We will check if there is an engineer
        if (DataSource.Engineers.Any(engineer => engineer.Id == item.Id))//if exist in the list
            throw new Exception($"Engineer with ID={item.Id} already exists");

        else
            DataSource.Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        if (DataSource.Engineers.Any(engineer => engineer.Id == id))//if exist in the list
        {   //delete it
            DataSource.Engineers.RemoveAll(E => E.Id == id);
        }
        else
            throw new Exception($"ID: {id}, not exist");
        
    }

    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(E => E.Id == id);
    }

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

    public void Update(Engineer item)
    {
        //if exist in the list
        if (DataSource.Engineers.Any(engineers => engineers.Id == item.Id))
        {
            Delete(item.Id);
            DataSource.Engineers.Add(item);
        }
        else
            throw new Exception($"Engineer with ID={item.Id} is not exists");

    }
}