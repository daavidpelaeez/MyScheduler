using MyScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class OnceValidator
    {
        public static void Validate(ScheduleEntity scheduleConfig, StringBuilder errors)
        {
            if (!scheduleConfig.EventDate.HasValue)
                errors.AppendLine("EventDate is required for OneTime tasks.");

            if (scheduleConfig.Recurrence > 0)
                errors.AppendLine("OneTime tasks cannot have a recurrence.");

            if (scheduleConfig.DailyFrequencyEveryCheckbox || scheduleConfig.DailyFrequencyOnceCheckbox)
                errors.AppendLine("OneTime tasks cannot have daily frequency");

            if (scheduleConfig.WeeklyRecurrence > 0 || (scheduleConfig.DaysOfWeek != null && scheduleConfig.DaysOfWeek.Count > 0))
                errors.AppendLine("OneTime tasks cannot have weekly configuration");

            if (scheduleConfig.DailyOnceExecutionTime != null)
                errors.AppendLine("OneTime tasks cannot have execution once on daily frequency");

            if (scheduleConfig.TimeUnit != null || scheduleConfig.TimeUnitNumberOf > 0 || scheduleConfig.DailyStartTime != null || scheduleConfig.DailyEndTime != null)
                errors.AppendLine("OneTime tasks cannot have daily frequency configuration");
        }
    }

}
