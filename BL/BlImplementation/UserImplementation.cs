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
            throw new BO.BlAlreadyExistsException($"ID: {user.UserId} already exist", ex);
        }
    }
    public void Delete(int id)
    {
        if (Read(id) == null)
            throw new BO.BlDoesNotExistException($"ID: {id} does not exist");
    }

    public User Read(int id)
    {
        DO.User user = _dal.User.Read(id);
        if (user == null)
            throw new BO.BlDoesNotExistException($"ID: {id} does not exist");
        BO.User read = new User { UserId = user.UserId, Password = user.Password, IsAdmin = user.IsAdmin };
        return read;
    }
}
