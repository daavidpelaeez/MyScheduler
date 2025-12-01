using MyScheduler.Domain.Entities;
using System;
using System.Collections.Generic;


namespace MyScheduler.Domain.Services.Calculators
{
    public class WeeklyCalculator
    {
        public  List<DateTimeOffset> GetMatchingDays(ScheduleEntity scheduleConfig, int? maxOccurrences)
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
    }
}
