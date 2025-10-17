using MyScheduler.Common;
using MyScheduler.Entities;
using MyScheduler.Helpers;
using MyScheduler.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyScheduler.Services.TaskCalculators
{
    public class WeeklyOnceTaskCalculator
    {
        public Result<TaskOutput> GetNextExecution(TaskEntity taskConfig, int? numOccurrences)
        {
            var dates = CalculateWeeklyOnceConfig(taskConfig, numOccurrences);
            if (dates.Count == 0)
                return Result<TaskOutput>.Failure("No next execution found");

            var output = new TaskOutput
            {
                ExecutionTime = dates.First(),
                Description = DescriptionGenerator.GetDescription(taskConfig)
            };

            return Result<TaskOutput>.Success(output);
        }

        public List<DateTimeOffset> CalculateWeeklyOnceConfig(TaskEntity taskConfig, int? maxOccurrences)
        {
            var result = new List<DateTimeOffset>();
            var selectedHour = taskConfig.ExecutionTimeOfOneDay!.Value;
            var days = WeeklyScheduleHelper.GetMatchingDays(taskConfig, maxOccurrences);

            foreach (var day in days)
            {

                if (maxOccurrences.HasValue && result.Count >= maxOccurrences)
                {
                    break;
                }

                result.Add(day + selectedHour);
                
            }

            return result;
        }
    }
}
