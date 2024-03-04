using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

internal class ScheduleImplementation : ISchedule
{
    public void setStartDateOfProject(DateTime startDate)
    {
        DataSource.Config.startProjectDate = startDate;
    }

    public void setEndDateOfProject(DateTime endDate)
    {
        DataSource.Config.endProjectDate = endDate;
    }

    public DateTime? getStartDateOfProject()
    {
        return DataSource.Config.startProjectDate;
    }

    public DateTime? getEndDateOfProject()
    {
        return DataSource.Config.endProjectDate;
    }
}
