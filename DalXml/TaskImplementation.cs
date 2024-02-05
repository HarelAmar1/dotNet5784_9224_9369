using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal;


internal class TaskImplementation :ITask
{
    readonly string s_tasks_xml = "tasks";

    public int Create(DO.Task item)
    {
        List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        int newId = XMLTools.GetAndIncreaseNextId("data-config", "NextTaskId");
        DO.Task updatedTask = item with { Id = newId };//update the ID
        listTask.Add(updatedTask);//insert to list
        XMLTools.SaveListToXMLSerializer<DO.Task>(listTask, s_tasks_xml); //return to XML file
        return newId;
    }

    public void Delete(int id)
    {
        List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);// this is root
        if (listTask.Any(task => task.Id == id))//Looking for the ID to delete
            listTask.RemoveAll(T => T.Id == id);
        else
            throw new DalDoesNotExistException($"ID: {id}, not exist");
        ////return to XML file
        XMLTools.SaveListToXMLSerializer<DO.Task>(listTask, s_tasks_xml);
    }

    public DO.Task? Read(int id)
    {
        
        List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);// this is root
        //Returns the first bargain
        return listTask.FirstOrDefault(T => T.Id == id);
    }

    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);// this is root

        //Returns the first bargain with a condition
        if (filter != null)
        {
            return (DO.Task?)(from item in listTask
                               where filter(item)
                               select item);
        }
        return null;
    }

    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);// this is root

        //Returns the entire list and if a condition exists, it returns only them
        if (filter != null)
        {
            return from item in listTask
                   where filter(item)
                   select item;
        }
        return from item in listTask
               select item;
    }

    public void Update(DO.Task item)
    {
        List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);// this is root
        //Looking for someone to update
        if (listTask.Any(task => task.Id == item.Id))
        {
            int index = listTask.FindIndex(e => e.Id == item.Id);
            listTask.RemoveAt(index);
            listTask.Add(item);
            //return to XML file
            XMLTools.SaveListToXMLSerializer<DO.Task>(listTask, s_tasks_xml);
        }
        else
            throw new DalDoesNotExistException($"Task with ID={item.Id} is not exists");
    }
}
