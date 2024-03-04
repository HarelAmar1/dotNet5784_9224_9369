namespace BlApi;

public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }
    public IMilestone Milestone{ get; }
    public ISchedule Schedule { get; }
    public void InitializeDB();
    public void ResetDB();

}
