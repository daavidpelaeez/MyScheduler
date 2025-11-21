using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Enums;
using MyScheduler.Domain.Services.Calculators;
using MyScheduler.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyScheduler.Application.ScheduleCalculators.Monthly
{
    public class MonthlyTheOutput
    {
        public Result<ScheduleOutput> GetOutput(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var dates = new MonthlyTheCalculator().CalculateExecutions(scheduleConfig, numOccurrences);
            return dates.Count > 0 ? Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(dates.First(), DescriptionGenerator.GetDescription(scheduleConfig))) :
                Result<ScheduleOutput>.Failure("No next execution found");
        }

        
    }
}
