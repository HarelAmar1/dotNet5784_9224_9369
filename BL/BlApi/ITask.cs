using System.Reflection.Emit;

namespace BlApi;

//פעולות על משימה: קובץ תיאור כללי עמוד 17
public interface ITask
{
    //Task list request
    public IEnumerable<BO.TaskInList> ReadAll(Func<DO.Task?, bool>? func = null);

    //Task details request
    public BO.Task Read(int idTask);

    //Adding a task
    public int Create(BO.Task task);

    //Update task
    public void Update(BO.Task task);

    //Deleting a task
    public void Delete(int idTask);

    //Update or add a task's scheduled start date
    public void startDateTimeManagement(int IdTask, DateTime dateTime);

    //A date generator for all tasks, activated in the transition phase
    public void dateGeneratorOfAllTasks();
}
