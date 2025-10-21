using MyScheduler.Common;
using MyScheduler.Entities;
using MyScheduler.Services.Helpers;
using System;

namespace MyScheduler.Services.TaskCalculators
{
    public class OnceCalculator
    {
        public Result<ScheduleOutput> GetNextExecutionOnce(ScheduleEntity scheduleConfig,int? numOccurrences)
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
