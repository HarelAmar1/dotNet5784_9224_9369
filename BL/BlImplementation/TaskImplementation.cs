using BlApi;
using BO;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    public void addTask(BO.Task task)
    {
        throw new NotImplementedException();
    }

    public void deleteTask(int idTask)
    {
        throw new NotImplementedException();
    }

    public System.Threading.Tasks.Task getTask(int idTask)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TaskInList> getTasksList(Func<DO.Task?, bool>? func = null)
    {
        throw new NotImplementedException();
    }

    public void startDateTimeManagement(BO.Task task)
    {
        throw new NotImplementedException();
    }

    public void updateTask(BO.Task task)
    {
        throw new NotImplementedException();
    }
}
