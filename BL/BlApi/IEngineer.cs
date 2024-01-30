namespace BlApi;

public interface IEngineer
{
    //(בקשת רשימת מהנדסים (עבור מסך מנהל
    public IEnumerable<BO.EngineerInTask> ReadAll(Func<DO.Task?, bool>? func = null);

    //(בקשת פרטי מהנדס (עבור מסך מהנדס
    public BO.Engineer Read(int id);

    //(הוספת מהנדס (עבור מסך מנהל
    public void Create(BO.Engineer engineerToAdd);

    //(מחיקת מהנדס(עבור מסך מנהל
    public void Delete(int id);

    //עדכון נתוני מהנדס 
    public void Update(BO.Engineer engineerToUpdate);
}