using MyScheduler.Domain.Entities;
using System.Text;

namespace MyScheduler.Domain.Validators
{
    public static class OnceValidator
    {
        public static void Validate(ScheduleEntity scheduleConfig, StringBuilder errors)
        {
            if (!scheduleConfig.OnceTypeDateExecution.HasValue)
                errors.AppendLine("OnceTypeDateExecution is required for OneTime tasks.");

            if (scheduleConfig.Recurrence > 0)
                errors.AppendLine("OneTime tasks cannot have a recurrence.");

            if (scheduleConfig.DailyFrequencyRangeCheckbox || scheduleConfig.DailyFrequencyOnceCheckbox)
                errors.AppendLine("OneTime tasks cannot have daily frequency");

            if (scheduleConfig.WeeklyRecurrence > 0 || scheduleConfig.DaysOfWeek != null && scheduleConfig.DaysOfWeek.Count > 0)
                errors.AppendLine("OneTime tasks cannot have weekly configuration");

            if (scheduleConfig.DailyOnceExecutionTime != null)
                errors.AppendLine("OneTime tasks cannot have execution once on daily frequency");

            if (scheduleConfig.TimeUnit != null || scheduleConfig.TimeUnitNumberOf > 0 || scheduleConfig.DailyStartTime != null || scheduleConfig.DailyEndTime != null)
                errors.AppendLine("OneTime tasks cannot have daily frequency configuration");
        }
    }

}
