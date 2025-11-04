
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

                return result;

        }

        private Result<ScheduleOutput> CalculateNext(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            if (scheduleConfig.ScheduleType == ScheduleType.Once)
            {
                var calc = new OneTimeCalculator();
                return calc.GetNextExecutionOnce(scheduleConfig);
            }

            if (scheduleConfig.ScheduleType == ScheduleType.Recurring)
            {
                return CalculateNextRecurring(scheduleConfig, numOccurrences);
            }

            return Result<ScheduleOutput>.Failure("Schedule type should be once or recurring");
        }

        private Result<ScheduleOutput> CalculateNextRecurring(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            if (scheduleConfig.Occurs == Occurs.Daily)
            {
                if (scheduleConfig.DailyFrequencyOnceCheckbox)
                    return new RecurringDailyOnceCalculator().GetNextExecutionDailyOnce(scheduleConfig, numOccurrences);

                if (scheduleConfig.DailyFrequencyRangeCheckbox)
                    return new RecurringDailyRangeCalculator().GetNextExecutionDailyEvery(scheduleConfig, numOccurrences);
            }

            if (scheduleConfig.Occurs == Occurs.Weekly)
            {
                if (scheduleConfig.DailyFrequencyOnceCheckbox)
                    return new RecurringWeeklyOnceCalculator().GetNextExecutionWeeklyOnce(scheduleConfig, numOccurrences);

                if (scheduleConfig.DailyFrequencyRangeCheckbox)
                    return new RecurringWeeklyRangeCalculator().GetNextExecutionWeeklyEvery(scheduleConfig, numOccurrences);
            }

            if(scheduleConfig.Occurs == Occurs.Monthly)
            {
                if (scheduleConfig.MonthlyFrequencyDayCheckbox && scheduleConfig.DailyFrequencyOnceCheckbox)
                    return new MonthlyDayDailyOnceCalculator().GetNextExecution(scheduleConfig, numOccurrences);

                if (scheduleConfig.MonthlyFrequencyDayCheckbox && scheduleConfig.DailyFrequencyRangeCheckbox)
                    return new MonthlyDayDailyRangeCalculator().GetNextExecution(scheduleConfig, numOccurrences);

                if (scheduleConfig.MonthlyFrequencyTheCheckbox && scheduleConfig.DailyFrequencyOnceCheckbox)
                    return new MonthlyTheDailyOnceCalculator().GetNextExecution(scheduleConfig, numOccurrences);

                if (scheduleConfig.MonthlyFrequencyTheCheckbox && scheduleConfig.DailyFrequencyRangeCheckbox)
                    return new MonthlyTheDailyRangeCalculator().GetNextExecution(scheduleConfig, numOccurrences);

                if (scheduleConfig.MonthlyFrequencyDayCheckbox)
                    return new MonthlyDayCalculator().GetNextExecution(scheduleConfig, numOccurrences);

                if(scheduleConfig.MonthlyFrequencyTheCheckbox)
                    return new MonthlyTheCalculator().GetNextExecution(scheduleConfig, numOccurrences);

            }

            return Result<ScheduleOutput>.Failure("Recurring tasks should be daily, weekly or monthly");
        }

    }
}


