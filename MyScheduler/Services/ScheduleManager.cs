using MyScheduler.Common;
using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Services.TaskCalculators;
using MyScheduler.Validators;

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

            return scheduleConfig.Type switch
            {
                Type.Once => new OnceCalculator().GetNextExecutionOnce(scheduleConfig,numOccurrences),
                Type.Recurring => new RecurringCalculator().GetNextExecutionRecurring(scheduleConfig, numOccurrences),
                Type.WeeklyOnce => new WeeklyOnceCalculator().GetNextExecutionWeeklyOnce(scheduleConfig, numOccurrences),
                Type.WeeklyEvery => new WeeklyEveryCalculator().GetNextExecutionWeeklyEvery(scheduleConfig, numOccurrences),
                _ => Result<ScheduleOutput>.Failure("scheduleConfig type not supported")
            };
        }
    }
}
