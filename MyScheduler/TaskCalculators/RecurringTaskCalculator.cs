using MyScheduler.Common;
using MyScheduler.Entities;
using MyScheduler.Services.Helpers;
using System;
using System.Collections.Generic;

namespace MyScheduler.Services.TaskCalculators
{
    public class RecurringTaskCalculator
    {
        public Result<TaskOutput> GetNextExecution(TaskEntity taskConfig, int? numOccurrences)
        {
            var recurringDays = GetRecurrentDays(taskConfig, numOccurrences);

            DateTimeOffset? nextExecution = null;

            foreach (var date in recurringDays)
            {
                if (date >= taskConfig.CurrentDate)
                {
                    nextExecution = date;
                    taskConfig.EventDate = date;
                    break;
                }
            }

            if (nextExecution == null)
                return Result<TaskOutput>.Failure("No next execution found");

            var output = new TaskOutput
            {
                ExecutionTime = (DateTimeOffset)nextExecution,
                Description = DescriptionGenerator.GetDescription(taskConfig)
            };

            return Result<TaskOutput>.Success(output);
        }

        private List<DateTimeOffset> GetRecurrentDays(TaskEntity taskConfig, int? limitOccurrences)
        {
            var listOfDays = new List<DateTimeOffset>();
            int count = 0;

            var endDate = taskConfig.EndDate ?? DateTimeOffset.MaxValue;

            for (var currentDateIterator = taskConfig.StartDate;
                 currentDateIterator <= endDate && (limitOccurrences == null || count < limitOccurrences);
                 currentDateIterator = currentDateIterator.AddDays(taskConfig.Recurrence))
            {
                if (currentDateIterator >= taskConfig.CurrentDate)
                {
                    listOfDays.Add(currentDateIterator);
                    count++;
                }
            }

            return listOfDays;
        }
    }
}
