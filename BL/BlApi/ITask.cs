namespace BlApi;


//פעולות על משימה: קובץ תיאור כללי עמוד 17
public interface ITask
{
    //בקשת רשימת משימות
    public IEnumerable<BO.TaskInList> getTaskList(Func<DO.Task?, bool>? func = null);

    //בקשת פרטי משימה
    public Task getTask(int id);

    //הוספת משימה
    public void addTask(Task task);

    //עדכון משימה
    public void updateTask(Task task);

    //מחיקת משימה
    public void deleteTask(Task task);

    //עדכון או הוספת תאריך התחלה מתוכנן של  משימה
    public void startDateTimeManagement(Task task);
}
