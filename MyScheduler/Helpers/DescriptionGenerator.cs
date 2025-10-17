using MyScheduler.Entities;
using MyScheduler.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyScheduler.Services.Helpers
{
    public static class DescriptionGenerator
    {
        public static string GetDescription(TaskEntity taskConfig)
        {
            switch (taskConfig.TypeTask)
            {
                case TypeTask.Once:
                    return $"Occurs once. Schedule on {taskConfig.EventDate!.Value.Date.ToShortDateString()} at {taskConfig.EventDate.Value.ToLocalTime().DateTime.ToShortTimeString()}, starting {taskConfig.StartDate.Date.ToShortDateString()}";

                case TypeTask.Recurring:
                    return $"Occurs every {taskConfig.Recurrence} day(s). Next on {taskConfig.EventDate!.Value.Date.ToShortDateString()}, starting {taskConfig.StartDate.Date.ToShortDateString()}";

                case TypeTask.WeeklyOnce:
                    return $"Occurs every {taskConfig.WeeklyRecurrence} weeks on {GetWeeklyDescription(taskConfig.DaysOfWeek)} at {taskConfig.ExecutionTimeOfOneDay}, starting {taskConfig.StartDate.Date.ToShortDateString()}";

                case TypeTask.WeeklyEvery:
                    return $"Occurs every {taskConfig.WeeklyRecurrence} weeks on {GetWeeklyDescription(taskConfig.DaysOfWeek)} every {taskConfig.TimeUnitNumberOf} {taskConfig.TimeUnit.ToString().ToLower()} between {taskConfig.DailyStartTime!.Value.Hours}:{taskConfig.DailyStartTime!.Value.Minutes} " +
                        $"and {taskConfig.DailyEndTime!.Value.Hours}:{taskConfig.DailyEndTime!.Value.Minutes} starting on {taskConfig.StartDate.Date.ToShortDateString()}";

                default:
                    return "Description not available for this task type.";
            }
        }

        private static string GetWeeklyDescription(List<DayOfWeek>? days)
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
