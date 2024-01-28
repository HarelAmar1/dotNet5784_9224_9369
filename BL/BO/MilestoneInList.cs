using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class MilestoneInList
{
    int Id {  get; init; }
    string Description {  get; init; }
    string Alias {  get; init; }
    Status Status {  get; set; }
    double CompletionPercentage {  get; set; }
};