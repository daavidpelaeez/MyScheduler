using MyScheduler.Domain.Entities;
using MyScheduler.Helpers;
using System;
using System.Text;

namespace MyScheduler.Domain.Validators
{
    public static class RecurringWeeklyOnceValidator
    {
        public static void Validate(ScheduleEntity scheduleConfig, StringBuilder errors)
        {
            DailyFrequencyCommonRules.CheckDailyCommonRules(scheduleConfig, errors);

            if (scheduleConfig.DaysOfWeek == null || scheduleConfig.DaysOfWeek.Count < 1)
            {
                errors.AppendLine("DaysOfWeek must be selected for RecurringWeeklyRange.");
                return;
            }

            if (scheduleConfig.DailyOnceExecutionTime < TimeSpan.Zero)
                errors.AppendLine("DailyOnceExecutionTime is wrong");

            if (scheduleConfig.WeeklyRecurrence < 1)
                errors.AppendLine("WeeklyRecurrence must be at least 1.");

            if (WeeklyScheduleHelper.SameWeekDayChecker(scheduleConfig.DaysOfWeek!))
                errors.AppendLine("Check days of the week they cant be repeated");

            if (!scheduleConfig.WeeklyRecurrence.HasValue || scheduleConfig.WeeklyRecurrence < 1)
                errors.AppendLine("WeeklyRecurrence must be at least 1.");

        }
    }
}
