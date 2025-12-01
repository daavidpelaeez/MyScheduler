using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Enums;
using System;
using System.Text;

namespace MyScheduler.Domain.Validators
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

            if (scheduleConfig.OnceTypeDateExecution.HasValue && scheduleConfig.OnceTypeDateExecution < scheduleConfig.CurrentDate)
                errors.AppendLine("The event date must be in the future.");

            if (scheduleConfig.OnceTypeDateExecution.HasValue && scheduleConfig.OnceTypeDateExecution < scheduleConfig.StartDate)
                errors.AppendLine("The event date cannot be before the start date.");

            if (scheduleConfig.OnceTypeDateExecution.HasValue && scheduleConfig.EndDate.HasValue && scheduleConfig.OnceTypeDateExecution > scheduleConfig.EndDate)
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

            if (string.IsNullOrEmpty(scheduleConfig.Language) || (scheduleConfig.Language != "en-US" && scheduleConfig.Language != "en-UK" && scheduleConfig.Language != "es-ES"))
                errors.AppendLine("You need to choose an available Language (en-US, en-UK, es)");

            if (string.IsNullOrEmpty(scheduleConfig.TimeZoneID))
                errors.AppendLine("You need to specify a time zone ID");
        }
    }
}
