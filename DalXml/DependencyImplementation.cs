using DalApi;
using DO;
using System.Runtime.Intrinsics.Arm;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencies_xml = "dependencies";

    public int Create(Dependency item)
    {
        XElement xElementDependency = XMLTools.LoadListFromXMLElement(s_dependencies_xml);// this is root

        int newId = XMLTools.GetAndIncreaseNextId("data-config", "NextDependencyId");    //לבדוק !! שאכן כתוב אידי כמו כאן או לשנות
        Dependency updatedDependency = item with { Id = newId };
        xElementDependency.Add(updatedDependency);  //לבדוק אם נכנס טוב לאלמנט?
        XMLTools.SaveListToXMLElement(xElementDependency, s_dependencies_xml);

        return newId;

    }

    public void Delete(int id)
    {
        //check if ID exist by Read func AND if not exist the Read func throw Exception
        if (Read(id) == null) ;
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_dependencies_xml);// this is root
        (from depend in rootDependency.Elements()
         where (int?)depend.Element("ID") == id
         select depend).FirstOrDefault()?.Remove();
        XMLTools.SaveListToXMLElement(rootDependency, s_dependencies_xml);

    }

    public Dependency? Read(int id)
    {
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_dependencies_xml);// this is root
        return (from depend in rootDependency.Elements()
                where (int?)depend.Element("ID") == id
                select new Dependency()
                {
                    Id = (int)(depend.Element("ID")),
                    DependentTask = (int?)depend.Element("DependentTask"),
                    DependsOnTask = (int?)depend.Element("DependsOnTask")
                }).FirstOrDefault() ?? throw new DalDoesNotExistException($"ID: {id}, not exist");
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        if (filter == null)
            return null;

        IEnumerable<Dependency?> dependencies = ReadAll(filter);
        return dependencies.FirstOrDefault();


    }
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_dependencies_xml);// this is roo
        List<XElement> dependFromXMLList = rootDependency.Elements().ToList();//להעביר את כל ה אקסמל לרשימה
        List<Dependency?> depends = new List<Dependency?>();//רשימה ששומרת את המשימות
        foreach (var dependFromXML in dependFromXMLList)
        {
            depends.Add(new Dependency()
            {
                Id = (int)dependFromXML.Element("ID"),
                DependentTask = (int?)dependFromXML.Element("DependentTask"),
                DependsOnTask = (int?)dependFromXML.Element("DependsOnTask")
            });
        }
        if (filter == null)
            return depends;

        List<Dependency> filterDependencies = depends.Where(filter).ToList();
        return filterDependencies;
    }


    public void Update(Dependency dependency)
    {
       /* if ID not exist the Delete func throw Exception 
        AND if exist we will delete it and bring it back*/

        Delete(dependency.Id);
        Create(dependency);
    }
}
