
using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Helpers;
using MyScheduler.ScheduleCalculators;
using MyScheduler.Validators;

namespace MyScheduler.Services
{
    public class ScheduleManager
    {
        public Result<ScheduleOutput> GetNextExecution(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var validation = Validator.ValidateTask(scheduleConfig, numOccurrences);

            if (validation.IsFailure)
                return Result<ScheduleOutput>.Failure(validation.Error);

                var result = GetOutput(scheduleConfig, numOccurrences);

                return result;
        }

        private Result<ScheduleOutput> GetOutput(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            switch (scheduleConfig.ScheduleType)
            {
                case Enums.ScheduleType.Once:

                    var calc = new OneTimeCalculator();
                    return calc.GetOnceOutput(scheduleConfig);

                case Enums.ScheduleType.Recurring:

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
                        return new RecurringDailyOnceCalculator().GetOutput(scheduleConfig, numOccurrences);

                    if (scheduleConfig.DailyFrequencyRangeCheckbox)
                        return new RecurringDailyRangeCalculator().GetOutput(scheduleConfig, numOccurrences);
                break;

                case Occurs.Weekly:

                    if (scheduleConfig.DailyFrequencyOnceCheckbox)
                        return new RecurringWeeklyOnceCalculator().GetOutput(scheduleConfig, numOccurrences);

                    if (scheduleConfig.DailyFrequencyRangeCheckbox)
                        return new RecurringWeeklyRangeCalculator().GetOutput(scheduleConfig, numOccurrences);
                break;

                case Occurs.Monthly:

                    if (scheduleConfig.MonthlyFrequencyDayCheckbox && scheduleConfig.DailyFrequencyOnceCheckbox)
                        return new MonthlyDayDailyOnceCalculator().GetOutput(scheduleConfig, numOccurrences);

                    if (scheduleConfig.MonthlyFrequencyDayCheckbox && scheduleConfig.DailyFrequencyRangeCheckbox)
                        return new MonthlyDayDailyRangeCalculator().GetOutput(scheduleConfig, numOccurrences);

                    if (scheduleConfig.MonthlyFrequencyTheCheckbox && scheduleConfig.DailyFrequencyOnceCheckbox)
                        return new MonthlyTheDailyOnceCalculator().GetOutput(scheduleConfig, numOccurrences);

                    if (scheduleConfig.MonthlyFrequencyTheCheckbox && scheduleConfig.DailyFrequencyRangeCheckbox)
                        return new MonthlyTheDailyRangeCalculator().GetOutput(scheduleConfig, numOccurrences);

                    if (scheduleConfig.MonthlyFrequencyDayCheckbox)
                        return new MonthlyDayCalculator().GetOutput(scheduleConfig, numOccurrences);

                    if (scheduleConfig.MonthlyFrequencyTheCheckbox)
                        return new MonthlyTheCalculator().GetOutput(scheduleConfig, numOccurrences);

                    break;
            }

            return Result<ScheduleOutput>.Failure("Recurring tasks should be daily, weekly or monthly");
        }

    }
}


