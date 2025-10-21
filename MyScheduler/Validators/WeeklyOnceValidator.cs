using MyScheduler.Entities;
using MyScheduler.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class WeeklyOnceValidator
    {
        public static void Validate(ScheduleEntity scheduleConfig, StringBuilder errors)
        {
            if (scheduleConfig.DaysOfWeek == null || scheduleConfig.DaysOfWeek.Count < 1)
                errors.AppendLine("DaysOfWeek must be selected for WeeklyOnce.");

            if(scheduleConfig.ExecutionTimeOfOneDay < TimeSpan.Zero)
                errors.AppendLine("ExecutionTimeOfOneDay is wrong");

            if (scheduleConfig.WeeklyRecurrence < 1)
                errors.AppendLine("WeeklyRecurrence must be at least 1.");

            if (scheduleConfig.ExecutionTimeOfOneDay == null)
                errors.AppendLine("ExecutionTimeOfOneDay is required for WeeklyOnce.");

            if (scheduleConfig.DailyStartTime != null)
                errors.AppendLine("DailyStartTime should not be set for WeeklyOnce.");

            if (scheduleConfig.DailyEndTime != null)
                errors.AppendLine("DailyEndTime should not be set for WeeklyOnce.");

            if (scheduleConfig.TimeUnit != null)
                errors.AppendLine("TimeUnit should not be set for WeeklyOnce.");

            if (scheduleConfig.TimeUnitNumberOf.HasValue)
                errors.AppendLine("TimeUnitNumberOf should not be set for WeeklyOnce.");

            if (WeeklyScheduleHelper.sameWeekDayChecker(scheduleConfig.DaysOfWeek!))
                errors.AppendLine("Check days of the week they cant be repeated");
        }
    }
}
