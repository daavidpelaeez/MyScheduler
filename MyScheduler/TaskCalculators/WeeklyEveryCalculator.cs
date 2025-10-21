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
    public class WeeklyEveryCalculator
    {
        public Result<ScheduleOutput> GetNextExecutionWeeklyEvery(ScheduleEntity scheduleConfig, int? maxOccurrences)
        {
            var dates = CalculateWeeklyRecurringConfig(scheduleConfig, maxOccurrences);

            if (dates.Count == 0)
                return Result<ScheduleOutput>.Failure("No next execution avaliable in that range");

            var output = new ScheduleOutput
            {
                ExecutionTime = dates.First(),
                Description = DescriptionGenerator.GetDescription(scheduleConfig)
            };

            return Result<ScheduleOutput>.Success(output);
        }

        public List<DateTimeOffset> CalculateWeeklyRecurringConfig(ScheduleEntity scheduleConfig, int? maxOccurrences)
        {
            var result = new List<DateTimeOffset>();
            var days = WeeklyScheduleHelper.GetMatchingDays(scheduleConfig, maxOccurrences);
            var interval = IntervalCalculator(scheduleConfig);
            var startTime = scheduleConfig.DailyStartTime;
            var endTime = scheduleConfig.DailyEndTime;
            int count = 0;

            foreach (var day in days)
            {
                for (var currentTime = startTime; 
                    currentTime >= startTime && currentTime <= endTime; 
                    currentTime = currentTime.Value.Add(interval))
                {
                    if (!scheduleConfig.EndDate.HasValue && count >= maxOccurrences)
                        break;

                    result.Add(day + currentTime.Value);
                    count++;
                }
            }

            return result;
        }

        private TimeSpan IntervalCalculator(ScheduleEntity scheduleConfig)
        {
            int timeUnitNumberOf = scheduleConfig.TimeUnitNumberOf!.Value;

            return scheduleConfig.TimeUnit switch
            {
                TimeUnit.Hours   => TimeSpan.FromHours(timeUnitNumberOf),
                TimeUnit.Minutes => TimeSpan.FromMinutes(timeUnitNumberOf),
                TimeUnit.Seconds => TimeSpan.FromSeconds(timeUnitNumberOf),
                _ => throw new ArgumentException("Unit Time not supported"),
            };
        }
    }
}
