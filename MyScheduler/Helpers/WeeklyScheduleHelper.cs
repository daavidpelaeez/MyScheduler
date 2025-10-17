using MyScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Helpers
{
    public static class WeeklyScheduleHelper
    {
        public static List<DateTimeOffset> GetMatchingDays(TaskEntity config, int? maxOccurrences)
        {
            var result = new List<DateTimeOffset>();
            var end = config.EndDate ?? DateTimeOffset.MaxValue;

            for (var current = config.StartDate; current <= end; current = current.AddDays(1))
            {
                if (maxOccurrences.HasValue && result.Count >= maxOccurrences.Value)
                    break;

                if (current >= config.CurrentDate && config.DaysOfWeek.Contains(current.DayOfWeek))
                {
                    result.Add(current);
                }

                if (current.DayOfWeek == DayOfWeek.Sunday)
                {
                    current = current.AddDays((config.WeeklyRecurrence - 1) * 7);
                }
            }
            return result;
        }
    }
}
