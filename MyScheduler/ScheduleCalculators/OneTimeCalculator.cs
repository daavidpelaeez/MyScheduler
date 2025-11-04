
using MyScheduler.Entities;
using MyScheduler.Helpers;
using System;

namespace MyScheduler.ScheduleCalculators
{
    public class OneTimeCalculator
    {
        public Result<ScheduleOutput> GetNextExecutionOnce(ScheduleEntity scheduleConfig)
        {
            var nextExecution = scheduleConfig.EventDate;

            return Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(nextExecution!.Value,DescriptionGenerator.GetDescription(scheduleConfig)));

        }
    }
}
