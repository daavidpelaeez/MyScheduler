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
            var output = new TaskOutput();

            output.executionTime = task.eventDate!.Value;

            output.description = $"Occurs once. Schedule will be used on {task.eventDate.Value.Date.ToShortDateString()} at {task.eventDate.Value.ToLocalTime().DateTime.ToShortTimeString()}" +
            $" starting on {task.startDate.Date.ToShortDateString()}";

            return Result<TaskOutput>.Success(output);
        }

        public Result<TaskOutput> GetNextExecutionRecurring(TaskEntity task)
        {
            DateTimeOffset nextExecution;

            if (task.startDate > task.currentDate)
            {
                nextExecution = task.startDate;
            }
            else
            {
                nextExecution = task.currentDate.AddDays(CalculateDaysToAdd(task)); 
            }

            var output = new TaskOutput();

            output.executionTime = nextExecution;

            output.description = $"Occurs every {task.recurrence} day/s. Schedule will be used on {output.executionTime.Date.ToShortDateString()} " +
            $"starting on {task.startDate.Date.ToShortDateString()} ";

            return Result<TaskOutput>.Success(output);

        }

        private int CalculateDaysToAdd(TaskEntity task)
        {
            int daysPassed = (task.currentDate - task.startDate).Days;
            int remainder = daysPassed % task.recurrence;
            return remainder == 0 ? 0 : (task.recurrence - remainder);
        }

        public SortedSet<DateTimeOffset> GetRecurrentDays(DateTimeOffset startFrom, int recurrenceDays, DateTimeOffset? endDate)
        {
            SortedSet<DateTimeOffset> listOfDays = new SortedSet<DateTimeOffset>();
            DateTimeOffset current = startFrom;
            int maxOcurrences = 10;
            int count = 0;

            while (count < maxOcurrences)
            {
                if (endDate.HasValue && current > endDate.Value)
                {
                    break;
                }

                listOfDays.Add(current);
                current = current.AddDays(recurrenceDays);
                count++;

            }

            return listOfDays;

        }
    }
}
