using MyScheduler.Entities;
using MyScheduler.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MyScheduler.ScheduleCalculators
{
    public class RecurringDailyRangeCalculator
    {
        public Result<ScheduleOutput> GetOutput(ScheduleEntity scheduleConfig, int? maxOccurrences)
        {
            var dates = CalculateExecutions(scheduleConfig, maxOccurrences);

            return (dates.Count > 0) ? Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(dates.First(),DescriptionGenerator.GetDescription(scheduleConfig))) 
                : Result<ScheduleOutput>.Failure("No next execution found"); 
        }

        public List<DateTimeOffset> CalculateExecutions(ScheduleEntity scheduleConfig, int? maxOccurrences)
        {
            var result = new List<DateTimeOffset>();
            var days = DailySchedulerHelper.GetRecurrentDays(scheduleConfig,maxOccurrences);
            var interval = WeeklyScheduleHelper.IntervalCalculator(scheduleConfig);
            var startTime = scheduleConfig.DailyStartTime;
            var endTime = scheduleConfig.DailyEndTime;
            int count = 0;


            foreach (var day in days) 
            {
                for (var currentTime = startTime!.Value;
                     currentTime >= startTime.Value && currentTime <= endTime!.Value;
                     currentTime = currentTime.Add(interval))
                {
                    if (!scheduleConfig.EndDate.HasValue && count >= maxOccurrences)
                        break;

                    var dateWithOffset = DateTimeZoneHelper.ToDateTimeOffset(day.Date, currentTime);

                    result.Add(dateWithOffset);
                    count++;
                }
            }

            return result;
        }


    }
}
