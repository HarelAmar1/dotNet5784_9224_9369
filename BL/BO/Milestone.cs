using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class Milestone
{
    int Id {  get; init; }
    string Description {  get; init; }
    string Alias {  get; init; }
    DateTime CreatedAtDate {  get; set; }
    Status Status {  get; set; }
    DateTime ForecastDate {  get; set; }
    DateTime DeadlineDate {  get; set; }
    DateTime CompleteDate {  get; set; }
    double CompletionPercentage {  get; set; }
    string Remarks {  get; set; }
    List<TaskInList> Dependencies {  get; set; }
};
