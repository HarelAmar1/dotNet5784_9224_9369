
using BO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlApi;

public interface  IMilestone
{
    //(יצירת משימות (שלב מקדים
    public IEnumerable<BO.MilestoneInList> milestonesList();//לשים לב איזה פרמטרים צריך להכניס

    //קביעת תאריכים מתוכננים לביצוע המשימות - מעבר משלב התכנון לשלב הביצוע
    //הפונקציה מקבלת משימה והיא יוצרת לו תאריך התחלה ע"פ שאר המשימות שהוא תלוי בהם
    //הפונקציה מחזירה את התאריך סיום המאוחר ביותר מבין כל התלויות בהם
    public DateTime dateTimeGenerator(BO.Task task);

   
   //public IEnumerable<BO.MilestoneInList> ReturnsTheEngineerListOfMilestones();
}
