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

    /// <summary>
    /// Creates a new dependency in the system.
    /// </summary>
    /// <param name="item">The dependency to create.</param>
    /// <returns>The ID of the created dependency.</returns>
    public int Create(Dependency item)
    {
        XElement xElementDependency = XMLTools.LoadListFromXMLElement(s_dependencies_xml);// this is root
        // Create a dependency with a new ID
        int newId = XMLTools.GetAndIncreaseNextId("data-config", "NextDependencyId");
        Dependency updatedDependency = item with { Id = newId };
        XElement id = new XElement("ID", updatedDependency.Id);
        XElement DependentTask = new XElement("DependentTask", updatedDependency.DependentTask);
        XElement DependsOnTask = new XElement("DependsOnTask", updatedDependency.DependsOnTask);
        XElement newDepend = new XElement("Dependency", id, DependentTask, DependsOnTask);
        // Add the new dependency to the XML
        xElementDependency.Add(newDepend);
        // Save the updated XML
        XMLTools.SaveListToXMLElement(xElementDependency, s_dependencies_xml);

        return newId;
    }

    /// <summary>
    /// Deletes a dependency from the system.
    /// </summary>
    /// <param name="id">The ID of the dependency to delete.</param>
    /// <exception cref="DalDoesNotExistException">Thrown when the dependency with the specified ID does not exist.</exception>
    public void Delete(int id)
    {
        // Check if the dependency exists and delete it
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_dependencies_xml);// this is root
        XElement elemToDelete = rootDependency.Elements("Dependency").FirstOrDefault(x => (int?)x.Element("ID") == id);
        if (elemToDelete != null)
        {
            elemToDelete.Remove();
            XMLTools.SaveListToXMLElement(rootDependency, s_dependencies_xml);
        }
        else
            throw new DalDoesNotExistException($"ID: {id}, not exist");
    }

    /// <summary>
    /// Reads a dependency from the system by ID.
    /// </summary>
    /// <param name="id">The ID of the dependency to read.</param>
    /// <returns>The dependency with the specified ID, or null if not found.</returns>
    /// <exception cref="DalDoesNotExistException">Thrown when the dependency with the specified ID does not exist.</exception>
    public Dependency? Read(int id)
    {
        // Find and return the dependency with the specified ID
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

    /// <summary>
    /// Reads a dependency from the system based on a filter.
    /// </summary>
    /// <param name="filter">The filter condition.</param>
    /// <returns>The first dependency that matches the filter condition, or null if not found.</returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        if (filter == null)
            return null;

        IEnumerable<Dependency?> dependencies = ReadAll(filter);
        return dependencies.FirstOrDefault();
    }

    /// <summary>
    /// Reads all dependencies from the system based on a filter condition.
    /// </summary>
    /// <param name="filter">The filter condition.</param>
    /// <returns>The list of dependencies that match the filter condition, or all dependencies if no filter is specified.</returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_dependencies_xml);// this is root
        List<XElement> dependFromXMLList = rootDependency.Elements().ToList();//Transfer all XML to a list
        List<Dependency?> depends = new List<Dependency?>();//A list that saves the dependencies
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
        // Filter the dependencies based on the provided condition
        List<Dependency> filterDependencies = depends.Where(filter).ToList();
        return filterDependencies;
    }

    /// <summary>
    /// Updates an existing dependency in the system.
    /// </summary>
    /// <param name="dependency">The dependency with updated information.</param>
    public void Update(Dependency dependency)
    {
        // Delete the existing dependency
        Delete(dependency.Id);

        // Create the updated dependency with the same ID but new data
        XElement xElementDependency = XMLTools.LoadListFromXMLElement(s_dependencies_xml);// this is root
        XElement id = new XElement("ID", dependency.Id);
        XElement DependentTask = new XElement("DependentTask", dependency.DependentTask);
        XElement DependsOnTask = new XElement("DependsOnTask", dependency.DependsOnTask);
        XElement newDepend = new XElement("Dependency", id, DependentTask, DependsOnTask);
        // Add the updated dependency to the XML
        xElementDependency.Add(newDepend);
        // Save the updated XML
        XMLTools.SaveListToXMLElement(xElementDependency, s_dependencies_xml);
    }
}
