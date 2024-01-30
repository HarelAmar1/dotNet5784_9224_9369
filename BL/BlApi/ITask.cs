namespace BlApi;

//פעולות על משימה: קובץ תיאור כללי עמוד 17
public interface ITask
{
    //בקשת רשימת משימות
    public IEnumerable<BO.TaskInList> ReadAll(Func<DO.Task?, bool>? func = null);

    //בקשת פרטי משימה
    public BO.Task Read(int idTask);

    //הוספת משימה
    public int Create(BO.Task task);

    //עדכון משימה
    public void Update(BO.Task task);

    //מחיקת משימה
    public void Delete(int idTask);

    //עדכון או הוספת תאריך התחלה מתוכנן של  משימה
    public void startDateTimeManagement(BO.Task task);
}
