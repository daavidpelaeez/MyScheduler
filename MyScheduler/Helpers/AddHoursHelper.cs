using MyScheduler.Entities;
using MyScheduler.ScheduleCalculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Helpers
{
    public class AddHoursHelper
    {
        public List<DateTimeOffset> addHourToList(ScheduleEntity scheduleConfig, int? numOccurrences, List<DateTimeOffset> listToAddExecutionOnce) 
        {
            var dates = listToAddExecutionOnce;
            var executionDailyOnceTime = scheduleConfig.DailyOnceExecutionTime;
            var datesWithHoursList = new List<DateTimeOffset>();

            foreach (var date in dates)
            {
                var dateWithOffset = DateTimeZoneHelper.ToDateTimeOffset(date.Date, executionDailyOnceTime!.Value);
                datesWithHoursList.Add(dateWithOffset);
            }

            return datesWithHoursList;
        }

        public List<DateTimeOffset> AddHourRangeToList(ScheduleEntity scheduleConfig, int? maxOccurrences, List<DateTimeOffset> listToAddHours)
        {
            var result = new List<DateTimeOffset>();
            var days = listToAddHours;
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
