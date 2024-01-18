using DalApi;
using DO;

namespace Dal;


internal class TaskImplementation :ITask
{
    readonly string s_tasks_xml = "tasks";

    public int Create(DO.Task item)
    {
        List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        int newId = XMLTools.GetAndIncreaseNextId(s_tasks_xml, "ID");//לבדוק !! שאכן כתוב אידי כמו כאן או לשנות
        DO.Task updatedTask = item with { Id = newId };//נעדכן את האידי
        listTask.Add(updatedTask);//נכניס לרשימה
        XMLTools.SaveListToXMLSerializer<DO.Task>(listTask, s_tasks_xml); //return to XML file
        return item.Id;
    }

    public void Delete(int id)
    {
        List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        if (listTask.Any(task => task.Id == id))
            listTask.RemoveAll(T => T.Id == id);
        else
            throw new DalDoesNotExistException($"ID: {id}, not exist");
        XMLTools.SaveListToXMLSerializer<DO.Task>(listTask, s_tasks_xml);
    }

    public DO.Task? Read(int id)
    {
        List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        return listTask.FirstOrDefault(T => T.Id == id);
    }

    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
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
        List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
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
        List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        if (listTask.Any(task => task.Id == item.Id))
        {
            Delete(item.Id);
            listTask.Add(item);
            XMLTools.SaveListToXMLSerializer<DO.Task>(listTask, s_tasks_xml);
        }
        else
            throw new DalDoesNotExistException($"Task with ID={item.Id} is not exists");
    }
}
