using DalApi;

namespace BlApi;

public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }
    public IMilestone Milestone { get; }
    public ISchedule Schedule { get; }
    public IUser User{ get; }
    public void InitializeDB();
    public void ResetDB();

    #region clock
    public DateTime Clock { get; }
    public void AddedInAnYear();
    public void AddedInAnHour();
    public void AddedInAnDay();
    public void TimeReset();
    #endregion
}