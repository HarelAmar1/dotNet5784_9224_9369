namespace Dal;

internal static class DataSource
{
    //Our temporary database
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Dependency> Dependencies { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();
    internal static List<DO.User> Users { get; } = new();

    internal static class Config
    {
        //for the TaskId running
        internal const int startTaskId = 1;//ID starts from 1
        private static int nextStartTaskId = startTaskId;
        internal static int NextStartTaskId { get => nextStartTaskId++; }


        //for the DependencyId running
        internal const int startDependencyId = 1;//ID starts from 1
        private static int nextstartDependencyId = startDependencyId;
        internal static int NextstartDependencyId { get => nextstartDependencyId++; }

        //for Project Times
        internal static DateTime? startProjectDate = null;
        internal static DateTime? endProjectDate = null;


    }
}
