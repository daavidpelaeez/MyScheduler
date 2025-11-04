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

        public static string GetRecurringDescription(ScheduleEntity s)
        {
            string description = "";

            switch (s.Occurs)
            {
                case Enums.Occurs.Daily:

                    if (s.DailyFrequencyOnceCheckbox)
                    {
                        description = $"Occurs every {s.Recurrence} day(s) at {s.DailyOnceExecutionTime}, starting {s.StartDate.Date.ToShortDateString()}";
                    }
                    else if (s.DailyFrequencyRangeCheckbox)
                    {
                        description = $"Occurs every {s.Recurrence} day(s) every {s.TimeUnitNumberOf} {s.TimeUnit?.ToString().ToLower()} between {s.DailyStartTime} and {s.DailyEndTime}, starting {s.StartDate.Date.ToShortDateString()}";
                    }

                    break;

                case Enums.Occurs.Weekly:

                    var days = GetWeeklyDayList(s.DaysOfWeek);

                    if (s.DailyFrequencyOnceCheckbox)
                    {
                        description = $"Occurs every {s.WeeklyRecurrence} week(s) on {days} at {s.DailyOnceExecutionTime}, starting {s.StartDate.Date.ToShortDateString()}";
                    }
                    else if (s.DailyFrequencyRangeCheckbox)
                    {
                        description = $"Occurs every {s.WeeklyRecurrence} week(s) on {days} every {s.TimeUnitNumberOf} {s.TimeUnit?.ToString().ToLower()} between {s.DailyStartTime} and {s.DailyEndTime}, starting {s.StartDate.Date.ToShortDateString()}";
                    }
                    break;

                case Enums.Occurs.Monthly:

                    if (s.MonthlyFrequencyDayCheckbox)
                    {
                        description = $"Occurs day {s.MonthlyDayNumber} every {s.MonthlyDayRecurrence} month(s)";
                    }
                    else if (s.MonthlyFrequencyTheCheckbox)
                    {
                        description = $"Occurs the {s.MonthlyTheOrder} {s.MonthlyTheDayOfWeek} of every {s.MonthlyTheRecurrence} month(s)";
                    }

                    if (s.DailyFrequencyOnceCheckbox)
                    {
                        description += $" at {s.DailyOnceExecutionTime}";
                    }
                    else if (s.DailyFrequencyRangeCheckbox)
                    {
                        description += $" every {s.TimeUnitNumberOf} {s.TimeUnit?.ToString().ToLower()} between {s.DailyStartTime} and {s.DailyEndTime}";
                    }

                    description += $", starting {s.StartDate.Date.ToShortDateString()}";

                    break;

                default:
                    description = "Recurring schedule description not available.";
                    break;
            }

            return string.IsNullOrWhiteSpace(description) ? "Recurring schedule description not available." : description;
        }



        public static string GetWeeklyDayList(List<DayOfWeek> days)
        {
            if (days == null || days.Count == 0)
                return "no days specified";

            if (days.Count == 1)
                return days[0].ToString().ToLower();

            if (days.Count == 2)
                return $"{days[0].ToString().ToLower()} and {days[1].ToString().ToLower()}";

            string allExceptLast = string.Join(", ", days.Take(days.Count - 1)).ToLower();
            return $"{allExceptLast} and {days.Last().ToString().ToLower()}";
        }
    }
}
