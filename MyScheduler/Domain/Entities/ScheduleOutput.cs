using System;

namespace MyScheduler.Domain.Entities
{
    public struct ScheduleOutput 
    {
        public DateTimeOffset ExecutionTime { get; set; } 
        public string Description { get; set; } 
    }
}
