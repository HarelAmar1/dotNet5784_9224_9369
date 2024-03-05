
namespace BlApi;

public interface IUser
{
    void Create(BO.User user);
    void Delete(int id);
    BO.User Read(int id);

}
