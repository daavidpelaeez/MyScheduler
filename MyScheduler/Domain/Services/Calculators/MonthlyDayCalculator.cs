using MyScheduler.Domain.Entities;
using System;
using System.Collections.Generic;

namespace MyScheduler.Domain.Services.Calculators
{
    public class MonthlyDayCalculator
    {
        public List<DateTimeOffset> CalculateExecutions(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var resultList = new List<DateTimeOffset>();
            int dayNumber = scheduleConfig.MonthlyDayNumber!.Value;
            int monthRecurrence = scheduleConfig.MonthlyDayRecurrence!.Value;
            DateTimeOffset endDate = scheduleConfig.EndDate ?? DateTimeOffset.MaxValue;
            var currentMonth = scheduleConfig.StartDate;
            for (int count = 0; currentMonth <= endDate; currentMonth = currentMonth.AddMonths(monthRecurrence))
            {
                if (numOccurrences.HasValue && count >= numOccurrences)
                    break;
                int daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
                if (dayNumber <= daysInMonth)
                {
                    var targetDate = new DateTimeOffset(currentMonth.Year, currentMonth.Month, dayNumber, currentMonth.Hour, currentMonth.Minute, currentMonth.Second, currentMonth.Offset);
                    if (targetDate >= scheduleConfig.StartDate)
                    {
                        resultList.Add(targetDate);
                        count++;
                    }
                }
            }
            return resultList;
        }
    }
}
