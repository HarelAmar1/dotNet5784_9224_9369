
using DalApi;
using DO;

namespace Dal;

internal class UserImplementation : IUser
{
    readonly string s_tasks_xml = "users";
    public void Create(User user)
    {
        //if the user is not Engineer throw exception
        EngineerImplementation engineer = new EngineerImplementation();
        if (engineer.Read(user.UserId) == null) 
            throw new DalDoesNotExistException($"ID: {user.UserId}, Not Engineer");


        List<DO.User> listUser = XMLTools.LoadListFromXMLSerializer<DO.User>(s_tasks_xml);// this is root
        if (Read(user.UserId) != null) //if the user exist
            throw new DalAlreadyExistsException($"User with Id: {user.UserId} already exists");
        listUser.Add(user);//insert to list
        XMLTools.SaveListToXMLSerializer<DO.User>(listUser, s_tasks_xml); //return to XML file
    }

    public void Delete(int id)
    {
        List<DO.User> listUser = XMLTools.LoadListFromXMLSerializer<DO.User>(s_tasks_xml);// this is root
        if (!listUser.Any(user => user.UserId == id))//Looking for the ID to delete
            throw new DalDoesNotExistException($"ID: {id}, Not Exist");
        listUser.RemoveAll(U => U.UserId == id);
        XMLTools.SaveListToXMLSerializer<DO.User>(listUser, s_tasks_xml);//return to XML file
    }

    public User? Read(int id)
    {
        List<DO.User> listUser = XMLTools.LoadListFromXMLSerializer<DO.User>(s_tasks_xml);// this is root
        return listUser.FirstOrDefault(U => U.UserId == id);
    }
}
