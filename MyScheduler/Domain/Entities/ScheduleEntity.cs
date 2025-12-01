using MyScheduler.Domain.Enums;
using System;
using System.Collections.Generic;

namespace MyScheduler.Domain.Entities
{
    public class ScheduleEntity
    {
        public DateTimeOffset CurrentDate { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public bool Enabled { get; set; }
        public DateTimeOffset? OnceTypeDateExecution { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public Occurs Occurs { get; set; }
        public int? Recurrence { get; set; }
        public TimeUnit? TimeUnit { get; set; }
        public int? TimeUnitNumberOf { get; set; }
        public List<DayOfWeek>? DaysOfWeek { get; set; }
        public int? WeeklyRecurrence { get; set; }
        public TimeSpan? DailyOnceExecutionTime { get; set; }
        public TimeSpan? DailyStartTime { get; set; }    
        public TimeSpan? DailyEndTime { get; set; }      
        public bool DailyFrequencyOnceCheckbox { get; set; } 
        public bool DailyFrequencyRangeCheckbox { get; set; }
        public bool MonthlyFrequencyDayCheckbox { get; set; }
        public bool MonthlyFrequencyTheCheckbox { get; set; }
        public int? MonthlyDayNumber { get; set; }                    
        public int? MonthlyDayRecurrence { get; set; }                      
        public MonthlyTheOrder? MonthlyTheOrder { get; set; }                   
        public MonthlyDayOfWeek? MonthlyTheDayOfWeek { get; set; }               
        public int? MonthlyTheRecurrence { get; set; }                 
        public string Language { get; set; } = "en-UK";
        public string TimeZoneID { get; set; } = "Romance Standard Time";
    }
}
