
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
                ScheduleType.Once => new OnceCalculator().GetNextExecutionOnce(scheduleConfig),
                ScheduleType.DailyOnce => new DailyOnceCalculator().GetNextExecutionDailyOnce(scheduleConfig, numOccurrences),
                ScheduleType.DailyEvery => new DailyEveryCalculator().GetNextExecutionDailyEvery(scheduleConfig, numOccurrences),
                ScheduleType.WeeklyOnce => new WeeklyOnceCalculator().GetNextExecutionWeeklyOnce(scheduleConfig, numOccurrences),
                ScheduleType.WeeklyEvery => new WeeklyEveryCalculator().GetNextExecutionWeeklyEvery(scheduleConfig, numOccurrences),
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}
