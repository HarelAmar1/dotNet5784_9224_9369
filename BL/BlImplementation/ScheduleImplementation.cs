using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BlApi;

namespace BlImplementation
{
    internal class ScheduleImplementation : ISchedule
    {
        private DalApi.IDal _dal = Factory.Get;

        /// <summary>
        /// Retrieves the end date of the project.
        /// </summary>
        /// <returns>The end date of the project.</returns>
        public DateTime? getEndDateOfProject()
        {
            return _dal.Schedule.getEndDateOfProject();
        }

        /// <summary>
        /// Retrieves the start date of the project.
        /// </summary>
        /// <returns>The start date of the project.</returns>
        public DateTime? getStartDateOfProject()
        {
            return _dal.Schedule.getStartDateOfProject();
        }

        /// <summary>
        /// Sets the end date of the project.
        /// </summary>
        /// <param name="endDate">The end date to set for the project.</param>
        public void setEndDateOfProject(DateTime endDate)
        {
            _dal.Schedule.setEndDateOfProject(endDate);
        }

        /// <summary>
        /// Sets the start date of the project.
        /// </summary>
        /// <param name="startDate">The start date to set for the project.</param>
        public void setStartDateOfProject(DateTime startDate)
        {
            _dal.Schedule.setStartDateOfProject(startDate);
        }
    }
}
