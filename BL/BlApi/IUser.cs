namespace BlApi
{
    public interface IUser
    {
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user to create.</param>
        void Create(BO.User user);

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        void Delete(int id);

        /// <summary>
        /// Reads a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to read.</param>
        /// <returns>Returns the user with the specified ID.</returns>
        BO.User Read(int id);
    }
}
