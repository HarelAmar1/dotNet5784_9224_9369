using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

internal class ScheduleImplementation : ISchedule
{
    /// setStartDateOfProject
    /// 
    /// <param name="startDate" Getting a start date for the project></param>
    public void setStartDateOfProject(DateTime startDate)
    {
        DataSource.Config.startProjectDate = startDate;
    }

    /// setEndDateOfProject
    /// 
    /// <param name="endDate" Receiving the end date of the project></param>
    public void setEndDateOfProject(DateTime endDate)
    {
        DataSource.Config.endProjectDate = endDate;
    }
    /// getStartDateOfProject
    /// 
    /// <returns Returns the end date of the project></returns>
    public DateTime? getStartDateOfProject()
    {
        return DataSource.Config.startProjectDate;
    }

    /// getEndDateOfProject
    /// 
    /// <returns Returns the start date of the project></returns>
    public DateTime? getEndDateOfProject()
    {
        return DataSource.Config.endProjectDate;
    }
}
