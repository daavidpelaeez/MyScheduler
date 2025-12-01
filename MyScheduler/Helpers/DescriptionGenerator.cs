using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Enums;
using MyScheduler.Infraestructure.Localizer;
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
            var language = scheduleConfig.Language;

            switch (scheduleConfig.ScheduleType)
            {
                case ScheduleType.Once:
                    return string.Format(
                        localizer.GetString("OccursOnce", language),
                        localizer.FormatDate(scheduleConfig.OnceTypeDateExecution!.Value, scheduleConfig, language), 
                        localizer.FormatTime(scheduleConfig.OnceTypeDateExecution!.Value.DateTime.TimeOfDay, language), 
                        localizer.FormatDate(scheduleConfig.StartDate, scheduleConfig, language) 
                    );

                case ScheduleType.Recurring:
                    return GetRecurringDescription(scheduleConfig, localizer, language);

                default:
                    return localizer.GetString("DescriptionNotAvailable", language);
            }
        }

        public static string GetRecurringDescription(ScheduleEntity schedule, Localizer localizer, string language)
        {
            string description = "";
            switch (schedule.Occurs)
            {
                case Occurs.Daily:
                    if (schedule.DailyFrequencyOnceCheckbox)
                    {
                        description = string.Format(
                            localizer.GetString("OccursEveryDayAt", language),
                            schedule.Recurrence,
                            localizer.FormatTime(schedule.DailyOnceExecutionTime!.Value, language),
                            localizer.FormatDate(schedule.StartDate, schedule, language)
                        );
                    }
                    else if (schedule.DailyFrequencyRangeCheckbox)
                    {
                        description = string.Format(
                            localizer.GetString("OccursEveryDayEveryUnit", language),
                            schedule.Recurrence,
                            schedule.TimeUnitNumberOf,
                            localizer.GetString(schedule.TimeUnit.ToString().ToLower(), language),
                            localizer.FormatTime(schedule.DailyStartTime!.Value, language) ,
                            localizer.FormatTime(schedule.DailyEndTime!.Value, language) ,
                            localizer.FormatDate(schedule.StartDate, schedule, language)
                        );
                    }
                    break;
                case Occurs.Weekly:
                    var days = GetWeeklyDayList(schedule.DaysOfWeek!, localizer, language);
                    if (schedule.DailyFrequencyOnceCheckbox)
                    {
                        description = string.Format(
                            localizer.GetString("OccursEveryWeekOnAt", language),
                            schedule.WeeklyRecurrence,
                            days,
                            localizer.FormatTime(schedule.DailyOnceExecutionTime!.Value, language),
                            localizer.FormatDate(schedule.StartDate, schedule, language)
                        );
                    }
                    else if (schedule.DailyFrequencyRangeCheckbox)
                    {
                        description = string.Format(
                            localizer.GetString("OccursEveryWeekOnEveryUnit", language),
                            schedule.WeeklyRecurrence,
                            days,
                            schedule.TimeUnitNumberOf,
                            localizer.GetString(schedule.TimeUnit.ToString().ToLower(), language),
                            localizer.FormatTime(schedule.DailyStartTime!.Value, language),
                            localizer.FormatTime(schedule.DailyEndTime!.Value, language),
                            localizer.FormatDate(schedule.StartDate, schedule, language)
                        );
                    }
                    break;
                case Occurs.Monthly:

                    if (schedule.MonthlyFrequencyDayCheckbox)
                    {
                        description = string.Format(
                            localizer.GetString("OccursDayEveryMonth", language),
                            schedule.MonthlyDayNumber,
                            schedule.MonthlyDayRecurrence
                        );
                    }
                    else if (schedule.MonthlyFrequencyTheCheckbox)
                    {
                        description = string.Format(
                            localizer.GetString("OccursTheOfEveryMonth", language),
                            localizer.GetString(schedule.MonthlyTheOrder.ToString(), language),
                            localizer.GetString(schedule.MonthlyTheDayOfWeek.ToString(), language),
                            schedule.MonthlyTheRecurrence
                        );
                    }

                    if (schedule.DailyFrequencyOnceCheckbox)
                    {
                        description += string.Format(localizer.GetString("AtTime", language),localizer.FormatTime(schedule.DailyOnceExecutionTime!.Value, language));
                    }
                    else if (schedule.DailyFrequencyRangeCheckbox)
                    {
                        description += string.Format(localizer.GetString("EveryUnitBetween", language), schedule.TimeUnitNumberOf, localizer.GetString(schedule.TimeUnit.ToString().ToLower(), language),  localizer.FormatTime(schedule.DailyStartTime!.Value, language), localizer.FormatTime(schedule.DailyEndTime!.Value, language));
                    }

                    description += string.Format(localizer.GetString("StartingOn", language), localizer.FormatDate(schedule.StartDate, schedule, language));

                    break;

                default:
                    description = localizer.GetString("RecurringDescriptionNotAvailable", language);
                    break;
            }
            return string.IsNullOrWhiteSpace(description) ? localizer.GetString("RecurringDescriptionNotAvailable", language) : description;
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
