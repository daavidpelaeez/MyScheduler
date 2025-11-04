using MyScheduler.Entities;
using MyScheduler.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyScheduler.ScheduleCalculators
{
    public class MonthlyTheDailyOnceCalculator
    {
        public Result<ScheduleOutput> GetNextExecution(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var dates = CalculateNextExecution(scheduleConfig, numOccurrences);

            return (dates.Count > 0) ? Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(dates.First(), DescriptionGenerator.GetDescription(scheduleConfig)))
                : Result<ScheduleOutput>.Failure("No next execution found");
        }

        public List<DateTimeOffset> CalculateNextExecution(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var dates = new MonthlyTheCalculator().CalculateNextExecution(scheduleConfig, numOccurrences);
            var executionDailyOnceTime = scheduleConfig.DailyOnceExecutionTime;
            var datesWithHoursList = new List<DateTimeOffset>();

            foreach (var date in dates)
            {
                if (datesWithHoursList.Count > numOccurrences)
                    break;

                var dateWithOffset = DateTimeZoneHelper.ToDateTimeOffset(date.Date, executionDailyOnceTime!.Value);
                datesWithHoursList.Add(dateWithOffset);
            }

            return datesWithHoursList;
        }
    }
}
