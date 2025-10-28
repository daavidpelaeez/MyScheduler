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

                case Enums.ScheduleType.DailyOnce:
                    return $"Occurs every {scheduleConfig.Recurrence} day(s). Next on {scheduleConfig.EventDate!.Value.Date.ToShortDateString()}, starting {scheduleConfig.StartDate.Date.ToShortDateString()}";

                case Enums.ScheduleType.WeeklyOnce:
                    return $"Occurs every {scheduleConfig.WeeklyRecurrence} weeks on {GetWeeklyDescription(scheduleConfig.DaysOfWeek)} at {scheduleConfig.ExecutionTimeOfOneDay}, starting {scheduleConfig.StartDate.Date.ToShortDateString()}";

                case Enums.ScheduleType.WeeklyEvery:
                    return $"Occurs every {scheduleConfig.WeeklyRecurrence} weeks on {GetWeeklyDescription(scheduleConfig.DaysOfWeek)} every {scheduleConfig.TimeUnitNumberOf} {scheduleConfig.TimeUnit.ToString().ToLower()} between {scheduleConfig.DailyStartTime!.Value} " +
                        $"and {scheduleConfig.DailyEndTime!.Value} starting on {scheduleConfig.StartDate.Date.ToShortDateString()}";

                case Enums.ScheduleType.DailyEvery:
                    return $"Occurs every {scheduleConfig.Recurrence} day(s) from {scheduleConfig.DailyStartTime} to {scheduleConfig.DailyEndTime} every {scheduleConfig.TimeUnitNumberOf} {scheduleConfig.TimeUnit.ToString().ToLower()} ";

                default:
                    return "Description not available for this task type.";
            }
        }

        public static string GetWeeklyDescription(List<DayOfWeek>? days)
        {
            if (days == null || days.Count == 0)
                return "No days specified";

            if (days.Count == 1)
                return days[0].ToString().ToLower();

            if (days.Count == 2)
                return $"{days[0].ToString().ToLower()} and {days[1].ToString().ToLower()}";

            var allExceptLast = string.Join(", ", days.Take(days.Count-1)).ToLower();

            return $"{allExceptLast} and {days.Last()}".ToLower();
        }
    }
}
