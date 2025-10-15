using MyScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class TaskCommonRules
    {
        public static void Validate(TaskEntity task, StringBuilder errors)
        {
            TimeSpan maxFutureRange = TimeSpan.FromDays(365 * 10);
            TimeSpan maxPastRange = TimeSpan.FromDays(365);
            const int maxRecurrence = 1000;

            if (task.EndDate.HasValue && task.StartDate > task.EndDate.Value)
                errors.AppendLine("The start date cannot be after the end date.");

            if (task.EndDate.HasValue && task.EndDate < task.CurrentDate)
                errors.AppendLine("The end date of a recurring task must be after the current date.");

            if (task.EventDate.HasValue && task.EventDate < task.CurrentDate)
                errors.AppendLine("The event date must be in the future.");

            if (task.EventDate.HasValue && task.EventDate < task.StartDate)
                errors.AppendLine("The event date cannot be before the start date.");

            if (task.EventDate.HasValue && task.EndDate.HasValue && task.EventDate > task.EndDate)
                errors.AppendLine("The event date cannot be after the end date.");

            if (task.StartDate < task.CurrentDate - maxPastRange)
                errors.AppendLine("StartDate cannot be more than 1 year in the past.");

            if (task.StartDate > task.CurrentDate + maxFutureRange)
                errors.AppendLine("StartDate cannot be more than 10 years in the future.");

            if (task.EndDate.HasValue && task.EndDate > task.CurrentDate + maxFutureRange)
                errors.AppendLine("EndDate cannot be more than 10 years in the future.");

            if (task.Recurrence > maxRecurrence)
                errors.AppendLine("Recurrence cannot be more than 1000.");
        }
    }

}


