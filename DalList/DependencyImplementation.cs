namespace Dal
{
    using DalApi;
    using DO;
    using System;
    using System.Collections.Generic;

    internal class DependencyImplementation : IDependency
    {
        /// <summary>
        /// Creates a new dependency.
        /// </summary>
        /// <param name="item">The dependency to create.</param>
        /// <returns>The ID of the created dependency.</returns>
        public int Create(Dependency item)
        {
            Dependency newItem = item with { Id = DataSource.Config.NextstartDependencyId };
            DataSource.Dependencies.Add(newItem);
            return newItem.Id;
        }

        /// <summary>
        /// Deletes a dependency by its ID.
        /// </summary>
        /// <param name="id">The ID of the dependency to delete.</param>
        /// <exception cref="DalDoesNotExistException">Thrown when the dependency with the given ID does not exist.</exception>
        public void Delete(int id)
        {
            var dependencyToRemove = DataSource.Dependencies.FirstOrDefault(dependency => dependency.Id == id);
            if (dependencyToRemove != null)
            {
                DataSource.Dependencies.Remove(dependencyToRemove);
            }
            else
            {
                throw new DalDoesNotExistException($"Dependency with ID: {id} does not exist");
            }
        }

        /// <summary>
        /// Retrieves a dependency by its ID.
        /// </summary>
        /// <param name="id">The ID of the dependency to retrieve.</param>
        /// <returns>The dependency with the given ID.</returns>
        public Dependency? Read(int id)
        {
            return DataSource.Dependencies.FirstOrDefault(dependency => dependency.Id == id);
        }

        /// <summary>
        /// Retrieves dependencies based on a filter function.
        /// </summary>
        /// <param name="filter">The filter function.</param>
        /// <returns>Dependencies filtered by the given filter function.</returns>
        public Dependency? Read(Func<Dependency, bool> filter)
        {
            if (filter != null)
            {
                return DataSource.Dependencies.FirstOrDefault(filter);
            }
            return null;
        }

        /// <summary>
        /// Retrieves all dependencies, optionally filtered by a given filter function.
        /// </summary>
        /// <param name="filter">The optional filter function.</param>
        /// <returns>All dependencies, optionally filtered by the given filter function.</returns>
        public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
        {
            if (filter != null)
            {
                return DataSource.Dependencies.Where(filter);
            }
            return DataSource.Dependencies;
        }

        /// <summary>
        /// Updates an existing dependency.
        /// </summary>
        /// <param name="item">The dependency to update.</param>
        /// <exception cref="DalDoesNotExistException">Thrown when the dependency with the given ID does not exist.</exception>
        public void Update(Dependency item)
        {
            var existingDependency = DataSource.Dependencies.FirstOrDefault(dependency => dependency.Id == item.Id);
            if (existingDependency != null)
            {
                DataSource.Dependencies.Remove(existingDependency);
                DataSource.Dependencies.Add(item);
            }
            else
            {
                throw new DalDoesNotExistException($"Dependency with ID: {item.Id} does not exist");
            }
        }
    }
}
