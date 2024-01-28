namespace BO;

public class TaskInList
{
    int Id { get; init; }
    string Description { get; set; }
    string Alias { get; set; }
    Status? Status { get; set; }
    //public override string ToString() => this.ToStringProperty();   לבדוק איך לממש
};

