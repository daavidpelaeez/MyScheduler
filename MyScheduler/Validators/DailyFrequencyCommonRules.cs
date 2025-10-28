using MyScheduler.Entities;
using MyScheduler.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class DailyFrequencyCommonRules
    {

        public static void CheckDailyCommonRules(ScheduleEntity schedulerConfig, StringBuilder errors)
        {
            // Validaciones básicas 
            if (schedulerConfig.DailyFrequencyOnce && schedulerConfig.DailyFrequencyEvery)
                errors.AppendLine("Daily frequency cannot be once and every at the same time");

            if (!schedulerConfig.DailyFrequencyEvery && !schedulerConfig.DailyFrequencyOnce)
                errors.AppendLine("Daily frequency cannot be empty for daily tasks");

            // para DailyOnce 
            if (schedulerConfig.DailyFrequencyOnce)
            {
                if (schedulerConfig.TimeUnitNumberOf > 0)
                    errors.AppendLine("Daily tasks cannot have timeUnitNumberOf");

                if (schedulerConfig.TimeUnit != null)
                    errors.AppendLine("Time unit cannot be set for a daily frequency one task");

                if (schedulerConfig.DailyStartTime != null)
                    errors.AppendLine("Daily start time cannot be set for a daily frequency one task");

                if (schedulerConfig.DailyEndTime != null)
                    errors.AppendLine("Daily end time cannot be set for a daily frequency one task");

                if (schedulerConfig.ExecutionTimeOfOneDay == null)
                    errors.AppendLine("ExecutionTimeOfOneDay is required");

                if (schedulerConfig.ScheduleType == ScheduleType.DailyEvery)
                    errors.AppendLine("You cannot set daily frequency once in a daily every task type");
            }

            // para DailyEvery ---
            if (schedulerConfig.DailyFrequencyEvery)
            {
                if (schedulerConfig.ExecutionTimeOfOneDay != null)
                    errors.AppendLine("Execution time of day cannot be set for a daily frequency every task");

                if (schedulerConfig.TimeUnit == null)
                    errors.AppendLine("You need to set a time unit for daily every configurations");

                if (schedulerConfig.TimeUnitNumberOf < 1)
                    errors.AppendLine("You need to set a time unit number of greater than 1");

                if (schedulerConfig.DailyStartTime == null)
                    errors.AppendLine("You need to set a daily start time");

                if (schedulerConfig.DailyEndTime == null)
                    errors.AppendLine("You need to set a daily end time");

                if (schedulerConfig.ScheduleType == ScheduleType.DailyOnce)
                    errors.AppendLine("You cannot set daily frequency every in a daily once task type");
            }
        }



    }
}
