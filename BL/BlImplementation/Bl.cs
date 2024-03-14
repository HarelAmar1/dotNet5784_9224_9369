using BlApi;
using BO;
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
        ChangeACompleteDate(Clock);

    }
    public void AddedInAnDay()
    {
        TimeSpan toAdd = new TimeSpan(1, 0, 0, 0);
        Clock = s_Clock + toAdd;
        ChangeACompleteDate(Clock);

    }
    public void AddedInAnHour()
    {
        TimeSpan toAdd = new TimeSpan(1, 0, 0);
        Clock = s_Clock + toAdd;
        ChangeACompleteDate(Clock);

    }
    public void TimeReset()
    {
        Clock = DateTime.Now;
        ChangeACompleteDateToInit(Clock);
    }
    public void ChangeACompleteDate(DateTime clock)
    {
        IEnumerable<BO.Task> tasks = (from task in Task.ReadAll()
                                      where Task.Read(task.Id).ScheduledDate + Task.Read(task.Id).RequiredEffortTime <= Clock
                                      select Task.Read(task.Id)).ToList();

        foreach (BO.Task task in tasks)
        {
            BO.Task task1 = new BO.Task
            {
                Id = task.Id,
                Description = task.Description,
                Alias = task.Alias,
                CreatedAtDate = task.CreatedAtDate,
                Status = task.Status,
                Dependencies = task.Dependencies,
                RequiredEffortTime = task.RequiredEffortTime,
                StartDate = task.StartDate,
                ScheduledDate = task.ScheduledDate,
                ForecastDate = task.ForecastDate,
                DeadlineDate = task.DeadlineDate,
                CompleteDate = task.ScheduledDate.Value.AddDays(task.RequiredEffortTime.Value.Days),
                Deliverables = task.Deliverables,
                Remarks = task.Remarks,
                Engineer = task.Engineer,
                Copmlexity = task.Copmlexity
            };
            Task.Update(task1);

        }
    }
    public void ChangeACompleteDateToInit(DateTime clock)
    {
        IEnumerable<BO.Task> tasks = (from task in Task.ReadAll()
                                      where Task.Read(task.Id).CompleteDate != null && Task.Read(task.Id).CompleteDate > Clock
                                      select Task.Read(task.Id)).ToList();


        foreach (BO.Task task in tasks)
        {
            BO.Task task1 = new BO.Task
            {
                Id = task.Id,
                Description = task.Description,
                Alias = task.Alias,
                CreatedAtDate = task.CreatedAtDate,
                Status = task.Status,
                Dependencies = task.Dependencies,
                RequiredEffortTime = task.RequiredEffortTime,
                StartDate = task.StartDate,
                ScheduledDate = task.ScheduledDate,
                ForecastDate = task.ForecastDate,
                DeadlineDate = task.DeadlineDate,
                CompleteDate = null,
                Deliverables = task.Deliverables,
                Remarks = task.Remarks,
                Engineer = task.Engineer,
                Copmlexity = task.Copmlexity
            };
            Task.Update(task1);

        }
    }
        #endregion
        public void InitializeDB() => DalTest.Initialization.Do();
        public void ResetDB() => DalTest.Initialization.Reset();
    }
