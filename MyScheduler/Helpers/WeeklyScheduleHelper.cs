using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MyScheduler.Helpers
{
    public static class WeeklyScheduleHelper
    {
        public static List<DateTimeOffset> GetMatchingDays(ScheduleEntity scheduleConfig, int? maxOccurrences)
        {
            var result = new List<DateTimeOffset>();
            var end = scheduleConfig.EndDate ?? DateTimeOffset.MaxValue;

            for (var current = scheduleConfig.StartDate; current <= end; current = current.AddDays(1))
            {
                if (maxOccurrences.HasValue && result.Count >= maxOccurrences.Value)
                    break;

                if (current >= scheduleConfig.CurrentDate && scheduleConfig.DaysOfWeek != null && scheduleConfig.DaysOfWeek.Contains(current.DayOfWeek))
                {
                    result.Add(current);
                }

                if (current.DayOfWeek == DayOfWeek.Sunday)
                {
                    current = current.AddDays((scheduleConfig.WeeklyRecurrence!.Value - 1) * 7);
                }
            }
            return result;
        }

        public static bool SameWeekDayChecker(List<DayOfWeek> daysOfWeek)
        {
            bool daysRepeated = daysOfWeek.Count != daysOfWeek.Distinct().Count();

            return daysRepeated;
        }

       public static TimeSpan IntervalCalculator(ScheduleEntity scheduleConfig)
        {
            int timeUnitNumberOf = scheduleConfig.TimeUnitNumberOf!.Value;

            return scheduleConfig.TimeUnit switch
            {
                TimeUnit.Hours => TimeSpan.FromHours(timeUnitNumberOf),
                TimeUnit.Minutes => TimeSpan.FromMinutes(timeUnitNumberOf),
                TimeUnit.Seconds => TimeSpan.FromSeconds(timeUnitNumberOf),
                _ => throw new ArgumentException("Time unit not supported"),
            };
        }
    }
}
