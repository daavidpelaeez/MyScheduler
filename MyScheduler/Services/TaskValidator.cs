using MyScheduler.Common;
using MyScheduler.Entities;
using MyScheduler.Enums;
using System.Text;

namespace MyScheduler.Services
{
    public static class TaskValidator
    {
        public static Result<TaskEntity> ValidateTask(TaskEntity task)
        {
            StringBuilder errorMessage = new StringBuilder();

            if (task.endDate.HasValue)
            {
                if (task.startDate > task.endDate.Value)
                {
                    errorMessage.AppendLine("The start date cannot be after the end date.");
                }

                if (task.endDate < task.currentDate)
                {
                    errorMessage.AppendLine("The end date of a recurring task must be after the current date.");
                }
            }

            if (task.eventDate.HasValue)
            {
                if (task.eventDate < task.currentDate)
                {
                    errorMessage.AppendLine("The event date must be after the current date.");
                }

                if (task.eventDate < task.startDate)
                {
                    errorMessage.AppendLine("The event date cannot be before the start date.");
                }

                if (task.endDate.HasValue && task.eventDate > task.endDate)
                {
                    errorMessage.AppendLine("The event date cannot be after the end date.");
                }
            }

            if (!task.eventDate.HasValue && task.typeTask == TypeTask.Once)
            {
                errorMessage.AppendLine("The event date is required for tasks of type Once.");
            }

            if (task.typeTask == TypeTask.Recurring && task.recurrence < 1)
            {
                errorMessage.AppendLine("Recurring tasks must have a recurrence greater than 0.");
            }

            if (task.typeTask == TypeTask.Once && task.recurrence > 0)
            {
                errorMessage.AppendLine("Once tasks cannot have a recurrence");
            }

            if (errorMessage.Length == 0)
            {
                return Result<TaskEntity>.Success(task);
            }

            return Result<TaskEntity>.Failure(errorMessage.ToString());
        }

    }
}
