namespace Dal
{
    using DalApi;
    using System;

    internal class ScheduleImplementation : ISchedule
    {
        /// <summary>
        /// Sets the start date of the project.
        /// </summary>
        /// <param name="startDate">The start date of the project.</param>
        public void setStartDateOfProject(DateTime startDate)
        {
            DataSource.Config.startProjectDate = startDate;
        }

        /// <summary>
        /// Sets the end date of the project.
        /// </summary>
        /// <param name="endDate">The end date of the project.</param>
        public void setEndDateOfProject(DateTime endDate)
        {
            DataSource.Config.endProjectDate = endDate;
        }

        /// <summary>
        /// Gets the start date of the project.
        /// </summary>
        /// <returns>The start date of the project.</returns>
        public DateTime? getStartDateOfProject()
        {
            return DataSource.Config.startProjectDate;
        }

        /// <summary>
        /// Gets the end date of the project.
        /// </summary>
        /// <returns>The end date of the project.</returns>
        public DateTime? getEndDateOfProject()
        {
            return DataSource.Config.endProjectDate;
        }
    }
}
