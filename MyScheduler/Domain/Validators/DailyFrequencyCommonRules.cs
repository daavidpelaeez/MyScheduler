using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Enums;
using System.Text;

namespace MyScheduler.Domain.Validators
{
    public static class DailyFrequencyCommonRules
    {
        public static void CheckDailyCommonRules(ScheduleEntity scheduleConfig, StringBuilder errors)
        {
            if (scheduleConfig.DailyFrequencyOnceCheckbox && scheduleConfig.DailyFrequencyRangeCheckbox)
                errors.AppendLine("Daily frequency cannot be once and every at the same time");

            if (!scheduleConfig.DailyFrequencyOnceCheckbox && !scheduleConfig.DailyFrequencyRangeCheckbox)
                errors.AppendLine("Daily frequency cannot be empty for daily tasks");

            if (scheduleConfig.DailyFrequencyOnceCheckbox)
            {
                if (scheduleConfig.TimeUnitNumberOf > 0)
                    errors.AppendLine("Daily tasks cannot have timeUnitNumberOf");

                if (scheduleConfig.TimeUnit != null)
                    errors.AppendLine("Time unit cannot be set for a daily frequency once task");

                if (scheduleConfig.DailyStartTime != null)
                    errors.AppendLine("Daily start time cannot be set for a daily frequency once task");

                if (scheduleConfig.DailyEndTime != null)
                    errors.AppendLine("Daily end time cannot be set for a daily frequency once task");

                if (scheduleConfig.DailyOnceExecutionTime == null)
                    errors.AppendLine("DailyOnceExecutionTime is required");

                if (scheduleConfig.ScheduleType == ScheduleType.Recurring && scheduleConfig.DailyFrequencyRangeCheckbox)
                    errors.AppendLine("You cannot set daily frequency once in a daily every task type");
            }

            if (scheduleConfig.DailyFrequencyRangeCheckbox)
            {
                if (scheduleConfig.DailyOnceExecutionTime != null)
                    errors.AppendLine("Execution time of day cannot be set for a daily frequency every task");

                if (scheduleConfig.TimeUnit == null)
                    errors.AppendLine("You need to set a time unit for daily every configurations");

                if (scheduleConfig.TimeUnitNumberOf < 1)
                    errors.AppendLine("You need to set a time unit number of greater than 0");

                if (scheduleConfig.DailyStartTime == null)
                    errors.AppendLine("You need to set a daily start time");

                if (scheduleConfig.DailyEndTime == null)
                    errors.AppendLine("You need to set a daily end time");

                if (scheduleConfig.ScheduleType == ScheduleType.Recurring && scheduleConfig.DailyFrequencyOnceCheckbox)
                    errors.AppendLine("You cannot set daily frequency every in a daily once task type");
            }
        }
    }
}
