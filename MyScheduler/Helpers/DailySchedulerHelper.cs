using MyScheduler.Entities;
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

            var selectedHour = (scheduleConfig.ScheduleType == Enums.ScheduleType.RecurringDailyOnce) ? scheduleConfig.ExecutionTimeOfOneDay!.Value : TimeSpan.Zero;

            var endDate = scheduleConfig.EndDate ?? DateTimeOffset.MaxValue;

            for (var currentDateIterator = scheduleConfig.StartDate;
                 currentDateIterator <= endDate && (limitOccurrences == null || count < limitOccurrences);
                 currentDateIterator = currentDateIterator.AddDays(scheduleConfig.Recurrence))
            {
                if (currentDateIterator >= scheduleConfig.CurrentDate)
                {
                        listOfDays.Add(currentDateIterator + selectedHour);
                        count++;
                }
            }

            return listOfDays;
        }
    }
}
