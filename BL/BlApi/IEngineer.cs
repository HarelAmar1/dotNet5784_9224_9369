namespace BlApi;

public interface IEngineer
{

    //(הוספת מהנדס (עבור מסך מנהל
    public int Create(BO.Engineer engineerToAdd);

    //(בקשת פרטי מהנדס (עבור מסך מהנדס
    public BO.Engineer Read(int id);

    //עדכון נתוני מהנדס 
    public void Update(BO.Engineer engineerToUpdate);

    //(מחיקת מהנדס(עבור מסך מנהל
    public void Delete(int id);

    //(בקשת רשימת מהנדסים (עבור מסך מנהל
    public IEnumerable<BO.EngineerInTask> ReadAll(Func<DO.Engineer?, bool>? func = null);
}