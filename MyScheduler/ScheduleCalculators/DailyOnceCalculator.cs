using MyScheduler.Entities;
using MyScheduler.Helpers;
using System;



namespace MyScheduler.ScheduleCalculators
{
    public class DailyOnceCalculator
    {
        public Result<ScheduleOutput> GetNextExecutionDailyOnce(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var nextExecution = CalculateNextExecution(scheduleConfig,numOccurrences);

            if (nextExecution == null)
                return Result<ScheduleOutput>.Failure("No next execution for that schedule config");

            var output = new ScheduleOutput
            {
                ExecutionTime = (DateTimeOffset)nextExecution!,
                Description = DescriptionGenerator.GetDescription(scheduleConfig)
            };

            return Result<ScheduleOutput>.Success(output);
        }

       private DateTimeOffset? CalculateNextExecution(ScheduleEntity scheduleConfig,int? numOccurrences)
        {
            var recurringDays = DailySchedulerHelper.GetRecurrentDays(scheduleConfig, numOccurrences);

            foreach (var date in recurringDays)
            {
                if (date >= scheduleConfig.CurrentDate)
                {
                    scheduleConfig.EventDate = date;
                    return date;
                }
            }

            return null;
        }
    }
}
