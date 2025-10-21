﻿using MyScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

                if (current >= scheduleConfig.CurrentDate && scheduleConfig.DaysOfWeek.Contains(current.DayOfWeek))
                {
                    result.Add(current);
                }

                if (current.DayOfWeek == DayOfWeek.Sunday)
                {
                    current = current.AddDays((scheduleConfig.WeeklyRecurrence - 1) * 7);
                }
            }
            return result;
        }

        public static bool sameWeekDayChecker(List<DayOfWeek> daysOfWeek)
        {
            bool daysRepeated = daysOfWeek.Count != daysOfWeek.Distinct().Count();

            return daysRepeated;
        }
    }
}
