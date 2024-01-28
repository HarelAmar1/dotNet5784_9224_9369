namespace BlApi;


//פעולות על משימה: קובץ תיאור כללי עמוד 17
public interface ITask
{
    //בקשת רשימת משימות
    public IEnumerable<BO.TaskInList> getTasksList(Func<DO.Task?, bool>? func = null);

    //בקשת פרטי משימה
    public Task getTask(int idTask);

    //הוספת משימה
    public void addTask(BO.Task task);

    //עדכון משימה
    public void updateTask(BO.Task task);

    //מחיקת משימה
    public void deleteTask(int idTask);

    //עדכון או הוספת תאריך התחלה מתוכנן של  משימה
    public void startDateTimeManagement(BO.Task task);
}
