using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BO;

public class Engineer
{
    int Id {  get; init; }   
    string Name;
    string Email;
    EngineerExperience Level;
    double Cost;
    TaskInEngineer Task;
};