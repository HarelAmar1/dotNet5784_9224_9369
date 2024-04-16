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
    public TaskInEngineer(int id, string alias) { Id = id; Alias = alias; }
    public override string ToString() => this.ToStringProperty();
};