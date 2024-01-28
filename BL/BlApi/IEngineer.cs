namespace BlApi;

public interface IEngineer
{
    public IEnumerable<BO.EngineerInTask> GettingTheListOfEngineer();

    public IEnumerable<BO.Engineer> EngineerDetailsRequest(int id);

    public void AddingAnEngineer(IEnumerable<BO.Engineer> NeedtoAdd);

    public void DeletingAnEngineer(int id);

    public void UpdatingAnEngineer(IEnumerable<BO.Engineer> NeedToUpdate);
}