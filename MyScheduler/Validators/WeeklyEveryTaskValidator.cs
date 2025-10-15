using MyScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class WeeklyEveryTaskValidator
    {
        public static void Validate(TaskEntity task, StringBuilder errors)
        {
            if (task.DaysOfWeek == null || task.DaysOfWeek.Count < 1)
                errors.AppendLine("DaysOfWeek must be selected for WeeklyEvery.");

            if (task.WeeklyRecurrence < 1)
                errors.AppendLine("WeeklyRecurrence must be at least 1.");

            if (task.TimeUnit == null)
                errors.AppendLine("TimeUnit is required for WeeklyEvery.");

            if (!task.TimeUnitNumberOf.HasValue || task.TimeUnitNumberOf < 1)
                errors.AppendLine("TimeUnitNumberOf must be a positive number for WeeklyEvery.");

            if (task.DailyStartTime == null)
                errors.AppendLine("DailyStartTime is required for WeeklyEvery.");

            if (task.DailyEndTime == null)
                errors.AppendLine("DailyEndTime is required for WeeklyEvery.");

            if (task.ExecutionTimeOfOneDay != null)
                errors.AppendLine("ExecutionTimeOfOneDay should not be set for WeeklyEvery.");
        }
    }
}
