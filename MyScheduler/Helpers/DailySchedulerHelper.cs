using MyScheduler.Entities;
using MyScheduler.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Helpers
{
    public static class DailySchedulerHelper
    {
        public static List<DateTimeOffset> GetRecurrentDays(ScheduleEntity scheduleConfig, int? limitOccurrences)
        {
            var listOfDays = new List<DateTimeOffset>();
            int count = 0;

            TimeSpan selectedHour = TimeSpan.Zero;

            if (scheduleConfig.ScheduleType == Enums.ScheduleType.Recurring &&
                scheduleConfig.Occurs == Occurs.Daily &&
                scheduleConfig.DailyOnceExecutionTime.HasValue)
            {
                selectedHour = scheduleConfig.DailyOnceExecutionTime.Value;
            }

            var endDate = scheduleConfig.EndDate ?? DateTimeOffset.MaxValue;

            for (var currentDateIterator = scheduleConfig.StartDate;
                 currentDateIterator <= endDate && (limitOccurrences == null || count < limitOccurrences);
                 currentDateIterator = currentDateIterator.AddDays(scheduleConfig.Recurrence))
            {
                if (currentDateIterator >= scheduleConfig.CurrentDate)
                {
                    listOfDays.Add(currentDateIterator.Add(selectedHour)); 
                    count++;
                }
            }

            return listOfDays;
        }
    }
}
