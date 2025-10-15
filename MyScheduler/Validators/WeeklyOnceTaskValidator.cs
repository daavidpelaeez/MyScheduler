using MyScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class WeeklyOnceTaskValidator
    {
        public static void Validate(TaskEntity task, StringBuilder errors)
        {
            if (task.DaysOfWeek == null || task.DaysOfWeek.Count < 1)
                errors.AppendLine("DaysOfWeek must be selected for WeeklyOnce.");

            if (task.WeeklyRecurrence < 1)
                errors.AppendLine("WeeklyRecurrence must be at least 1.");

            if (task.ExecutionTimeOfOneDay == null)
                errors.AppendLine("ExecutionTimeOfOneDay is required for WeeklyOnce.");

            if (task.DailyStartTime != null)
                errors.AppendLine("DailyStartTime should not be set for WeeklyOnce.");

            if (task.DailyEndTime != null)
                errors.AppendLine("DailyEndTime should not be set for WeeklyOnce.");

            if (task.TimeUnit != null)
                errors.AppendLine("TimeUnit should not be set for WeeklyOnce.");

            if (task.TimeUnitNumberOf.HasValue)
                errors.AppendLine("TimeUnitNumberOf should not be set for WeeklyOnce.");
        }
    }


}
