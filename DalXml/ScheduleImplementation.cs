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
    public void setStartDateOfProject(DateTime startDate)
    {
        XElement root = XMLTools.LoadListFromXMLElement(xml_file);
        root.Element("startProjectDate")?.SetValue(startDate.ToString());
        XMLTools.SaveListToXMLElement(root, xml_file);
    }
    public void setEndDateOfProject(DateTime endDate)
    {
        XElement root = XMLTools.LoadListFromXMLElement(xml_file);
        root.Element("endProjectDate")?.SetValue(endDate.ToString());
        XMLTools.SaveListToXMLElement(root, xml_file);
    }
    public DateTime? getStartDateOfProject()
    {
        XElement root = XMLTools.LoadListFromXMLElement(xml_file);
        XElement startDate = root.Element("startProjectDate");
        if (startDate.Value == "")
            return null;
        return DateTime.Parse(startDate.ToString());
    }
    public DateTime? getEndDateOfProject()
    {
        XElement root = XMLTools.LoadListFromXMLElement(xml_file);
        XElement endDate = root.Element("endProjectDate");
        if (endDate.Value == "")
            return null;
        return DateTime.Parse(endDate.ToString());
    }
}
