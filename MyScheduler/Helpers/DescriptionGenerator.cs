using MyScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyScheduler.Helpers
{
    public static class DescriptionGenerator
    {
        public static string GetDescription(ScheduleEntity scheduleConfig)
        {
            switch (scheduleConfig.ScheduleType)
            {
                case Enums.ScheduleType.Once:
                        return $"Occurs once. Schedule on {scheduleConfig.EventDate!.Value.Date.ToShortDateString()} at {scheduleConfig.EventDate.Value.DateTime.ToShortTimeString()}, starting {scheduleConfig.StartDate.Date.ToShortDateString()}";

                case Enums.ScheduleType.Recurring:
                    return GetRecurringDescription(scheduleConfig);

                default:
                    return "Description not available for this task type.";
            }
        }

        public static string GetRecurringDescription(ScheduleEntity scheduleConfig)
        {
            if (scheduleConfig.Occurs == Enums.Occurs.Daily)
            {
                if (scheduleConfig.DailyFrequencyOnceCheckbox)
                {
                   return $"Occurs every {scheduleConfig.Recurrence} day(s). Next on {scheduleConfig.EventDate!.Value.Date.ToShortDateString()}, starting {scheduleConfig.StartDate.Date.ToShortDateString()}";
                }

                if (scheduleConfig.DailyFrequencyEveryCheckbox)
                {
                    return $"Occurs every {scheduleConfig.Recurrence} day(s) from {scheduleConfig.DailyStartTime} to {scheduleConfig.DailyEndTime} every {scheduleConfig.TimeUnitNumberOf} {scheduleConfig.TimeUnit?.ToString().ToLower()}";
                }

            }
            else if (scheduleConfig.Occurs == Enums.Occurs.Weekly)
            {
                string days = GetWeeklyDescription(scheduleConfig.DaysOfWeek);

                if (scheduleConfig.DailyFrequencyOnceCheckbox)
                {
                    return $"Occurs every {scheduleConfig.WeeklyRecurrence} weeks on {days} at {scheduleConfig.DailyOnceExecutionTime}, starting {scheduleConfig.StartDate.Date.ToShortDateString()}";
                }
                else if (scheduleConfig.DailyFrequencyEveryCheckbox)
                {
                    return $"Occurs every {scheduleConfig.WeeklyRecurrence} weeks on {days} every {scheduleConfig.TimeUnitNumberOf} {scheduleConfig.TimeUnit?.ToString().ToLower()} between {scheduleConfig.DailyStartTime} and {scheduleConfig.DailyEndTime}, starting {scheduleConfig.StartDate.Date.ToShortDateString()}";
                }

            }

            return "Recurring schedule description not available.";
        }

        public static string GetWeeklyDescription(List<DayOfWeek> days)
        {
            if (days == null || days.Count == 0)
                return "no days specified";

            if (days.Count == 1)
                return days[0].ToString().ToLower();

            if (days.Count == 2)
                return $"{days[0].ToString().ToLower()} and {days[1].ToString().ToLower()}";

            string allExceptLast = string.Join(", ", days.Take(days.Count - 1).Select(d => d.ToString().ToLower()));
            return $"{allExceptLast} and {days.Last().ToString().ToLower()}";
        }
    }
}
