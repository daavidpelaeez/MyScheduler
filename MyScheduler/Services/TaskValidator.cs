using MyScheduler.Common;
using MyScheduler.Entities;
using MyScheduler.Enums;
using System;
using System.Text;

namespace MyScheduler.Services
{
    public static class TaskValidator
    {
        public static Result<TaskEntity> ValidateTask(TaskEntity task)
        {
            StringBuilder errorMessage = new StringBuilder();
            TimeSpan maxFutureRange = TimeSpan.FromDays(365 * 10); 
            TimeSpan maxPastRange = TimeSpan.FromDays(365);
            const int maxRecurrence = 1000;

            if (task.EndDate.HasValue && task.StartDate > task.EndDate.Value)
            {
                errorMessage.AppendLine("The start date cannot be after the end date.");
            }

            if (task.EndDate.HasValue && task.EndDate < task.CurrentDate)
            {
                errorMessage.AppendLine("The end date of a recurring task must be after the current date.");
            }

            if (task.EventDate.HasValue && task.EventDate < task.CurrentDate)
            {
                errorMessage.AppendLine("The event date must be after the current date.");
            }

            if (task.EventDate.HasValue && task.EventDate < task.StartDate)
            {
                errorMessage.AppendLine("The event date cannot be before the start date.");
            }

            if (task.EventDate.HasValue && task.EndDate.HasValue && task.EventDate > task.EndDate)
            {
                errorMessage.AppendLine("The event date cannot be after the end date.");
            }

            if (!task.EventDate.HasValue && task.TypeTask == TypeTask.Once)
            {
                errorMessage.AppendLine("The event date is required for tasks of type Once.");
            }

            if (task.TypeTask == TypeTask.Recurring && task.Recurrence < 1)
            {
                errorMessage.AppendLine("Recurring tasks must have a recurrence greater than 0.");
            }

            if (task.TypeTask == TypeTask.Once && task.Recurrence > 0)
            {
                errorMessage.AppendLine("Once tasks cannot have a recurrence");
            }

            if (task.StartDate < task.CurrentDate - maxPastRange)
            {
                errorMessage.AppendLine("You can't set a StartDate before 1 year ago.");

            }
            if (task.StartDate > task.CurrentDate + maxFutureRange)
            {
                errorMessage.AppendLine("You can't set a StartDate more than 5 years in the future");
            }

            if (task.EndDate.HasValue && task.EndDate > task.CurrentDate + maxFutureRange)
            {
                errorMessage.AppendLine("The endDate cannot be more than 10 years in the future.");
            }

            if(task.Recurrence > maxRecurrence)
            {
                errorMessage.AppendLine("The task recurrence cannot be more than 1000");
            }

            if (errorMessage.Length == 0)
            {
                return Result<TaskEntity>.Success(task);
            }

            return Result<TaskEntity>.Failure(errorMessage.ToString());
        }

        

    }
}
