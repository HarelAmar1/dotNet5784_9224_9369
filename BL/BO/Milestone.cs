using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public record class Milestone
(
     int Id,
    string Description,
    string Alias,
    DateTime CreatedAtDate,
    Status Status,
    DateTime ForecastDate,
    DateTime DeadlineDate,
    DateTime CompleteDate,
    double CompletionPercentage,
    string Remarks,
    List<TaskInList> Dependencies
);
