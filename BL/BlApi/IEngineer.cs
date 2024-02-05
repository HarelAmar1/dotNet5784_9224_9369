namespace BlApi;

public interface IEngineer
{

    //adding engineer (for admin screen)
    public int Create(BO.Engineer engineerToAdd);

    //Engineer details request (for engineer screen)
    public BO.Engineer Read(int id);

    //update engineer data
    public void Update(BO.Engineer engineerToUpdate);

    //deleting engineer (for manager screen)
    public void Delete(int id);

    //Engineer list request (for admin screen)
    public IEnumerable<BO.Engineer> ReadAll(Func<DO.Engineer?, bool>? func = null);
} 