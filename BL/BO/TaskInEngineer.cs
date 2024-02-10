using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class TaskInEngineer
{
    public int Id { get; init; }
    public string Alias { get; set; }
    //public override string ToString() => this.ToStringProperty();   
   public TaskInEngineer(int id, string alias) { Id = id; Alias = alias; }
};