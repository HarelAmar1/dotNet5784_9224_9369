using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal
{
    internal class TaskImplementation : ITask
    {
        readonly string s_tasks_xml = "tasks";

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="item">The task to create.</param>
        /// <returns>The ID of the created task.</returns>
        public int Create(DO.Task item)
        {
            List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
            int newId = XMLTools.GetAndIncreaseNextId("data-config", "NextTaskId");
            DO.Task updatedTask = item with { Id = newId }; // Update the ID
            listTask.Add(updatedTask); // Insert to list
            XMLTools.SaveListToXMLSerializer<DO.Task>(listTask, s_tasks_xml); // Save to XML file
            return newId;
        }

        /// <summary>
        /// Deletes a task by its ID.
        /// </summary>
        /// <param name="id">The ID of the task to delete.</param>
        /// <exception cref="DalDoesNotExistException">Thrown when the task with the specified ID does not exist.</exception>
        public void Delete(int id)
        {
            List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
            if (listTask.Any(task => task.Id == id)) // Check if the task exists
                listTask.RemoveAll(T => T.Id == id); // Remove the task
            else
                throw new DalDoesNotExistException($"ID: {id}, not exist");
            XMLTools.SaveListToXMLSerializer<DO.Task>(listTask, s_tasks_xml); // Save to XML file
        }

        /// <summary>
        /// Reads a task by its ID.
        /// </summary>
        /// <param name="id">The ID of the task to read.</param>
        /// <returns>The task with the specified ID, or null if not found.</returns>
        public DO.Task? Read(int id)
        {
            List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
            return listTask.FirstOrDefault(T => T.Id == id); // Return the task with the specified ID
        }

        /// <summary>
        /// Reads a task based on a filter.
        /// </summary>
        /// <param name="filter">The filter condition.</param>
        /// <returns>The first task that satisfies the filter condition, or null if not found.</returns>
        public DO.Task? Read(Func<DO.Task, bool> filter)
        {
            List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
            if (filter != null)
            {
                return listTask.FirstOrDefault(filter); // Return the first task that satisfies the filter condition
            }
            return null;
        }

        /// <summary>
        /// Reads all tasks based on a filter.
        /// </summary>
        /// <param name="filter">The filter condition.</param>
        /// <returns>All tasks that satisfy the filter condition, or all tasks if filter is null.</returns>
        public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
        {
            List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
            if (filter != null)
            {
                return listTask.Where(filter); // Return tasks that satisfy the filter condition
            }
            return listTask; // Return all tasks
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="item">The task to update.</param>
        /// <exception cref="DalDoesNotExistException">Thrown when the task with the specified ID does not exist.</exception>
        public void Update(DO.Task item)
        {
            List<DO.Task> listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
            if (listTask.Any(task => task.Id == item.Id)) // Check if the task exists
            {
                int index = listTask.FindIndex(e => e.Id == item.Id);
                listTask.RemoveAt(index);
                listTask.Add(item);
                XMLTools.SaveListToXMLSerializer<DO.Task>(listTask, s_tasks_xml); // Save to XML file
            }
            else
                throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");
        }
    }
}
