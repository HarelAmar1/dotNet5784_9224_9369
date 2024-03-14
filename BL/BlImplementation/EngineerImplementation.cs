using BlApi;
using BO;
using DO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BlImplementation
{
    internal class EngineerImplementation : IEngineer
    {
        private DalApi.IDal _dal = Factory.Get;

        /// <summary>
        /// Adds an engineer to the DAL layer.
        /// </summary>
        /// <param name="engineerToAdd">The engineer to add.</param>
        /// <returns>Returns the ID of the added engineer.</returns>
        /// <exception cref="BlIncorrectInputException">Thrown when the input is incorrect.</exception>
        /// <exception cref="BlDoesNotExistException">Thrown when the engineer does not exist.</exception>
        public int Create(BO.Engineer engineerToAdd)
        {
            IEnumerable<BO.Engineer> engineers = (from item in _dal.Engineer.ReadAll().ToList()
                                                  let tasks = _dal.Task.ReadAll()
                                                  select new BO.Engineer()
                                                  {
                                                      Id = item.Id,
                                                      Name = item.Name,
                                                      Cost = item.Cost,
                                                      Email = item.Email,
                                                      Level = (BO.EngineerExperience)(int)item.level,
                                                      Task = (from task in tasks
                                                              where (task.EngineerId == item.Id)
                                                              select new TaskInEngineer(task.Id, task.Alias)
                                                              ).FirstOrDefault()
                                                  });

            try
            {
                var mailAddress = new MailAddress(engineerToAdd.Email);
            }
            catch (FormatException)
            {
                throw new BlIncorrectInputException($"Email={engineerToAdd.Email}, is incorrect input");
            }

            string error = "";
            if (engineerToAdd.Id < 0)
                error = $"Id: {engineerToAdd.Id}";
            else if (engineerToAdd.Name == "")
                error = $"Name: {engineerToAdd.Name}";
            else if (engineerToAdd.Cost < 0)
                error = $"Cost: {engineerToAdd.Cost}";
            else if ((int)engineerToAdd.Level >= 5)
                error = $"Level: {engineerToAdd.Level}";
            if (error != "")
                throw new BlIncorrectInputException($"{error}, is incorrect input");

            if (!engineers.Any(engineer => engineer?.Id == engineerToAdd.Id))
                throw new BlDoesNotExistException($"Engineer with ID: {engineerToAdd.Id} Does Not exist");

            if (engineerToAdd.Task != null)
            {
                DO.Task? taskToChangeInDal = (from task in _dal.Task.ReadAll()
                                              where task.Id == engineerToAdd.Task.Id
                                              select task).FirstOrDefault();

                DO.Task taskToChangeInDalrecord = taskToChangeInDal with { EngineerId = engineerToAdd.Id };
                _dal.Task.Update(taskToChangeInDalrecord);
            }

            DO.Engineer becomeDO = new DO.Engineer(engineerToAdd.Id, engineerToAdd.Email, engineerToAdd.Cost, engineerToAdd.Name, (DO.EngineerExperience)(int)engineerToAdd.Level);
            _dal.Engineer.Create(becomeDO);

            return engineerToAdd.Id;
        }

        /// <summary>
        /// Retrieves an engineer based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the engineer to retrieve.</param>
        /// <returns>Returns the engineer with the specified ID.</returns>
        /// <exception cref="BlDoesNotExistException">Thrown when the engineer does not exist.</exception>
        public BO.Engineer Read(int id)
        {
            IEnumerable<DO.Task> tasks = _dal.Task.ReadAll().ToList();
            IEnumerable<BO.Engineer?> engineers = (from item in _dal.Engineer.ReadAll().ToList()
                                                   select new BO.Engineer()
                                                   {
                                                       Id = item.Id,
                                                       Name = item.Name,
                                                       Cost = item.Cost,
                                                       Email = item.Email,
                                                       Level = (BO.EngineerExperience)(int)item.level,
                                                       Task = (from task in tasks
                                                               where task.EngineerId == item.Id
                                                               select new TaskInEngineer(task.Id, task.Alias)
                                                               ).FirstOrDefault()
                                                   });

            BO.Engineer? EngineerToGet = engineers.FirstOrDefault(item => item.Id == id);

            if (EngineerToGet == null)
                throw new BlDoesNotExistException($"Engineer with ID: {id} Does Not exist");

            return EngineerToGet;
        }

        /// <summary>
        /// Updates the specified engineer.
        /// </summary>
        /// <param name="AnUpdatedEngineer">The engineer to update.</param>
        /// <exception cref="BlIncorrectInputException">Thrown when the input is incorrect.</exception>
        /// <exception cref="BlDoesNotExistException">Thrown when the engineer does not exist.</exception>
        public void Update(BO.Engineer AnUpdatedEngineer)
        {
            if (AnUpdatedEngineer.Task == null)
            {
                var taskToUpdate = _dal.Task.ReadAll().FirstOrDefault(T => T.EngineerId != null && T.EngineerId == AnUpdatedEngineer.Id);
                if (taskToUpdate != null)
                {
                    var newTask = taskToUpdate with { EngineerId = null };
                    _dal.Task.Update(newTask);
                }
            }

            IEnumerable<BO.Engineer?> engineers = (from item in _dal.Engineer.ReadAll().ToList()
                                                   let tasks = _dal.Task.ReadAll()
                                                   select new BO.Engineer()
                                                   {
                                                       Id = item.Id,
                                                       Name = item.Name,
                                                       Cost = item.Cost,
                                                       Email = item.Email,
                                                       Level = (BO.EngineerExperience)(int)item.level,
                                                       Task = (from task in tasks
                                                               where task.EngineerId == item.Id
                                                               select new TaskInEngineer(task.Id, task.Alias)).FirstOrDefault()
                                                   });

            BO.Engineer? EngineerToUp = engineers.FirstOrDefault(item => item.Id == AnUpdatedEngineer.Id);

            try
            {
                var mailAddress = new MailAddress(AnUpdatedEngineer.Email);
            }
            catch (FormatException)
            {
                throw new BlIncorrectInputException($"Email={AnUpdatedEngineer.Email}, is incorrect input");
            }

            string error = "";
            if (AnUpdatedEngineer.Id < 0)
                error = $"Id: {AnUpdatedEngineer.Id}";
            else if (AnUpdatedEngineer.Name == "")
                error = $"Name: {AnUpdatedEngineer.Name}";
            else if (AnUpdatedEngineer.Cost < 0)
                error = $"Cost: {AnUpdatedEngineer.Cost}";
            else if ((int)AnUpdatedEngineer.Level >= 5)
                error = $"Level: {AnUpdatedEngineer.Level}";
            if (error != "")
                throw new BlIncorrectInputException($"{error}, is incorrect input");

            if (AnUpdatedEngineer.Task != null)
            {
                if (EngineerToUp == null)
                {
                    error = $"Id: {AnUpdatedEngineer.Id}";
                    throw new BlDoesNotExistException($"{error} Does Not Exist");
                }

                IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();

                DO.Task taskToremoveInDal = tasks.FirstOrDefault(task => task.EngineerId == AnUpdatedEngineer.Id);
                if (taskToremoveInDal != null)
                {
                    taskToremoveInDal = taskToremoveInDal with { EngineerId = null };
                    _dal.Task.Update(taskToremoveInDal);
                }

                DO.Task taskToChangeInDal = tasks.FirstOrDefault(task => task.Id == AnUpdatedEngineer.Task.Id) with { EngineerId = AnUpdatedEngineer.Id };
                _dal.Task.Update(taskToChangeInDal);
            }

            BO.EngineerExperience changeUpTheLevel = (int)EngineerToUp.Level > (int)AnUpdatedEngineer.Level ? EngineerToUp.Level : AnUpdatedEngineer.Level;

            DO.Engineer becomeDO = new DO.Engineer(AnUpdatedEngineer.Id, AnUpdatedEngineer.Email, AnUpdatedEngineer.Cost, AnUpdatedEngineer.Name, (DO.EngineerExperience)(int)changeUpTheLevel);

            _dal.Engineer.Update(becomeDO);
        }

        /// <summary>
        /// Deletes the engineer with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the engineer to delete.</param>
        /// <exception cref="BlDoesNotExistException">Thrown when the engineer does not exist.</exception>
        /// <exception cref="BlCanNotBeDeletedException">Thrown when the engineer cannot be deleted.</exception>
        /// <exception cref="BlIncorrectInputException">Thrown when the input is incorrect.</exception>
        public void Delete(int id)
        {
            IEnumerable<BO.Engineer?> engineers = (from item in _dal.Engineer.ReadAll().ToList()
                                                   let tasks = _dal.Task.ReadAll()
                                                   select new BO.Engineer()
                                                   {
                                                       Id = item.Id,
                                                       Name = item.Name,
                                                       Cost = item.Cost,
                                                       Email = item.Email,
                                                       Level = (BO.EngineerExperience)(int)item.level,
                                                       Task = (from task in tasks
                                                               where task.EngineerId == item.Id
                                                               select new TaskInEngineer(task.Id, task.Alias)
                                                               ).FirstOrDefault()
                                                   });

            string error = "";

            if (id < 0)
                error = $"Id: {id}";
            else
            {
                if (!engineers.Any(engineer => engineer?.Id == id))
                    throw new BlDoesNotExistException($"Engineer with ID: {id} Does Not Exist");
                else
                {
                    BO.Engineer? toDelete = engineers.FirstOrDefault(item => item.Id == id);

                    TaskImplementation toCheckStatus = new TaskImplementation(new Bl());
                    IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();

                    if ((BO.Status)(toCheckStatus.Read(toDelete.Task.Id).Status) != BO.Status.Done && (BO.Status)(toCheckStatus.Read(toDelete.Task.Id).Status) != BO.Status.OnTrack)
                    {
                        DO.Task taskToremoveInDal = tasks.FirstOrDefault(task => task.EngineerId == id) with { EngineerId = null };
                        _dal.Task.Update(taskToremoveInDal);
                        _dal.Engineer.Delete(toDelete.Id);
                    }
                    else
                        throw new BlCanNotBeDeletedException($"Id: {toDelete.Id} Can Not Be Deleted");
                }
            }

            if (error != "")
                throw new BlIncorrectInputException($"{error}, is incorrect input");
        }

        /// <summary>
        /// Retrieves a list of engineers based on the provided filter.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>Returns a list of engineers according to the filter.</returns>
        public IEnumerable<BO.Engineer> ReadAll(Func<DO.Engineer?, bool>? filter = null)
        {
            IEnumerable<BO.Engineer?> engineers = (from item in _dal.Engineer.ReadAll(filter).ToList()
                                                   let tasks = _dal.Task.ReadAll()
                                                   select new BO.Engineer()
                                                   {
                                                       Id = item.Id,
                                                       Name = item.Name,
                                                       Cost = item.Cost,
                                                       Email = item.Email,
                                                       Level = (BO.EngineerExperience)(int)item.level,
                                                       Task = (from task in tasks
                                                               where task.EngineerId == item.Id
                                                               select new TaskInEngineer(task.Id, task.Alias)).FirstOrDefault()
                                                   });
            return engineers;
        }
    }
}
