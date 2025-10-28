
using MyScheduler.Entities;
using MyScheduler.Helpers;
using System;

namespace MyScheduler.ScheduleCalculators
{
    public class OnceCalculator
    {
        public Result<ScheduleOutput> GetNextExecutionOnce(ScheduleEntity scheduleConfig)
        {
            var output = new ScheduleOutput
            {
                ExecutionTime = (DateTimeOffset)scheduleConfig.EventDate!,
                Description = DescriptionGenerator.GetDescription(scheduleConfig)
            };
            return Result<ScheduleOutput>.Success(output);
        }
    }
}
