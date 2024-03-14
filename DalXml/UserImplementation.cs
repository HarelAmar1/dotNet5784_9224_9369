using DalApi;
using DO;

namespace Dal
{
    internal class UserImplementation : IUser
    {
        readonly string s_tasks_xml = "users";

        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        /// <param name="user">The user object to create.</param>
        /// <exception cref="DalDoesNotExistException">Thrown when the user is not an engineer.</exception>
        /// <exception cref="DalAlreadyExistsException">Thrown when the user already exists.</exception>
        public void Create(User user)
        {
            // Check if the user is an engineer, throw exception if not
            EngineerImplementation engineer = new EngineerImplementation();
            if (engineer.Read(user.UserId) == null)
                throw new DalDoesNotExistException($"ID: {user.UserId}, Not Engineer");

            // Load existing users from XML
            List<User> listUser = XMLTools.LoadListFromXMLSerializer<User>(s_tasks_xml);

            // Check if the user already exists
            if (Read(user.UserId) != null)
                throw new DalAlreadyExistsException($"User with Id: {user.UserId} already exists");

            // Add the new user to the list and save to XML
            listUser.Add(user);
            XMLTools.SaveListToXMLSerializer<User>(listUser, s_tasks_xml);
        }

        /// <summary>
        /// Deletes a user from the system based on their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <exception cref="DalDoesNotExistException">Thrown when the user does not exist.</exception>
        public void Delete(int id)
        {
            // Load existing users from XML
            List<User> listUser = XMLTools.LoadListFromXMLSerializer<User>(s_tasks_xml);

            // Check if the user exists in the list
            if (!listUser.Any(user => user.UserId == id))
                throw new DalDoesNotExistException($"ID: {id}, Not Exist");

            // Remove the user from the list and save to XML
            listUser.RemoveAll(U => U.UserId == id);
            XMLTools.SaveListToXMLSerializer<User>(listUser, s_tasks_xml);
        }

        /// <summary>
        /// Reads a user from the system based on their ID.
        /// </summary>
        /// <param name="id">The ID of the user to read.</param>
        /// <returns>The user object if found, otherwise null.</returns>
        public User? Read(int id)
        {
            // Load existing users from XML
            List<User> listUser = XMLTools.LoadListFromXMLSerializer<User>(s_tasks_xml);

            // Find and return the user with the given ID, if exists
            return listUser.FirstOrDefault(U => U.UserId == id);
        }
    }
}
