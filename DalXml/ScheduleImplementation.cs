using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dal;

internal class ScheduleImplementation : ISchedule
{
    readonly string xml_file = "data-config";

    /// <summary>
    /// Sets the start date of the project in the XML file.
    /// </summary>
    /// <param name="startDate">The start date of the project.</param>
    public void setStartDateOfProject(DateTime startDate)
    {
        // Load the XML file
        XElement root = XMLTools.LoadListFromXMLElement(xml_file);
        // Set the start project date element value to the provided start date
        root.Element("startProjectDate")?.SetValue(startDate.ToString());
        // Save the updated XML file
        XMLTools.SaveListToXMLElement(root, xml_file);
    }

    /// <summary>
    /// Sets the end date of the project in the XML file.
    /// </summary>
    /// <param name="endDate">The end date of the project.</param>
    public void setEndDateOfProject(DateTime endDate)
    {
        // Load the XML file
        XElement root = XMLTools.LoadListFromXMLElement(xml_file);
        // Set the end project date element value to the provided end date
        root.Element("endProjectDate")?.SetValue(endDate.ToString());
        // Save the updated XML file
        XMLTools.SaveListToXMLElement(root, xml_file);
    }

    /// <summary>
    /// Gets the start date of the project from the XML file.
    /// </summary>
    /// <returns>The start date of the project, or null if not set.</returns>
    public DateTime? getStartDateOfProject()
    {
        // Load the XML file
        XElement root = XMLTools.LoadListFromXMLElement(xml_file);
        // Get the start project date element
        XElement startDate = root.Element("startProjectDate");
        // If start date is not set, return null; otherwise, parse and return the date
        if (startDate.Value == "")
            return null;
        return DateTime.Parse(startDate.Value.ToString());
    }

    /// <summary>
    /// Gets the end date of the project from the XML file.
    /// </summary>
    /// <returns>The end date of the project, or null if not set.</returns>
    public DateTime? getEndDateOfProject()
    {
        // Load the XML file
        XElement root = XMLTools.LoadListFromXMLElement(xml_file);
        // Get the end project date element
        XElement endDate = root.Element("endProjectDate");
        // If end date is not set, return null; otherwise, parse and return the date
        if (endDate.Value == "")
            return null;
        return DateTime.Parse(endDate.ToString());
    }
}
