namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    /// Create
    /// 
    /// <param name="item" Create a new task for the data layer></param>
    /// <returns></returns>
    public int Create(Task item)
    {
        Task newItem = item with { Id = DataSource.Config.NextStartTaskId };
        DataSource.Tasks.Add(newItem);
        return newItem.Id;
    }
    /// Delete
    /// 
    /// <param name="id" Deletes a task according to the code you receive></param>
    /// <exception cref="DalDoesNotExistException" There is no exception to this type of exception></exception>
    public void Delete(int id)
    {
        if (DataSource.Tasks.Any(task => task.Id == id))//if exist in the list
        {   //delete it
            DataSource.Tasks.RemoveAll(T => T.Id == id);
        }
        else
            throw new DalDoesNotExistException($"ID: {id}, not exist");
    }
    /// Read
    /// 
    /// <param name="id" Returns a task according to the code you received></param>
    /// <returns> Task </returns>
    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(T => T.Id == id);
    }

    /// Read
    /// 
    /// <param name="filter" Returns a task according to a certain filter ></param>
    /// <returns>Task</returns>
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
    /// ReadAll
    /// 
    /// <param name="filter" certain filter></param>
    /// <returns>Returns a list of tasks according to a certain filter</returns>
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
    /// Update
    /// 
    /// <param name="item" Receives Task </param>
    /// <exception cref="DalDoesNotExistException" There is no exception to this type of exception ></exception>
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