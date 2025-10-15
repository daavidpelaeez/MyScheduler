using MyScheduler.Common;
using MyScheduler.Entities;
using MyScheduler.Services.Helpers;
using System;

namespace MyScheduler.Services.TaskCalculators
{
    public class OnceTaskCalculator
    {
        public Result<TaskOutput> GetNextExecution(TaskEntity taskConfig)
        {
            var output = new TaskOutput
            {
                ExecutionTime = (DateTimeOffset)taskConfig.EventDate!,
                Description = DescriptionGenerator.GetDescription(taskConfig)
            };
            return Result<TaskOutput>.Success(output);
        }
    }
}
