using MyScheduler.Entities;
using MyScheduler.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyScheduler.ScheduleCalculators
{
    public class MonthlyTheDailyOnceCalculator
    {
        public Result<ScheduleOutput> GetOutput(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var dates = new AddHoursHelper().addHourToList(scheduleConfig, numOccurrences, new MonthlyTheCalculator().CalculateExecutions(scheduleConfig, numOccurrences));

            return (dates.Count > 0) ? Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(dates.First(), DescriptionGenerator.GetDescription(scheduleConfig)))
                : Result<ScheduleOutput>.Failure("No next execution found");
        }
    }
}
