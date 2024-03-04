using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BlApi;

namespace BlImplementation;

internal class ScheduleImplementation : ISchedule
{
    private DalApi.IDal _dal = Factory.Get;
    public DateTime? getEndDateOfProject()
    {
        return _dal.Schedule.getStartDateOfProject();
    }

    public DateTime? getStartDateOfProject()
    {
        return _dal.Schedule.getEndDateOfProject();
    }

    public void setEndDateOfProject(DateTime endDate)
    {
        _dal.Schedule.setEndDateOfProject(endDate);
    }

    public void setStartDateOfProject(DateTime startDate)
    {
        _dal.Schedule.setStartDateOfProject(startDate);
    }
}
