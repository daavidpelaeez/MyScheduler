using MyScheduler.Common;
using MyScheduler.Entities;
using MyScheduler.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

            return task.TypeTask switch
            {
                TypeTask.Once => GetNextExecutionOnce(task),
                TypeTask.Recurring => GetNextExecutionRecurring(task),
                _ => Result<TaskOutput>.Failure("Task type not supported")
            };
        }

        public Result<TaskOutput> GetNextExecutionOnce(TaskEntity task)
        {
            var output = GetOutput(task.EventDate!.Value, task);

            return Result<TaskOutput>.Success(output);
        }

        public Result<TaskOutput> GetNextExecutionRecurring(TaskEntity task)
        {
            DateTimeOffset? nextExecution = null;

            var recurringDays = GetRecurrentDays(task, 10);

            for (int i = 0; i < recurringDays.Count; i++)
            {
                if (recurringDays[i] >= task.CurrentDate)
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

            switch (task.TypeTask)
            {
                case TypeTask.Once:
                    output.description = $"Occurs once. Schedule will be used on {task.EventDate!.Value.Date.ToShortDateString()} at {task.EventDate.Value.ToLocalTime().DateTime.ToShortTimeString()}" +
                    $" starting on {task.StartDate.Date.ToShortDateString()}";
                    return output;


                case TypeTask.Recurring:
                    output.description = $"Occurs every {task.Recurrence} day/s. Schedule will be used on {output.executionTime.Date.ToShortDateString()} " +
                    $"starting on {task.StartDate.Date.ToShortDateString()} ";
                    return output;

                default:
                    throw new Exception("Task type not supported at GetOutput");
            }
        }


        public List<DateTimeOffset> GetRecurrentDays(TaskEntity task, int limitOccurrences)
        {
            var listOfDays = new List<DateTimeOffset>();
            int count = 0;
            bool useEndDate = task.EndDate.HasValue;

            for (var currentDateIterator = task.StartDate;
                 (useEndDate || count < limitOccurrences);
                 currentDateIterator = currentDateIterator.AddDays(task.Recurrence))
            {
                if (useEndDate && currentDateIterator > task.EndDate!.Value)
                {
                    break;
                }

                if (currentDateIterator >= task.CurrentDate)
                {
                    listOfDays.Add(currentDateIterator);
                    count++;
                }
            }

            return listOfDays;
        }


    }
}
