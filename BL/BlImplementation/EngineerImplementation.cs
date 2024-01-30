using BlApi;
using BO;

namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{

    
    private DalApi.IDal _dal = Factory.Get;
    public void Create(BO.Engineer engineerToAdd)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Engineer Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<EngineerInTask> ReadAll(Func<DO.Task?, bool>? func = null)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Engineer engineerToUpdate)
    {
        throw new NotImplementedException();
    }
}
