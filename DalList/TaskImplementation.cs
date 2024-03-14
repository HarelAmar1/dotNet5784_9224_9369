namespace Dal
{
    using DalApi;
    using DO;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class TaskImplementation : ITask
    {
        /// <summary>
        /// Creates a new task in the data layer.
        /// </summary>
        /// <param name="item">The task to create.</param>
        /// <returns>The ID of the newly created task.</returns>
        public int Create(Task item)
        {
            Task newItem = item with { Id = DataSource.Config.NextStartTaskId };
            DataSource.Tasks.Add(newItem);
            return newItem.Id;
        }

        /// <summary>
        /// Deletes a task by its ID.
        /// </summary>
        /// <param name="id">The ID of the task to delete.</param>
        /// <exception cref="DalDoesNotExistException">Thrown when the task with the specified ID does not exist.</exception>
        public void Delete(int id)
        {
            var task = DataSource.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                throw new DalDoesNotExistException($"ID: {id}, does not exist");

            DataSource.Tasks.RemoveAll(t => t.Id == id);
        }

        /// <summary>
        /// Reads a task by its ID.
        /// </summary>
        /// <param name="id">The ID of the task to read.</param>
        /// <returns>The task with the specified ID, or null if not found.</returns>
        public Task? Read(int id)
        {
            return DataSource.Tasks.FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Reads a task based on a provided filter.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>The task that matches the filter.</returns>
        public Task? Read(Func<Task, bool> filter)
        {
            if (filter != null)
            {
                return DataSource.Tasks.FirstOrDefault(filter);
            }

            return null;
        }

        /// <summary>
        /// Reads all tasks based on a provided filter.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>A list of tasks that match the filter.</returns>
        public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
        {
            if (filter != null)
            {
                return DataSource.Tasks.Where(filter);
            }
            return DataSource.Tasks;
        }

        /// <summary>
        /// Updates a task in the data layer.
        /// </summary>
        /// <param name="item">The task to update.</param>
        /// <exception cref="DalDoesNotExistException">Thrown when the task with the specified ID does not exist.</exception>
        public void Update(Task item)
        {
            var existingTask = DataSource.Tasks.FirstOrDefault(t => t.Id == item.Id);
            if (existingTask == null)
                throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");

            Delete(item.Id);
            DataSource.Tasks.Add(item);
        }
    }
}
