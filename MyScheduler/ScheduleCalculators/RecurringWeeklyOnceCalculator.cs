
using MyScheduler.Entities;
using MyScheduler.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyScheduler.ScheduleCalculators
{
    public class RecurringWeeklyOnceCalculator
    {
        public Result<ScheduleOutput> GetOutput(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var dates = CalculateExecutions(scheduleConfig, numOccurrences);

            return (dates.Count > 0) ? Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(dates.First(),DescriptionGenerator.GetDescription(scheduleConfig))) :
                Result<ScheduleOutput>.Failure("No next execution found");
        }

        public List<DateTimeOffset> CalculateExecutions(ScheduleEntity scheduleConfig, int? maxOccurrences)
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
