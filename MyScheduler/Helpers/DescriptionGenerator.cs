using MyScheduler.Entities;
using MyScheduler.Localizers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyScheduler.Helpers
{
    public static class DescriptionGenerator
    {
        public static string GetDescription(ScheduleEntity scheduleConfig)
        {
            var localizer = new Localizer();
            var lang = scheduleConfig.language ?? "en-US";

            switch (scheduleConfig.ScheduleType)
            {
                case Enums.ScheduleType.Once:
                    return string.Format(
                        localizer.GetString("OccursOnce", lang),
                        localizer.FormatDate(scheduleConfig.OnceTypeDateExecution!.Value, scheduleConfig, lang),
                        scheduleConfig.OnceTypeDateExecution.Value.DateTime.ToShortTimeString(),
                        localizer.FormatDate(scheduleConfig.StartDate, scheduleConfig, lang)
                    );

                case Enums.ScheduleType.Recurring:
                    return GetRecurringDescription(scheduleConfig, localizer, lang);

                default:
                    return localizer.GetString("DescriptionNotAvailable", lang);
            }
        }

        public static string GetRecurringDescription(ScheduleEntity s, Localizer localizer, string lang)
        {
            string description = "";

            switch (s.Occurs)
            {
                case Enums.Occurs.Daily:
                    if (s.DailyFrequencyOnceCheckbox)
                    {
                        description = string.Format(
                            localizer.GetString("OccursEveryDayAt", lang),
                            s.Recurrence,
                            s.DailyOnceExecutionTime,
                            localizer.FormatDate(s.StartDate, s, lang)
                        );
                    }
                    else if (s.DailyFrequencyRangeCheckbox)
                    {
                        description = string.Format(
                            localizer.GetString("OccursEveryDayEveryUnit", lang),
                            s.Recurrence,
                            s.TimeUnitNumberOf,
                            s.TimeUnit?.ToString().ToLower(),
                            s.DailyStartTime,
                            s.DailyEndTime,
                            localizer.FormatDate(s.StartDate, s, lang)
                        );
                    }
                    break;

                case Enums.Occurs.Weekly:
                    var days = GetWeeklyDayList(s.DaysOfWeek, localizer, lang);
                    if (s.DailyFrequencyOnceCheckbox)
                    {
                        description = string.Format(
                            localizer.GetString("OccursEveryWeekOnAt", lang),
                            s.WeeklyRecurrence,
                            days,
                            s.DailyOnceExecutionTime,
                            localizer.FormatDate(s.StartDate, s, lang)
                        );
                    }
                    else if (s.DailyFrequencyRangeCheckbox)
                    {
                        description = string.Format(
                            localizer.GetString("OccursEveryWeekOnEveryUnit", lang),
                            s.WeeklyRecurrence,
                            days,
                            s.TimeUnitNumberOf,
                            s.TimeUnit?.ToString().ToLower(),
                            s.DailyStartTime,
                            s.DailyEndTime,
                            localizer.FormatDate(s.StartDate, s, lang)
                        );
                    }
                    break;

                case Enums.Occurs.Monthly:
                    if (s.MonthlyFrequencyDayCheckbox)
                    {
                        description = string.Format(
                            localizer.GetString("OccursDayEveryMonth", lang),
                            s.MonthlyDayNumber,
                            s.MonthlyDayRecurrence
                        );
                    }
                    else if (s.MonthlyFrequencyTheCheckbox)
                    {
                        description = string.Format(
                            localizer.GetString("OccursTheOfEveryMonth", lang),
                            s.MonthlyTheOrder,
                            s.MonthlyTheDayOfWeek,
                            s.MonthlyTheRecurrence
                        );
                    }

                    if (s.DailyFrequencyOnceCheckbox)
                    {
                        description += string.Format(localizer.GetString("AtTime", lang), s.DailyOnceExecutionTime);
                    }
                    else if (s.DailyFrequencyRangeCheckbox)
                    {
                        description += string.Format(localizer.GetString("EveryUnitBetween", lang), s.TimeUnitNumberOf, s.TimeUnit?.ToString().ToLower(), s.DailyStartTime, s.DailyEndTime);
                    }

                    description += string.Format(localizer.GetString("StartingOn", lang), localizer.FormatDate(s.StartDate, s, lang));
                    break;

                default:
                    description = localizer.GetString("RecurringDescriptionNotAvailable", lang);
                    break;
            }

            return string.IsNullOrWhiteSpace(description) ? localizer.GetString("RecurringDescriptionNotAvailable", lang) : description;
        }

        public static string GetWeeklyDayList(List<DayOfWeek> days, Localizer localizer, string language)
        {
            if (days == null || days.Count == 0)
                return localizer.GetString("NoDaysSpecified", language);

            if (days.Count == 1)
                return localizer.GetString(days[0].ToString(), language).ToLower();

            if (days.Count == 2)
                return string.Format(localizer.GetString("DayAndDay", language), localizer.GetString(days[0].ToString(), language).ToLower(), localizer.GetString(days[1].ToString(), language).ToLower());

            string allExceptLast = string.Join(", ", days.Take(days.Count - 1)).ToLower();

            return string.Format(localizer.GetString("DaysAndDay", language), allExceptLast, localizer.GetString(days.Last().ToString(), language).ToLower());
        }
    }
}
