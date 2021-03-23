using System;

namespace Domain.ActivityTracking
{
    public class Log
    {
       public Guid Id { get; set; }
       public string Activity { get; set; }
       public DateTime TimeOfTheActivity { get; set; } 
    }
}