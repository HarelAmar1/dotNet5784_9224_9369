using BlApi;
namespace BlImplementation;

internal class Bl : IBl
{

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation(this);
    public ISchedule Schedule => new ScheduleImplementation();

    public IUser User => new UserImplementation();
    public IMilestone Milestone => throw new NotImplementedException();//check if we do

    #region Clock
    private static DateTime s_Clock = DateTime.Now;
    public DateTime Clock
    {
        get { return s_Clock; }
        private set { s_Clock = value; }
    }
    public void AddedInAnYear()
    {
        TimeSpan toAdd = new TimeSpan(365, 0, 0, 0);
        Clock = s_Clock + toAdd;
    }
    public void AddedInAnDay()
    {
        TimeSpan toAdd = new TimeSpan(1, 0, 0, 0);
        Clock = s_Clock + toAdd;
    }
    public void AddedInAnHour()
    {
        TimeSpan toAdd = new TimeSpan(1, 0, 0);
        Clock = s_Clock + toAdd;
    }
    public void TimeReset()
    {
        Clock = DateTime.Now;
    }
    #endregion 

    public void InitializeDB() => DalTest.Initialization.Do();
    public void ResetDB() => DalTest.Initialization.Reset();
}
