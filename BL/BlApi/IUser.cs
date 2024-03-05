
namespace BlApi;

public interface IUser
{
    void Create(BO.User user);
    void Delete(int id);
    bool ExistUser(int id);

}
