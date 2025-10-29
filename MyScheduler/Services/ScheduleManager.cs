
using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Helpers;
using MyScheduler.ScheduleCalculators;
using MyScheduler.Validators;
using System;

namespace MyScheduler.Services
{
    public class ScheduleManager
    {
        public Result<ScheduleOutput> GetNextExecution(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var validation = Validator.ValidateTask(scheduleConfig, numOccurrences);

            if (validation.IsFailure)
                return Result<ScheduleOutput>.Failure(validation.Error);

                var result = CalculateNext(scheduleConfig, numOccurrences);
                if (result == null)
                    return Result<ScheduleOutput>.Failure("Configuration not valid!");

                return result;

        }

        private Result<ScheduleOutput> CalculateNext(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            if (scheduleConfig.ScheduleType == ScheduleType.Once)
            {
                var calc = new OnceTimeCalculator();
                return calc.GetNextExecutionOnce(scheduleConfig);
            }

            if (scheduleConfig.ScheduleType == ScheduleType.Recurring)
            {
                if (scheduleConfig.Occurs == Occurs.Daily)
                {
                    if (scheduleConfig.DailyFrequencyOnceCheckbox)
                        return new RecurringDailyOnceCalculator().GetNextExecutionDailyOnce(scheduleConfig, numOccurrences);

                    if (scheduleConfig.DailyFrequencyEveryCheckbox)
                        return new RecurringDailyRangeCalculator().GetNextExecutionDailyEvery(scheduleConfig, numOccurrences);
                }

                if (scheduleConfig.Occurs == Occurs.Weekly)
                {
                    if (scheduleConfig.DailyFrequencyOnceCheckbox)
                        return new RecurringWeeklyOnceCalculator().GetNextExecutionWeeklyOnce(scheduleConfig, numOccurrences);

                    if (scheduleConfig.DailyFrequencyEveryCheckbox)
                        return new RecurringWeeklyRangeCalculator().GetNextExecutionWeeklyEvery(scheduleConfig, numOccurrences);
                }

            }

            return Result<ScheduleOutput>.Failure("Error calculando la siguiente ejecucion");
        }

    }
}


