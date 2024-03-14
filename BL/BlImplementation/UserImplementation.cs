using BlApi;
using BO;
using System.Threading.Tasks;

namespace BlImplementation
{
    internal class UserImplementation : IUser
    {
        private DalApi.IDal _dal = Factory.Get;

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <exception cref="BO.BlAlreadyExistsException">Thrown when the user already exists.</exception>
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
                throw new BO.BlAlreadyExistsException($"ID: {user.UserId} already exists", ex);
            }
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <exception cref="BO.BlDoesNotExistException">Thrown when the user does not exist.</exception>
        public void Delete(int id)
        {
            if (Read(id) == null)
                throw new BO.BlDoesNotExistException($"ID: {id} does not exist");
        }

        /// <summary>
        /// Reads a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to read.</param>
        /// <returns>Returns the user with the specified ID.</returns>
        /// <exception cref="BO.BlDoesNotExistException">Thrown when the user does not exist.</exception>
        public User Read(int id)
        {
            DO.User user = _dal.User.Read(id);
            if (user == null)
                throw new BO.BlDoesNotExistException($"ID: {id} does not exist");
            BO.User read = new User { UserId = user.UserId, Password = user.Password, IsAdmin = user.IsAdmin };
            return read;
        }
    }
}
