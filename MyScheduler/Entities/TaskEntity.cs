using MyScheduler.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Entities
{
    public class TaskEntity
    {
        public DateTimeOffset CurrentDate { get; set; }
        public TypeTask TypeTask { get; set; }
        public DateTimeOffset? EventDate { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int Recurrence { get; set; }
        public TimeUnit? TimeUnit { get; set; }             
        public int? TimeUnitNumberOf { get; set; }
        public List<DayOfWeek> DaysOfWeek { get; set; } = new List<DayOfWeek>();
        public int WeeklyRecurrence { get; set; }
        public TimeSpan? ExecutionTimeOfOneDay { get; set; }
        public TimeSpan? DailyStartTime { get; set; }    
        public TimeSpan? DailyEndTime { get; set; }      

    }
}
