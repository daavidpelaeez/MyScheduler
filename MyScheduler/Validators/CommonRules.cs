using MyScheduler.Entities;
using MyScheduler.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class CommonRules
    {
        public static void Validate(ScheduleEntity scheduleConfig, StringBuilder errors, int? numOccurrences)
        {
            const int maxRecurrence = 1000;

            if (!scheduleConfig.EndDate.HasValue && (numOccurrences < 1 || numOccurrences == null))
                errors.AppendLine("You must specified end date or num occurrences");

            if (scheduleConfig.EndDate.HasValue && scheduleConfig.StartDate > scheduleConfig.EndDate.Value)
                errors.AppendLine("The start date cannot be after the end date.");

            if (scheduleConfig.EndDate.HasValue && scheduleConfig.EndDate < scheduleConfig.CurrentDate)
                errors.AppendLine("The end date of a recurring scheduleConfig must be after the current date.");

            if (scheduleConfig.EventDate.HasValue && scheduleConfig.EventDate < scheduleConfig.CurrentDate)
                errors.AppendLine("The event date must be in the future.");

            if (scheduleConfig.EventDate.HasValue && scheduleConfig.EventDate < scheduleConfig.StartDate)
                errors.AppendLine("The event date cannot be before the start date.");

            if (scheduleConfig.EventDate.HasValue && scheduleConfig.EndDate.HasValue && scheduleConfig.EventDate > scheduleConfig.EndDate)
                errors.AppendLine("The event date cannot be after the end date.");

            if (scheduleConfig.StartDate <= DateTimeOffset.MinValue)
                errors.AppendLine("StartDate cannot be less or equal than Min value");

            if (scheduleConfig.StartDate >= DateTimeOffset.MaxValue)
                errors.AppendLine("StartDate cannot be more or equal than Max value");

            if (scheduleConfig.EndDate.HasValue && scheduleConfig.EndDate >= DateTimeOffset.MaxValue)
                errors.AppendLine("EndDate cannot be more or equal than Max value.");

            if (scheduleConfig.Recurrence > maxRecurrence)
                errors.AppendLine("Recurrence cannot be more than 1000.");

            if (!scheduleConfig.Enabled)
                errors.AppendLine("The form its not enabled, check enable checkbox");

            if (scheduleConfig.ScheduleType == ScheduleType.Recurring)
            {
                if (scheduleConfig.Occurs == Occurs.Daily)
                {
                    if (scheduleConfig.DailyFrequencyEveryCheckbox && scheduleConfig.DailyFrequencyOnceCheckbox)
                        errors.AppendLine("You cannot set daily frequency every and once at the same time");

                    if (!scheduleConfig.DailyFrequencyOnceCheckbox && !scheduleConfig.DailyFrequencyEveryCheckbox)
                        errors.AppendLine("Daily recurring task must specify either 'once' or 'every' frequency");
                }
            }
        }
    }
}
