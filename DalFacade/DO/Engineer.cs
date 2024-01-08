namespace DO;
/// <summary>
/// Represents an engineer's profile within a project management system.
/// </summary>
/// <param name="Id">Unique identifier for the engineer.</param>
/// <param name="Email">Email address of the engineer.</param>
/// <param name="Cost">Hourly or project cost associated with the engineer.</param>
/// <param name="Name">Name of the engineer.</param>
/// <param name="level">Experience level of the engineer, with a default of 'Beginner'.</param>
/// <param name="Active">Indicates whether the engineer is currently active in the system.</param>
public record Engineer
(
    int Id,
    string? Email = null,
    double? Cost = null,
    string? Name = null,
    DO.EngineerExperience level = EngineerExperience.Beginner,
    bool Active = true
)
{
    //Empty Ctor
    public Engineer() : this(0)
    { }
}
