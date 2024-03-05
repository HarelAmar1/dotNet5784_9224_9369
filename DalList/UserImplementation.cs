
using DalApi;
using Dal;
using DO;

namespace Dal;

internal class UserImplementation : IUser
{
    public void Create(User user)
    {
        //if the user is not Engineer throw exception
        EngineerImplementation engineer = new EngineerImplementation();
        if (engineer.Read(user.UserId) == null)
            throw new DalDoesNotExistException($"ID: {user.UserId}, not exist");

        //check if exsist in the List
        foreach (var obj in DataSource.Users)
        {
            if(obj.UserId == user.UserId)
                throw new DalAlreadyExistsException($"User with UserName: {obj.UserId} already exists");
        }
        DataSource.Users.Add(user);
    }
    public void Delete(int id)
    {
        bool exist = false; 
        foreach (var obj in DataSource.Users)
        {
            if (obj.UserId == id)
            {
                exist = true;
                DataSource.Users.Remove(obj); 
            }
        }
        if(exist == false)
            throw new DalDoesNotExistException($"ID: {id}, not exist");
    }
}
