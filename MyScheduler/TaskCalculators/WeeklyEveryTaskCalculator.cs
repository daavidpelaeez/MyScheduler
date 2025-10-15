using MyScheduler.Common;
using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Services.Helpers;
using System;
using System.Collections.Generic;

namespace MyScheduler.Services.TaskCalculators
{
    public class WeeklyEveryTaskCalculator
    {
        public Result<TaskOutput> GetNextExecution(TaskEntity taskConfig, int? maxOccurrences)
        {
            var dates = CalculateWeeklyRecurringConfig(taskConfig, maxOccurrences);
            if (dates.Count == 0)
                return Result<TaskOutput>.Failure("No next execution found");

            var output = new TaskOutput
            {
                ExecutionTime = dates[0],
                Description = DescriptionGenerator.GetDescription(taskConfig)
            };

            return Result<TaskOutput>.Success(output);
        }

        private List<DateTimeOffset> CalculateWeeklyRecurringConfig(TaskEntity taskConfig, int? maxOccurrences)
        {
            var result = new List<DateTimeOffset>();
            var days = GetMatchingDays(taskConfig, maxOccurrences);
            var interval = IntervalCalculator(taskConfig);
            var startTime = taskConfig.DailyStartTime;
            var endTime = taskConfig.DailyEndTime;
            int count = 0;

            foreach (var day in days)
            {
                for (var currentTime = startTime; currentTime >= startTime && currentTime <= endTime; currentTime = currentTime.Value.Add(interval))
                {
                    if (maxOccurrences.HasValue && count >= maxOccurrences)
                        break;

                    result.Add(day + currentTime.Value);
                    count++;
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

        private TimeSpan IntervalCalculator(TaskEntity taskConfig)
        {
            int timeUnitNumberOf = taskConfig.TimeUnitNumberOf!.Value;

            return taskConfig.TimeUnit switch
            {
                TimeUnit.Hours => TimeSpan.FromHours(timeUnitNumberOf),
                TimeUnit.Minutes => TimeSpan.FromMinutes(timeUnitNumberOf),
                TimeUnit.Seconds => TimeSpan.FromSeconds(timeUnitNumberOf),
                _ => throw new ArgumentException("Unit Time not supported"),
            };
        }
    }
}
