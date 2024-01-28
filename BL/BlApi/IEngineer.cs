namespace BlApi;

public interface IEngineer
{
    //(בקשת רשימת מהנדסים (עבור מסך מנהל
    public IEnumerable<BO.EngineerInTask> getEngineersList(Func<DO.Task?, bool>? func = null);

    //(בקשת פרטי מהנדס (עבור מסך מהנדס
    public BO.Engineer getEngineer(int id);

    //(הוספת מהנדס (עבור מסך מנהל
    public void addEngineer(BO.Engineer engineerToAdd);

    //(מחיקת מהנדס(עבור מסך מנהל
    public void deleteEngineer(int id);

    //עדכון נתוני מהנדס 
    public void UpdateEngineer(BO.Engineer engineerToUpdate);
}