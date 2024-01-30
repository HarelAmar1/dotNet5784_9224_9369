using BlApi;
using BO;

namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{


    private DalApi.IDal _dal = Factory.Get;
    public void Create(BO.Engineer engineerToAdd)
    {
        //Converts the list of engineers from DO to BO
        IEnumerable<BO.Engineer?> engineers = (from item in _dal.Engineer.ReadAll().ToList()
                                               select new BO.Engineer()
                                               {
                                                   Id = item.Id,
                                                   Name = item.Name,
                                                   Cost = item.Cost,
                                                   Email = item.Email
                                               });

        if (engineerToAdd?.Id >= 0 && engineerToAdd?.Name != "" && engineerToAdd?.Cost >= 0)
        {
            if (engineers.Any(engineer => engineer?.Id == engineerToAdd.Id)) ;
            //  throw new DalAlreadyExistsException($"Engineer with ID={engineerToAdd.Id} already exists");
            else
                engineers.ToList().Add(engineerToAdd);
        }
    }
    public void Delete(int id)
    {
        IEnumerable<DO.Engineer?> engineers = _dal.Engineer.ReadAll().ToList();
        if (id >= 0)
        {
            if (engineers.Any(engineer => engineer?.Id != id)) ;
            //  throw new DalAlreadyExistsException($"Engineer with ID={id} already exists");
            else
            {
                DO.Engineer? toDelete = (from item in engineers where (item.Id == id) select item).FirstOrDefault();
                BO.Task? cheakTheId = new();
                if ( )
                {
                    engineers.ToList().Remove(toDelete);
                }
            }
        }
    }

    public BO.Engineer Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.EngineerInTask> ReadAll(Func<DO.Task?, bool>? func = null)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Engineer engineerToUpdate)
    {
        throw new NotImplementedException();
    }
}
