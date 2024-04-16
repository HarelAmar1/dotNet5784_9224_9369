using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BO;

public class Engineer
{
   public int Id {  get; init; }
    public string Name {  get; set; }
    public string Email {  get; set; }
    public EngineerExperience Level { get; init; }
    public double Cost {  get; set; }
    public TaskInEngineer? Task {  get; set; }
    public override string ToString() => this.ToStringProperty();

};