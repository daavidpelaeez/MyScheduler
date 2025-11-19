using MyScheduler.Entities;
using MyScheduler.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MyScheduler.ScheduleCalculators
{
    public class MonthlyDayCalculator
    {
        public Result<ScheduleOutput> GetOutput(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var dates = CalculateExecutions(scheduleConfig, numOccurrences);

            return (dates.Count > 0) ? Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(dates.First(), DescriptionGenerator.GetDescription(scheduleConfig)))
                : Result<ScheduleOutput>.Failure("No next execution found");
        }

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
