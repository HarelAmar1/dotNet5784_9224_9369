namespace BlApi;

public interface  IMilestone
{
    public IEnumerable<BO.MilestoneInList> SeeTheListOfMilestones();
    public void UpdateMilestoneDetails();
    public IEnumerable<BO.MilestoneInList> ReturnsTheEngineerListOfMilestones();
}