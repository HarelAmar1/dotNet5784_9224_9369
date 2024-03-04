namespace BlApi;

public interface ISchedule
{
    public void setStartDateOfProject(DateTime startDate);
    public void setEndDateOfProject(DateTime endDate);
    public DateTime? getStartDateOfProject();
    public DateTime? getEndDateOfProject();
}