namespace DO;

/// <summary>
/// Represents a task dependency within a project management framework.
/// </summary>
/// <param name="Id">Unique identifier for the dependency.</param>
/// <param name="DependentTask">ID of the task that has a dependency.</param>
/// <param name="DependsOnTask">ID of the task that it depends on.</param>
public record Dependency
(
    int Id,
    int? DependentTask = null,
    int? DependsOnTask = null
)
{
    //Empty Ctor
    public Dependency() : this(0)
    { }
}
