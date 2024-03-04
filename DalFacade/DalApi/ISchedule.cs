using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi;

public interface ISchedule
{
    void setStartDateOfProject(DateTime startDate);
    void setEndDateOfProject(DateTime endDate);

    DateTime? getStartDateOfProject();
    DateTime? getEndDateOfProject();

}
