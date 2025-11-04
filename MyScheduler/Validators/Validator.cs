
using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Helpers;
using MyScheduler.ScheduleCalculators;
using System.Text;

namespace MyScheduler.Validators
{
    public static class Validator
    {
        public static Result<ScheduleEntity> ValidateTask(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var errors = new StringBuilder();

            CommonRules.Validate(scheduleConfig, errors, numOccurrences);

            if (errors.Length > 0)
                return Result<ScheduleEntity>.Failure(errors.ToString());

            switch (scheduleConfig.ScheduleType)
            {
                case Enums.ScheduleType.Once:

                    OnceValidator.Validate(scheduleConfig, errors);

                    break;

                case Enums.ScheduleType.Recurring:

                    ValidateRecurring(scheduleConfig, errors);

                    break;
            }

            return errors.Length == 0 ? Result<ScheduleEntity>.Success(scheduleConfig) : Result<ScheduleEntity>.Failure(errors.ToString());
        }

        public static void ValidateRecurring(ScheduleEntity scheduleConfig, StringBuilder errors)
        {
            switch (scheduleConfig.Occurs)
            {
                case Occurs.Daily:
                    if (scheduleConfig.DailyFrequencyOnceCheckbox)
                        RecurringDailyOnceValidator.Validate(scheduleConfig, errors);

                    else if (scheduleConfig.DailyFrequencyRangeCheckbox)
                        RecurringDailyRangeValidator.Validate(scheduleConfig, errors);

                    else
                        errors.AppendLine("You need to select daily frequency once or every");

                    break;

                case Occurs.Weekly:

                    if (scheduleConfig.DailyFrequencyOnceCheckbox)
                        RecurringWeeklyOnceValidator.Validate(scheduleConfig, errors);

                    else if (scheduleConfig.DailyFrequencyRangeCheckbox)
                        RecurringWeeklyRangeValidator.Validate(scheduleConfig, errors);

                    else
                        errors.AppendLine("You need to select daily frequency once or every");

                    break;

                case Occurs.Monthly:

                    RecurringMonthlyValidator.Validate(scheduleConfig, errors);

                    break;

                default:
                    errors.AppendLine("You need to select daily frequency once or every");
                    break;
            }
        }
    }

}
