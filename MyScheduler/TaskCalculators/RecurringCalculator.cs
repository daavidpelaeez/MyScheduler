using MyScheduler.Common;
using MyScheduler.Entities;
using MyScheduler.Services.Helpers;
using System;
using System.Collections.Generic;

namespace MyScheduler.Services.TaskCalculators
{
    public class RecurringCalculator
    {
        public Result<ScheduleOutput> GetNextExecutionRecurring(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var recurringDays = GetRecurrentDays(scheduleConfig, numOccurrences);

            DateTimeOffset? nextExecution = null;

            foreach (var date in recurringDays)
            {
                if (date >= scheduleConfig.CurrentDate)
                {
                    nextExecution = date;
                    scheduleConfig.EventDate = date;
                    break;
                }
            }

            if (nextExecution == null)
                return Result<ScheduleOutput>.Failure("No next execution found");

            var output = new ScheduleOutput
            {
                ExecutionTime = (DateTimeOffset)nextExecution,
                Description = DescriptionGenerator.GetDescription(scheduleConfig)
            };

            return Result<ScheduleOutput>.Success(output);
        }

        private List<DateTimeOffset> GetRecurrentDays(ScheduleEntity scheduleConfig, int? limitOccurrences)
        {
            var listOfDays = new List<DateTimeOffset>();
            int count = 0;

            var endDate = scheduleConfig.EndDate ?? DateTimeOffset.MaxValue;

            for (var currentDateIterator = scheduleConfig.StartDate;
                 currentDateIterator <= endDate && (limitOccurrences == null || count < limitOccurrences);
                 currentDateIterator = currentDateIterator.AddDays(scheduleConfig.Recurrence))
            {
                if (currentDateIterator >= scheduleConfig.CurrentDate)
                {
                    listOfDays.Add(currentDateIterator);
                    count++;
                }
            }

            return listOfDays;
        }
    }
}
