using MyScheduler.Common;
using MyScheduler.Entities;
using MyScheduler.Enums;
using System;
using System.Collections.Generic;

namespace MyScheduler.Services
{
    public class TaskManager
    {
        public Result<TaskOutput> GetNextExecution(TaskEntity task)
        {
            var validation = TaskValidator.ValidateTask(task);

            if (validation.IsFailure)
            {
                return Result<TaskOutput>.Failure(validation.Error);
            }

            return task.typeTask switch
            {
                TypeTask.Once => GetNextExecutionOnce(task),
                TypeTask.Recurring => GetNextExecutionRecurring(task),
                _ => Result<TaskOutput>.Failure("Task type not supported")
            };
        }

        public Result<TaskOutput> GetNextExecutionOnce(TaskEntity task)
        {
            var output = GetOutput(task.eventDate!.Value,task);

            return Result<TaskOutput>.Success(output);
        }

        public Result<TaskOutput> GetNextExecutionRecurring(TaskEntity task)
        {
            DateTimeOffset? nextExecution = null;

            var recurringDays = GetRecurrentDays(task.startDate, task.recurrence, task.endDate, 10);

            for (int i = 0; i < recurringDays.Count; i++)
            {
                if (recurringDays[i] >= task.currentDate)
                {
                    nextExecution = recurringDays[i];
                    break;
                }
            }

            var output = GetOutput(nextExecution!.Value, task);

            return Result<TaskOutput>.Success(output);
        }

        public TaskOutput GetOutput(DateTimeOffset executionTime, TaskEntity task)
        {

            var output = new TaskOutput();
            output.executionTime = executionTime;

            switch (task.typeTask)
            {
                case TypeTask.Once:
                    output.description = $"Occurs once. Schedule will be used on {task.eventDate!.Value.Date.ToShortDateString()} at {task.eventDate.Value.ToLocalTime().DateTime.ToShortTimeString()}" +
                    $" starting on {task.startDate.Date.ToShortDateString()}";
                    return output;


                case TypeTask.Recurring:
                    output.description = $"Occurs every {task.recurrence} day/s. Schedule will be used on {output.executionTime.Date.ToShortDateString()} " +
                    $"starting on {task.startDate.Date.ToShortDateString()} ";
                    return output;

                default:
                    throw new Exception("Task type not supported at GetOutput");
            }
        }

        private DateTimeOffset AddRecurrence(DateTimeOffset date, int recurrence)
        {
            return date.AddDays(recurrence);
        }

        public List<DateTimeOffset> GetRecurrentDays(DateTimeOffset startFrom, int recurrence, DateTimeOffset? endDate, int limit)
        {
            var listOfDays = new List<DateTimeOffset>();

            DateTimeOffset current = startFrom;

            int count = 0;

            while (count < limit)
            {
                if (endDate.HasValue && current > endDate.Value)
                {
                    break;
                }

                listOfDays.Add(current);
                current = AddRecurrence(current, recurrence);
                count++;

            }

            return listOfDays;

        }


    }
}
