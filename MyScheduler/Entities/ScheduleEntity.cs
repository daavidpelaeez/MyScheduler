using MyScheduler.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Entities
{
    public class ScheduleEntity
    {
        public DateTimeOffset CurrentDate { get; set; } = DateTimeOffset.Now;
        public Enums.ScheduleType ScheduleType { get; set; } 
        public DateTimeOffset? EventDate { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public Occurs Occurs { get; set; }
        public int Recurrence { get; set; }
        public TimeUnit? TimeUnit { get; set; }
        public int TimeUnitNumberOf { get; set; } = 0;
        public List<DayOfWeek> DaysOfWeek { get; set; } = new List<DayOfWeek>(); 
        public int WeeklyRecurrence { get; set; }
        public TimeSpan? DailyOnceExecutionTime { get; set; }
        public TimeSpan? DailyStartTime { get; set; }    
        public TimeSpan? DailyEndTime { get; set; }      
        public bool Enabled { get; set; } 
        public bool DailyFrequencyOnceCheckbox { get; set; } 
        public bool DailyFrequencyEveryCheckbox { get; set; }

    }
}
