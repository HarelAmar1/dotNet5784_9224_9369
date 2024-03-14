using DalApi;
using DO;
using System.Linq;

namespace Dal
{
    internal class UserImplementation : IUser
    {
        /// <summary>
        /// Creates a new user if they are an engineer.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <exception cref="DalDoesNotExistException">Thrown when the user is not an engineer.</exception>
        /// <exception cref="DalAlreadyExistsException">Thrown when a user with the same ID already exists.</exception>
        public void Create(User user)
        {
            // Check if the user is an engineer
            EngineerImplementation engineer = new EngineerImplementation();
            if (engineer.Read(user.UserId) == null)
                throw new DalDoesNotExistException($"ID: {user.UserId}, Not an Engineer");

            // Check if the user already exists
            if (Read(user.UserId) != null)
                throw new DalAlreadyExistsException($"User with UserName: {user.UserId} already exists");

            DataSource.Users.Add(user);
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <exception cref="DalDoesNotExistException">Thrown when the user with the specified ID does not exist.</exception>
        public void Delete(int id)
        {
            var user = DataSource.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
                throw new DalDoesNotExistException($"ID: {id}, does not exist");

            DataSource.Users.Remove(user);
        }

        /// <summary>
        /// Reads a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to read.</param>
        /// <returns>The user with the specified ID, or null if not found.</returns>
        public User? Read(int id)
        {
            return DataSource.Users.FirstOrDefault(u => u.UserId == id);
        }
    }
}
