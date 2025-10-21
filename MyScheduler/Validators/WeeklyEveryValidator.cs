using MyScheduler.Entities;
using MyScheduler.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class WeeklyEveryValidator
    {
        public static void Validate(ScheduleEntity scheduleConfig, StringBuilder errors)
        {
            if (scheduleConfig.DaysOfWeek == null || scheduleConfig.DaysOfWeek.Count < 1)
                errors.AppendLine("DaysOfWeek must be selected for WeeklyEvery.");

            if (scheduleConfig.WeeklyRecurrence < 1)
                errors.AppendLine("WeeklyRecurrence must be at least 1.");

            if (scheduleConfig.TimeUnit == null)
                errors.AppendLine("TimeUnit is required for WeeklyEvery.");

            if (!scheduleConfig.TimeUnitNumberOf.HasValue || scheduleConfig.TimeUnitNumberOf < 1)
                errors.AppendLine("TimeUnitNumberOf must be a positive number for WeeklyEvery.");

            if (scheduleConfig.DailyStartTime == null)
                errors.AppendLine("DailyStartTime is required for WeeklyEvery.");

            if (scheduleConfig.DailyEndTime == null)
                errors.AppendLine("DailyEndTime is required for WeeklyEvery.");

            if (scheduleConfig.ExecutionTimeOfOneDay != null)
                errors.AppendLine("ExecutionTimeOfOneDay should not be set for WeeklyEvery.");

            if (scheduleConfig.DailyStartTime > scheduleConfig.DailyEndTime)
                errors.AppendLine("DailyStartTime cannot be after the DailyEndTime.");

            if (scheduleConfig.DailyStartTime == scheduleConfig.DailyEndTime)
                errors.AppendLine("DailyStartTime cannot be the same as DailyEndTime.");

            if (WeeklyScheduleHelper.sameWeekDayChecker(scheduleConfig.DaysOfWeek!))
                errors.AppendLine("Check days of the week they cant be repeated");

        }
    }
}
