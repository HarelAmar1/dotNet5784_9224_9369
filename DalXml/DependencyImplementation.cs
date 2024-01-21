using DalApi;
using DO;
using System.Runtime.Intrinsics.Arm;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dal;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencies_xml = "dependencies";

    public int Create(Dependency item)
    {
        XElement xElementDependency = XMLTools.LoadListFromXMLElement(s_dependencies_xml);//this is  root
        //craete a Depedency  
        int newId = XMLTools.GetAndIncreaseNextId("data-config", "NextDependencyId");    
        Dependency updatedDependency = item with { Id = newId };
        XElement id = new XElement("ID", updatedDependency.Id);
        XElement DependentTask = new XElement("DependentTask", updatedDependency.DependentTask);
        XElement DependsOnTask = new XElement("DependsOnTask", updatedDependency.DependsOnTask);
        XElement newDepend = new XElement("Dependency", id, DependentTask, DependsOnTask);
        //adds to the list
        xElementDependency.Add(newDepend);
        //Returns the list to a file
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
        //Returns the list to a file
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
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_dependencies_xml);// this is root
        List<XElement> dependFromXMLList = rootDependency.Elements().ToList();//Transfer all XML to a list
        List<Dependency?> depends = new List<Dependency?>();//A list that saves the tasks
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
        //Returns the list to a file
        List<Dependency> filterDependencies = depends.Where(filter).ToList();
        return filterDependencies;
    }


    public void Update(Dependency dependency)
    {

        //We will delete the object we received
        Delete(dependency.Id);

        //We will recreate it with the same ID but the new data
        XElement xElementDependency = XMLTools.LoadListFromXMLElement(s_dependencies_xml);// this is root
        XElement id = new XElement("ID", dependency.Id);
        XElement DependentTask = new XElement("DependentTask", dependency.DependentTask);
        XElement DependsOnTask = new XElement("DependsOnTask", dependency.DependsOnTask);
        XElement newDepend = new XElement("Dependency", id, DependentTask, DependsOnTask);
        //adds to the list
        xElementDependency.Add(newDepend);
        //Returns the list to a file
        XMLTools.SaveListToXMLElement(xElementDependency, s_dependencies_xml);
    }
}
