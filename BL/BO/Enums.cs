using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;


enum EngineerExperience
{
    Beginner,
    AdvancedBeginner,
    Intermediate,
    Advanced,
    Expert
}


enum Status
{
    Unscheduled,
    Scheduled,
    OnTrack,
    InJeopardy,
    Done
}