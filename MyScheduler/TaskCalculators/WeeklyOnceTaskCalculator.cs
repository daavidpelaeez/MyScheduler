using MyScheduler.Common;
using MyScheduler.Entities;
using MyScheduler.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyScheduler.Services.TaskCalculators
{
    public class WeeklyOnceTaskCalculator
    {
        public Result<TaskOutput> GetNextExecution(TaskEntity taskConfig, int? numOccurrences)
        {
            var dates = CalculateWeeklyOnceConfig(taskConfig, numOccurrences);
            if (dates.Count == 0)
                return Result<TaskOutput>.Failure("No next execution found");

            var output = new TaskOutput
            {
                ExecutionTime = dates.First(),
                Description = DescriptionGenerator.GetDescription(taskConfig)
            };

            return Result<TaskOutput>.Success(output);
        }

        private List<DateTimeOffset> CalculateWeeklyOnceConfig(TaskEntity taskConfig, int? maxOccurrences)
        {
            var result = new List<DateTimeOffset>();
            var selectedHour = taskConfig.ExecutionTimeOfOneDay!.Value;
            var days = GetMatchingDays(taskConfig, maxOccurrences);
            int count = 0;

            foreach (var day in days)
            {
                result.Add(day + selectedHour);
                count++;

                if (maxOccurrences.HasValue && count >= maxOccurrences)
                {
                    break;
                }
            }

            return result;
        }

        private List<DateTimeOffset> GetMatchingDays(TaskEntity config, int? maxOccurrences)
        {
            var result = new List<DateTimeOffset>();
            var end = config.EndDate ?? DateTimeOffset.MaxValue;

            for (var current = config.StartDate; current <= end; current = current.AddDays(1))
            {
                if (current >= config.CurrentDate && config.DaysOfWeek.Contains(current.DayOfWeek))
                {
                    result.Add(current);
                }

                if (maxOccurrences.HasValue && result.Count >= maxOccurrences.Value)
                    break;

                if (current.DayOfWeek == DayOfWeek.Sunday)
                {
                    current = current.AddDays((config.WeeklyRecurrence - 1) * 7);
                }
            }
            return result;
        }
    }
}
