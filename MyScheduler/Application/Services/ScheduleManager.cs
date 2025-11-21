using MyScheduler.Application.ScheduleCalculators;
using MyScheduler.Application.ScheduleCalculators.Daily;
using MyScheduler.Application.ScheduleCalculators.Monthly;
using MyScheduler.Application.ScheduleCalculators.Once;
using MyScheduler.Application.ScheduleCalculators.Weekly;
using MyScheduler.Application.ScheduleOutputs.Monthly;
using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Enums;
using MyScheduler.Domain.Validators;
using MyScheduler.Helpers;


namespace MyScheduler.Application.Services
{
    public class ScheduleManager
    {
        public Result<ScheduleOutput> GetOutput(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var validation = Validator.ValidateTask(scheduleConfig, numOccurrences);

            if (validation.IsFailure)
                return Result<ScheduleOutput>.Failure(validation.Error);

                var result = GetTypeOutput(scheduleConfig, numOccurrences);

                return result;
        }

        private Result<ScheduleOutput> GetTypeOutput(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            switch (scheduleConfig.ScheduleType)
            {
                case ScheduleType.Once:

                    var calc = new OneTimeOutput();
                    return calc.GetOnceOutput(scheduleConfig);

                case ScheduleType.Recurring:

                    return GetRecurringOutput(scheduleConfig, numOccurrences);
            }

            return Result<ScheduleOutput>.Failure("Schedule type should be once or recurring");
        }

        private Result<ScheduleOutput> GetRecurringOutput(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            switch (scheduleConfig.Occurs)
            {
                case Occurs.Daily:
                    if (scheduleConfig.DailyFrequencyOnceCheckbox)
                        return new RecurringDailyOnceOutput().GetOutput(scheduleConfig, numOccurrences);

                    if (scheduleConfig.DailyFrequencyRangeCheckbox)
                        return new RecurringDailyRangeOutput().GetOutput(scheduleConfig, numOccurrences);
                break;

                case Occurs.Weekly:

                    if (scheduleConfig.DailyFrequencyOnceCheckbox)
                        return new RecurringWeeklyOnceOutput().GetOutput(scheduleConfig, numOccurrences);

                    if (scheduleConfig.DailyFrequencyRangeCheckbox)
                        return new RecurringWeeklyRangeOutput().GetOutput(scheduleConfig, numOccurrences);
                break;

                case Occurs.Monthly:

                    if (scheduleConfig.MonthlyFrequencyDayCheckbox && scheduleConfig.DailyFrequencyOnceCheckbox)
                        return new MonthlyDayDailyOnceOutput().GetOutput(scheduleConfig, numOccurrences);

                    if (scheduleConfig.MonthlyFrequencyDayCheckbox && scheduleConfig.DailyFrequencyRangeCheckbox)
                        return new MonthlyDayDailyRangeOutput().GetOutput(scheduleConfig, numOccurrences);

                    if (scheduleConfig.MonthlyFrequencyTheCheckbox && scheduleConfig.DailyFrequencyOnceCheckbox)
                        return new MonthlyTheDailyOnceOutput().GetOutput(scheduleConfig, numOccurrences);

                    if (scheduleConfig.MonthlyFrequencyTheCheckbox && scheduleConfig.DailyFrequencyRangeCheckbox)
                        return new MonthlyTheDailyRangeOutput().GetOutput(scheduleConfig, numOccurrences);

                    if (scheduleConfig.MonthlyFrequencyDayCheckbox)
                        return new MonthlyDayOutput().GetOutput(scheduleConfig, numOccurrences);

                    if (scheduleConfig.MonthlyFrequencyTheCheckbox)
                        return new MonthlyTheOutput().GetOutput(scheduleConfig, numOccurrences);

                    break;
            }

            return Result<ScheduleOutput>.Failure("Recurring tasks should be daily, weekly or monthly");
        }

    }
}


