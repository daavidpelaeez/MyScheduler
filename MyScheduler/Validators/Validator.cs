
using MyScheduler.Entities;
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

            CommonRules.Validate(scheduleConfig, errors,numOccurrences);

            switch (scheduleConfig.ScheduleType)
            {
                case Enums.ScheduleType.OneTime:
                    OnceValidator.Validate(scheduleConfig, errors);
                    break;

                case Enums.ScheduleType.RecurringDailyOnce:
                    RecurringDailyOnceValidator.Validate(scheduleConfig, errors);
                    break;

                case Enums.ScheduleType.RecurringWeeklyOnce:
                    RecurringWeeklyOnceValidator.Validate(scheduleConfig, errors);
                    break;

                case Enums.ScheduleType.RecurringWeeklyRange:
                    RecurringWeeklyRangeValidator.Validate(scheduleConfig, errors);
                    break;

                case Enums.ScheduleType.RecurringDailyRange:
                    RecurringDailyRangeValidator.Validate(scheduleConfig, errors);
                    break;

                default:
                    errors.AppendLine("Unsupported scheduleConfig type.");
                    break;
            }

            return errors.Length == 0  ? Result<ScheduleEntity>.Success(scheduleConfig) : Result<ScheduleEntity>.Failure(errors.ToString());
        }
    }

}
