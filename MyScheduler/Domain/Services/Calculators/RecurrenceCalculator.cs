using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Domain.Services.Calculators
{
    public class RecurrenceCalculator
    {
        public static List<DateTimeOffset> GetRecurrentDays(ScheduleEntity scheduleConfig, int? limitOccurrences)
        {
            var listOfDays = new List<DateTimeOffset>();
            var count = 0;
            var selectedHour = TimeSpan.Zero;
            var endDate = scheduleConfig.EndDate ?? DateTimeOffset.MaxValue;
            if (scheduleConfig.ScheduleType == ScheduleType.Recurring &&
                (scheduleConfig.Occurs == Occurs.Daily || scheduleConfig.Occurs == Occurs.Monthly) && scheduleConfig.DailyFrequencyOnceCheckbox)
            {
                selectedHour = scheduleConfig.DailyOnceExecutionTime!.Value;
            }
            for (var currentDateIterator = scheduleConfig.StartDate;
                 currentDateIterator <= endDate && (limitOccurrences == null || count < limitOccurrences);
                 currentDateIterator = currentDateIterator.AddDays(scheduleConfig.Recurrence!.Value))
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
