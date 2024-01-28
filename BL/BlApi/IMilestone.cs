
using BO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
namespace BlApi;

public interface  IMilestone
{
    public IEnumerable<BO.MilestoneInList> SeeTheListOfMilestones();
    public void UpdateMilestoneDetails();
    public IEnumerable<BO.MilestoneInList> ReturnsTheEngineerListOfMilestones();
}
