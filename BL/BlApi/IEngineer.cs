namespace BlApi;

public interface IEngineer
{

    //(הוספת מהנדס (עבור מסך מנהל
    public int Create(BO.Engineer engineerToAdd);

    //(בקשת פרטי מהנדס (עבור מסך מהנדס
    public BO.Engineer Read(int id);

    //עדכון נתוני מ
    //
    //
    //
    //נדס 
    public void Update(BO.Engineer engineerToUpdate);

    //(מחיקת מ
    //
    //
    //נדס(עבור מסך מנ
    //
    //
    //ל
    public void Delete(int id);

    //(בקשת רשימת מ
    //
    //
    //נדסים (עבור מסך מנהל
    public IEnumerable<BO.Engineer> ReadAll(Func<DO.Engineer?, bool>? func = null);
} 