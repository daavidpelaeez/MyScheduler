using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Entities
{
    public class ScheduleOutput
    {
        public DateTimeOffset ExecutionTime { get; set; } 
        public string Description { get; set; } = string.Empty; 

    }
}
