using MyScheduler.Entities;
using MyScheduler.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MyScheduler.ScheduleCalculators
{
    public class RecurringDailyRangeCalculator
    {
        public Result<ScheduleOutput> GetNextExecutionDailyEvery(ScheduleEntity scheduleConfig, int? maxOccurrences)
        {
            var dates = CalculateNextExecutions(scheduleConfig, maxOccurrences);

            if (dates.Count == 0)
                return Result<ScheduleOutput>.Failure("No next execution avaliable in that range");

            var output = new ScheduleOutput
            {
                ExecutionTime = dates.First(),
                Description = DescriptionGenerator.GetDescription(scheduleConfig)
            };

            return Result<ScheduleOutput>.Success(output);
        }

        public List<DateTimeOffset> CalculateNextExecutions(ScheduleEntity scheduleConfig, int? maxOccurrences)
        {
            var result = new List<DateTimeOffset>();
            var days = DailySchedulerHelper.GetRecurrentDays(scheduleConfig,maxOccurrences);
            var interval = WeeklyScheduleHelper.IntervalCalculator(scheduleConfig);
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


    }
}
