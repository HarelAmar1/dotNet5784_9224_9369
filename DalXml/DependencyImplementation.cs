using DalApi;
using DO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencies_xml = "dependencies";

    public int Create(Dependency item)
    {
        XElement xElementDependency = XMLTools.LoadListFromXMLElement(s_dependencies_xml);

        int newId = XMLTools.GetAndIncreaseNextId(s_dependencies_xml, "ID");    //לבדוק !! שאכן כתוב אידי כמו כאן או לשנות
        Dependency updatedDependency = item with { Id = newId };
        xElementDependency.Add(updatedDependency);  //לבדוק אם נכנס טוב לאלמנט?
        return newId;
    }

    public void Delete(int id)
    {
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_dependencies_xml);// this is root
        XName nameOfTheId = 
        XElement a = rootDependency.Element(nameOfTheId);
        if (a != null)
            a.Remove();
        else
            throw new DalDoesNotExistException($"ID: {id}, not exist");
    }

    public Dependency? Read(int id)
    {
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_dependencies_xml);// this is root
        XElement = 
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Dependency item)
    {
        throw new NotImplementedException();
    }
}
