
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
            throw new DalDoesNotExistException($"ID: {user.UserId}, not exist");


        List<DO.User> listTask = XMLTools.LoadListFromXMLSerializer<DO.User>(s_tasks_xml);// this is root
        if (listTask.Any(u => u.UserId == user.UserId))//if the user exist
            throw new DalAlreadyExistsException($"User with Id: {user.UserId} already exists");
        listTask.Add(user);//insert to list
        XMLTools.SaveListToXMLSerializer<DO.User>(listTask, s_tasks_xml); //return to XML file
    }

    public void Delete(int id)
    {
        List<DO.User> listTask = XMLTools.LoadListFromXMLSerializer<DO.User>(s_tasks_xml);// this is root
        if (!listTask.Any(user => user.UserId == id))//Looking for the ID to delete
            throw new DalDoesNotExistException($"ID: {id}, not exist");
        listTask.RemoveAll(U => U.UserId == id);
        XMLTools.SaveListToXMLSerializer<DO.User>(listTask, s_tasks_xml);//return to XML file
    }
}
