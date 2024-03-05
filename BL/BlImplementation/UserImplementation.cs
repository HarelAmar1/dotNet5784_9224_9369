using BlApi;
using BO;

using System.Threading.Tasks;

namespace BlImplementation;

internal class UserImplementation : IUser
{
    private DalApi.IDal _dal = Factory.Get;
    public void Create(BO.User user)
    {
        try
        {
            DO.User toDal = new DO.User()
            {
                UserId = user.UserId,
                Password = user.Password,
                IsAdmin = user.IsAdmin
            };
            _dal.User.Create(toDal);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID: {user.UserId} already exist", ex);
        }
    }
    public void Delete(int id)
    {
        if (!ExistUser(id))
            throw new BO.BlDoesNotExistException($"Task with ID: {id} does not exist");
    }

    public bool ExistUser(int id)
    {
        return _dal.User.ExistUser(id);
    }
}
