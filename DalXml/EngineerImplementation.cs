using DalApi;
using DO;
using System.Data.Common;

namespace Dal;

internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";

    public int Create(Engineer item)
    {
        List<Engineer> listEngineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);// this is root
        if (listEngineers.Any(engineer => engineer.Id == item.Id))
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        else
            listEngineers.Add(item);//insert to list
        //return to XML file
        XMLTools.SaveListToXMLSerializer<Engineer>(listEngineers, s_engineers_xml);
        return item.Id;
    }

    public void Delete(int id)
    {
        List<Engineer> listEngineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        //Looking for the ID to delete
        if (listEngineers.Any(engineer => engineer.Id == id))
            listEngineers.RemoveAll(E => E.Id == id);
        else
            throw new DalDoesNotExistException($"ID: {id}, not exist");
        //return to XML file
        XMLTools.SaveListToXMLSerializer<Engineer>(listEngineers, s_engineers_xml);
    }

    public Engineer? Read(int id)
    {
        List<Engineer> listEngineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        return listEngineers.FirstOrDefault(E => E.Id == id);
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer> listEngineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (filter != null)//Returns the first bargain
        {
            return (Engineer?)(from item in listEngineers
                               where filter(item)
                               select item);
        }
        return null;
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> listEngineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);// this is root
        //Returns the entire list and if a condition exists, it returns only them
        if (filter != null)
        {
            return from item in listEngineers
                   where filter(item)
                   select item;
        }
        return from item in listEngineers
               select item;
    }

    public void Update(Engineer item)
    {
        List<Engineer> listEngineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (listEngineers.Any(engineers => engineers.Id == item.Id))
        {
            int index = listEngineers.FindIndex(e => e.Id == item.Id);
            listEngineers.RemoveAt(index);
            listEngineers.Add(item);
            //return to XML file
            XMLTools.SaveListToXMLSerializer<Engineer>(listEngineers, s_engineers_xml);
        }
        else
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} is not exists");
    }
}