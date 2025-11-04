
using MyScheduler.Entities;
using MyScheduler.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace MyScheduler.ScheduleCalculators
{
    public class RecurringWeeklyRangeCalculator
    {
        public Result<ScheduleOutput> GetNextExecutionWeeklyEvery(ScheduleEntity scheduleConfig, int? maxOccurrences)
        {
            var dates = CalculateWeeklyRecurringConfig(scheduleConfig, maxOccurrences);

            return (dates.Count > 0) ? Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(dates.First(), DescriptionGenerator.GetDescription(scheduleConfig))) :
                Result<ScheduleOutput>.Failure("No next execution found");
          
        }

        public List<DateTimeOffset> CalculateWeeklyRecurringConfig(ScheduleEntity scheduleConfig, int? maxOccurrences)
        {
            var result = new List<DateTimeOffset>();
            var days = WeeklyScheduleHelper.GetMatchingDays(scheduleConfig, maxOccurrences);
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

                    var dateWithOffset = DateTimeZoneHelper.ToDateTimeOffset(day.Date, currentTime.Value);

                    result.Add(dateWithOffset);
                    count++;
                }
            }

            return result;
        }

    }
}
