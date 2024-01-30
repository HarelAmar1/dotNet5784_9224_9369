using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

//לא לשכוח לתעד את הכל עם 3 סלשים
public class Task
{
    public int Id { get; init; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public DateTime CreatedAtDate { get; init; }
    public Status? Status { get; set; }
    public List<BO.TaskInList>? Dependencies { get; set; }
    public MilestoneInTask? MilestoneInTask { get; set; }//רלוונטי רק מי שמוסיף את האבני דרך. ראה פרויקט כללי עמוד 13 (אנחנו ככל הנראה עושים)
    public TimeSpan? RequiredEffortTime { get; set; }
    public DateTime? StartDate { get; init; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; init; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public EngineerInTask? Engineer { get; set; }
    public EngineerExperience? Copmlexity { get; set; }
    //public override string ToString() => this.ToStringProperty();   לבדוק איך לממש

};

