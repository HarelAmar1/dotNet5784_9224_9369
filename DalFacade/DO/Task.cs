using System.Reflection.Emit;

namespace DO;

/// <summary>
/// Represents a task in a project management context.
/// </summary>
/// <param name="Id">The unique identifier of the task.</param>
/// <param name="Alias">An optional short name or alias for the task.</param>
/// <param name="Description">An optional description of the task.</param>
/// <param name="CreatedAtDate">The date and time when the task was created.</param>
/// <param name="RequiredEffortTime">The estimated time required to complete the task.</param>
/// <param name="IsMilestone">Indicates whether the task is a milestone in the project.</param>
/// <param name="Copmlexity">The complexity level of the task, represented by EngineerExperience.</param>
/// <param name="StartDate">The date when work on the task is scheduled to start.</param>
/// <param name="ScheduledDate">The scheduled date for the task, if different from the start date.</param>
/// <param name="DeadlineDate">The deadline by which the task should be completed.</param>
/// <param name="CompleteDate">The date when the task was actually completed.</param>
/// <param name="Deliverables">A description of what will be delivered upon task completion.</param>
/// <param name="Remarks">Any additional remarks or notes related to the task.</param>
/// <param name="EngineerId">The identifier of the engineer responsible for the task.</param>
public record Task
(
    int Id,
    string Description,
    string Alias,
    bool IsMilestone,
    DateTime CreatedAtDate,
    DateTime? RequiredEffortTime = null,
    EngineerExperience? Copmlexity = null, 
    DateTime? StartDate = null,
    DateTime? ScheduledDate = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    string? Deliverables = null,
    string? Remarks = null,
    int EngineerId = 0
)
{
    //Empty Ctor
    public Task() : this(Id: 0, Description: "", Alias:"", IsMilestone:false, CreatedAtDate:DateTime.Now)
    { }

}