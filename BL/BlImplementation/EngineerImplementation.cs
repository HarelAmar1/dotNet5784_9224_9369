using BlApi;
using BO;

namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{

    
    private DalApi.IDal _dal = DalApi.Factory.Get();
    public void addEngineer(BO.Engineer engineerToAdd)
    {
        throw new NotImplementedException();
    }

    public void deleteEngineer(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Engineer getEngineer(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<EngineerInTask> getEngineersList(Func<DO.Task?, bool>? func = null)
    {
        throw new NotImplementedException();
    }

    public void UpdateEngineer(BO.Engineer engineerToUpdate)
    {
        throw new NotImplementedException();
    }
}
