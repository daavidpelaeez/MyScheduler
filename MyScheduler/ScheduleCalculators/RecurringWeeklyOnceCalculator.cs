
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

            return (dates.Count > 0) ? Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(dates.First(),DescriptionGenerator.GetDescription(scheduleConfig))) :
                Result<ScheduleOutput>.Failure("No next execution found");
        }

        public List<DateTimeOffset> CalculateWeeklyOnceConfig(ScheduleEntity scheduleConfig, int? maxOccurrences)
        {
            var result = new List<DateTimeOffset>();
            var selectedHour = scheduleConfig.DailyOnceExecutionTime!.Value;
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
