using MyScheduler.Entities;
using MyScheduler.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyScheduler.Services.Helpers
{
    public static class DescriptionGenerator
    {
        public static string GetDescription(ScheduleEntity scheduleConfig)
        {
            switch (scheduleConfig.Type)
            {
                case Enums.Type.Once:
                    return $"Occurs once. Schedule on {scheduleConfig.EventDate!.Value.Date.ToShortDateString()} at {scheduleConfig.EventDate.Value.DateTime.ToShortTimeString()}, starting {scheduleConfig.StartDate.Date.ToShortDateString()}";

                case Enums.Type.Recurring:
                    return $"Occurs every {scheduleConfig.Recurrence} day(s). Next on {scheduleConfig.EventDate!.Value.Date.ToShortDateString()}, starting {scheduleConfig.StartDate.Date.ToShortDateString()}";

                case Enums.Type.WeeklyOnce:
                    return $"Occurs every {scheduleConfig.WeeklyRecurrence} weeks on {GetWeeklyDescription(scheduleConfig.DaysOfWeek)} at {scheduleConfig.ExecutionTimeOfOneDay}, starting {scheduleConfig.StartDate.Date.ToShortDateString()}";

                case Enums.Type.WeeklyEvery:
                    return $"Occurs every {scheduleConfig.WeeklyRecurrence} weeks on {GetWeeklyDescription(scheduleConfig.DaysOfWeek)} every {scheduleConfig.TimeUnitNumberOf} {scheduleConfig.TimeUnit.ToString().ToLower()} between {scheduleConfig.DailyStartTime!.Value} " +
                        $"and {scheduleConfig.DailyEndTime!.Value} starting on {scheduleConfig.StartDate.Date.ToShortDateString()}";

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
