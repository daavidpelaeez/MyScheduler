using MyScheduler.Entities;
using MyScheduler.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyScheduler.ScheduleCalculators
{
    public class MonthlyDayDailyRangeCalculator
    {
        public Result<ScheduleOutput> GetNextExecution(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var dates = new AddHoursHelper().AddHourRangeToList(scheduleConfig,numOccurrences,new MonthlyDayCalculator().CalculateNextExecution(scheduleConfig,numOccurrences));

            return (dates.Count > 0) ? Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(dates.First(), DescriptionGenerator.GetDescription(scheduleConfig)))
                : Result<ScheduleOutput>.Failure("No next execution found");
        }
    }
}
