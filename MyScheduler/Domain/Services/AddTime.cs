using MyScheduler.Domain.Entities;
using MyScheduler.Helpers;
using System;
using System.Collections.Generic;


namespace MyScheduler.Domain.Services
{
    public class AddTime
    {
        public List<DateTimeOffset> AddHourToList(ScheduleEntity scheduleConfig, List<DateTimeOffset> listToAddExecutionOnce) 
        {
            var dates = listToAddExecutionOnce;
            var executionDailyOnceTime = scheduleConfig.DailyOnceExecutionTime!;
            var timeZoneID = scheduleConfig.TimeZoneID;
            var datesWithHoursList = new List<DateTimeOffset>();

            foreach (var date in dates)
            {
                var dateWithOffset = TimeZoneService.ToTimeZoneOffset(date.Date, executionDailyOnceTime.Value,timeZoneID);
                datesWithHoursList.Add(dateWithOffset);
            }

            return datesWithHoursList;
        }

        public List<DateTimeOffset> AddHourRangeToList(ScheduleEntity scheduleConfig, int? maxOccurrences, List<DateTimeOffset> listToAddHours)
        {
            var result = new List<DateTimeOffset>();
            var days = listToAddHours;
            var interval = WeeklyScheduleHelper.IntervalCalculator(scheduleConfig);
            var timeZoneID = scheduleConfig.TimeZoneID;
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

                    var dateWithOffset = TimeZoneService.ToTimeZoneOffset(day.Date, currentTime,scheduleConfig.TimeZoneID);

                    result.Add(dateWithOffset);
                    count++;
                }
            }

            return result;
        }


    }
}
