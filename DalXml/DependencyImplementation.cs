using DalApi;
using DO;
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
        (from depend in rootDependency.Elements()
         where (int?)depend.Element("ID") == id
         select depend).FirstOrDefault()?.Remove(); // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!11?? throw new DalDoesNotExistException($"ID: {id}, not exist");
    }

    public Dependency? Read(int id)
    {
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_dependencies_xml);// this is root
        (from depend in rootDependency.Elements()
         where (int?)depend.Element("ID") == id
         select new Dependency()
         {
             Id = (int)(depend.Element("ID") ?? throw new DalCanNotBeNULL("Dal Can Not Be NULL"),
            DependentTask = (int?)(depend.Element("DependentTask") ?? throw new DalCanNotBeNULL("Dal Can Not Be NULL"),
            DependsOnTask = (int?)(depend.Element("DependsOnTask") ??throw new DalCanNotBeNULL("Dal Can Not Be NULL")
         }
         ).FirstOrDefault()?.Remove();
        throw new NotImplementedException();
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
