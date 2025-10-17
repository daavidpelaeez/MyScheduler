using MyScheduler.Common;
using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Helpers;
using MyScheduler.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyScheduler.Services.TaskCalculators
{
    public class WeeklyEveryTaskCalculator
    {
        public Result<TaskOutput> GetNextExecution(TaskEntity taskConfig, int? maxOccurrences)
        {
            var dates = CalculateWeeklyRecurringConfig(taskConfig, maxOccurrences);

            if (dates.Count == 0)
                return Result<TaskOutput>.Failure("No next execution avaliable in that range");

            var output = new TaskOutput
            {
                ExecutionTime = dates.First(),
                Description = DescriptionGenerator.GetDescription(taskConfig)
            };

            return Result<TaskOutput>.Success(output);
        }

        public List<DateTimeOffset> CalculateWeeklyRecurringConfig(TaskEntity taskConfig, int? maxOccurrences)
        {
            var result = new List<DateTimeOffset>();
            var days = WeeklyScheduleHelper.GetMatchingDays(taskConfig, maxOccurrences);
            var interval = IntervalCalculator(taskConfig);
            var startTime = taskConfig.DailyStartTime;
            var endTime = taskConfig.DailyEndTime;
            int count = 0;

            foreach (var day in days)
            {
                for (var currentTime = startTime; 
                    currentTime >= startTime && currentTime <= endTime; 
                    currentTime = currentTime.Value.Add(interval))
                {
                    if (!taskConfig.EndDate.HasValue && count >= maxOccurrences)
                        break;

                    var execution = day + currentTime.Value;

                    result.Add(execution);
                    count++;
                }
            }

            return result;
        }

        private TimeSpan IntervalCalculator(TaskEntity taskConfig)
        {
            int timeUnitNumberOf = taskConfig.TimeUnitNumberOf!.Value;

            return taskConfig.TimeUnit switch
            {
                TimeUnit.Hours   => TimeSpan.FromHours(timeUnitNumberOf),
                TimeUnit.Minutes => TimeSpan.FromMinutes(timeUnitNumberOf),
                TimeUnit.Seconds => TimeSpan.FromSeconds(timeUnitNumberOf),
                _ => throw new ArgumentException("Unit Time not supported"),
            };
        }
    }
}
