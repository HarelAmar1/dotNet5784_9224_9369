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
    string Name {  get; init; }
    string Email {  get; init; }
    EngineerExperience Level {  get; init; }
    double Cost {  get; set; }
    TaskInEngineer Task{  get; set; }
};