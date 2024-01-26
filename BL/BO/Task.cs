using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class Task
{
    int Id;
    string Description;
    string Alias;
    DateTime CreatedAtDate;
    Status? Status;
    List<BO.TaskInList>? Dependencies;
    MilestoneInTask? MilestoneInTask;
    TimeSpan? RequiredEffortTime;
    DateTime? StartDate;
    DateTime? ScheduledDate;
    DateTime? ForecastDate;
    DateTime? DeadlineDate;
    DateTime? CompleteDate;
    string? Deliverables;
    string? Remarks;
    EngineerInTask? Engineer;
    EngineerExperience? Copmlexity;
};

