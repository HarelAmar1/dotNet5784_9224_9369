namespace Dal
{
    using DalApi;
    using DO;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class EngineerImplementation : IEngineer
    {
        /// <summary>
        /// Creates a new engineer in the DAL layer.
        /// </summary>
        /// <param name="item">The engineer to add to the DAL layer.</param>
        /// <returns>The ID of the added engineer.</returns>
        /// <exception cref="DalAlreadyExistsException">Thrown if an engineer with the same ID already exists.</exception>
        public int Create(Engineer item)
        {
            if (DataSource.Engineers.Any(engineer => engineer.Id == item.Id))
                throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");

            DataSource.Engineers.Add(item);
            return item.Id;
        }

        /// <summary>
        /// Deletes an engineer by ID.
        /// </summary>
        /// <param name="id">The ID of the engineer to delete.</param>
        /// <exception cref="DalDoesNotExistException">Thrown if no engineer with the given ID exists.</exception>
        public void Delete(int id)
        {
            var engineer = DataSource.Engineers.FirstOrDefault(e => e.Id == id);
            if (engineer != null)
                DataSource.Engineers.Remove(engineer);
            else
                throw new DalDoesNotExistException($"Engineer with ID={id} does not exist");
        }

        /// <summary>
        /// Reads an engineer by ID.
        /// </summary>
        /// <param name="id">The ID of the engineer to read.</param>
        /// <returns>The engineer with the given ID.</returns>
        public Engineer? Read(int id)
        {
            return DataSource.Engineers.FirstOrDefault(e => e.Id == id);
        }

        /// <summary>
        /// Reads an engineer based on the provided filter.
        /// </summary>
        /// <param name="filter">The filter predicate to apply.</param>
        /// <returns>The engineer that matches the filter predicate.</returns>
        public Engineer? Read(Func<Engineer, bool> filter)
        {
            return DataSource.Engineers.FirstOrDefault(filter);
        }

        /// <summary>
        /// Reads all engineers optionally filtered by a predicate.
        /// </summary>
        /// <param name="filter">The filter predicate to apply.</param>
        /// <returns>The list of engineers filtered by the predicate.</returns>
        public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
        {
            return filter != null ? DataSource.Engineers.Where(filter) : DataSource.Engineers;
        }

        /// <summary>
        /// Updates an engineer's information.
        /// </summary>
        /// <param name="item">The updated engineer object.</param>
        /// <exception cref="DalDoesNotExistException">Thrown if no engineer with the given ID exists.</exception>
        public void Update(Engineer item)
        {
            var existingEngineer = DataSource.Engineers.FirstOrDefault(e => e.Id == item.Id);
            if (existingEngineer != null)
            {
                DataSource.Engineers.Remove(existingEngineer);
                DataSource.Engineers.Add(item);
            }
            else
            {
                throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exist");
            }
        }
    }
}
