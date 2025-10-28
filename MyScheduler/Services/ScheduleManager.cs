
using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.ScheduleCalculators;
using MyScheduler.Validators;
using MyScheduler.Helpers;

namespace MyScheduler.Services
{
    public class ScheduleManager
    {
        public Result<ScheduleOutput> GetNextExecution(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var validation = Validator.ValidateTask(scheduleConfig,numOccurrences);

            if (validation.IsFailure)
            {
                return Result<ScheduleOutput>.Failure(validation.Error);
            }

            return scheduleConfig.ScheduleType switch
            {
                ScheduleType.OneTime => new OnceCalculator().GetNextExecutionOnce(scheduleConfig),
                ScheduleType.RecurringDailyOnce => new RecurringDailyOnceCalculator().GetNextExecutionDailyOnce(scheduleConfig, numOccurrences),
                ScheduleType.RecurringDailyRange => new RecurringDailyRangeCalculator().GetNextExecutionDailyEvery(scheduleConfig, numOccurrences),
                ScheduleType.RecurringWeeklyOnce => new RecurringWeeklyOnceCalculator().GetNextExecutionWeeklyOnce(scheduleConfig, numOccurrences),
                ScheduleType.RecurringWeeklyRange => new RecurringWeeklyRangeCalculator().GetNextExecutionWeeklyEvery(scheduleConfig, numOccurrences),
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}
