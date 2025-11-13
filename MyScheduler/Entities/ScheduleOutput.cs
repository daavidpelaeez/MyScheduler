using System;

namespace MyScheduler.Entities
{
    public struct ScheduleOutput 
    {
        public DateTimeOffset ExecutionTime { get; set; } 
        public string Description { get; set; } 
    }
}
