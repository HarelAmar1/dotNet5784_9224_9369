using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

//לא לשכוח לתעד את הכל עם 3 סלשים
public class Task
{
    int Id { get; init; }
    string Description { get; set; }
    string Alias { get; set; }
    DateTime CreatedAtDate { get; init; }
    Status? Status { get; set; }
    List<BO.TaskInList>? Dependencies { get; set; }
    MilestoneInTask? MilestoneInTask { get; set; }//רלוונטי רק מי שמוסיף את האבני דרך. ראה פרויקט כללי עמוד 13 (אנחנו ככל הנראה עושים)
    TimeSpan? RequiredEffortTime { get; set; }
    DateTime? StartDate { get; init; }
    DateTime? ScheduledDate { get; set; }
    DateTime? ForecastDate { get; set; }
    DateTime? DeadlineDate { get; set; }
    DateTime? CompleteDate { get; init; }
    string? Deliverables { get; set; }
    string? Remarks { get; set; }
    EngineerInTask? Engineer { get; set; }
    EngineerExperience? Copmlexity { get; set; }
    //public override string ToString() => this.ToStringProperty();   לבדוק איך לממש
};

