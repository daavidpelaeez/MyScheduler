using MyScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class WeeklyEveryTaskValidator
    {
        public static void Validate(TaskEntity taskConfig, StringBuilder errors)
        {
            if (taskConfig.DaysOfWeek == null || taskConfig.DaysOfWeek.Count < 1)
                errors.AppendLine("DaysOfWeek must be selected for WeeklyEvery.");

            if (taskConfig.WeeklyRecurrence < 1)
                errors.AppendLine("WeeklyRecurrence must be at least 1.");

            if (taskConfig.TimeUnit == null)
                errors.AppendLine("TimeUnit is required for WeeklyEvery.");

            if (!taskConfig.TimeUnitNumberOf.HasValue || taskConfig.TimeUnitNumberOf < 1)
                errors.AppendLine("TimeUnitNumberOf must be a positive number for WeeklyEvery.");

            if (taskConfig.DailyStartTime == null)
                errors.AppendLine("DailyStartTime is required for WeeklyEvery.");

            if (taskConfig.DailyEndTime == null)
                errors.AppendLine("DailyEndTime is required for WeeklyEvery.");

            if (taskConfig.ExecutionTimeOfOneDay != null)
                errors.AppendLine("ExecutionTimeOfOneDay should not be set for WeeklyEvery.");

            if(taskConfig.DailyStartTime > taskConfig.DailyEndTime)
                errors.AppendLine("DailyStartTime cannot be after the DailyEndTime.");

            if (taskConfig.DailyStartTime == taskConfig.DailyEndTime)
                errors.AppendLine("DailyStartTime cannot be the same as DailyEndTime.");

        }
    }
}
