using BlApi;
namespace BlImplementation;

internal class Bl : IBl
{

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public IMilestone Milestone => throw new NotImplementedException();//check if we do

    
}
