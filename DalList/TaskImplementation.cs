namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        Task newItem = item with { Id = DataSource.Config.NextStartTaskId };
        DataSource.Tasks.Add(newItem);
        return newItem.Id;
    }

    public void Delete(int id)
    {
        if (DataSource.Tasks.Any(task => task.Id == id))//if exist in the list
        {   //delete it
            DataSource.Tasks.RemoveAll(T => T.Id == id);
        }
        else
            throw new DalDoesNotExistException($"ID: {id}, not exist");
    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(T => T.Id == id);
    }

    public Task? Read(Func<Task, bool> filter)
    {
        if (filter != null)
        {
            return (Task?)(from item in DataSource.Tasks
                   where filter(item)
                     select item);
        }
        
        return null;
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Tasks
               select item;
    }

    public void Update(Task item)
    {
        //if exist in the list
        if (DataSource.Tasks.Any(task => task.Id == item.Id))
        {
            Delete(item.Id);
            DataSource.Tasks.Add(item);
        }
        else
            throw new DalDoesNotExistException($"Task with ID={item.Id} is not exists");

    }

}