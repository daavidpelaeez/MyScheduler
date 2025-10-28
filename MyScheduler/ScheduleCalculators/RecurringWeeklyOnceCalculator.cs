
using MyScheduler.Entities;
using MyScheduler.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyScheduler.ScheduleCalculators
{
    public class RecurringWeeklyOnceCalculator
    {
        public Result<ScheduleOutput> GetNextExecutionWeeklyOnce(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var dates = CalculateWeeklyOnceConfig(scheduleConfig, numOccurrences);
            if (dates.Count < 1)
                return Result<ScheduleOutput>.Failure("No next execution found");

            var output = new ScheduleOutput
            {
                ExecutionTime = dates.First(),
                Description = DescriptionGenerator.GetDescription(scheduleConfig)
            };

            return Result<ScheduleOutput>.Success(output);
        }

        public List<DateTimeOffset> CalculateWeeklyOnceConfig(ScheduleEntity scheduleConfig, int? maxOccurrences)
        {
            var result = new List<DateTimeOffset>();
            var selectedHour = scheduleConfig.ExecutionTimeOfOneDay!.Value;
            var days = WeeklyScheduleHelper.GetMatchingDays(scheduleConfig, maxOccurrences);


            foreach (var day in days)
            {

                if (maxOccurrences.HasValue && result.Count >= maxOccurrences)
                {
                    break;
                }

                result.Add(day + selectedHour);
                
            }

            return result;
        }
    }
}
