
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
                case Enums.ScheduleType.Once:
                    OnceValidator.Validate(scheduleConfig, errors);
                    break;

                case Enums.ScheduleType.DailyOnce:
                    DailyOnceValidator.Validate(scheduleConfig, errors);
                    break;

                case Enums.ScheduleType.WeeklyOnce:
                    WeeklyOnceValidator.Validate(scheduleConfig, errors);
                    break;

                case Enums.ScheduleType.WeeklyEvery:
                    WeeklyEveryValidator.Validate(scheduleConfig, errors);
                    break;

                case Enums.ScheduleType.DailyEvery:
                    DailyEveryValidator.Validate(scheduleConfig, errors);
                    break;

                default:
                    errors.AppendLine("Unsupported scheduleConfig type.");
                    break;
            }

            return errors.Length == 0  ? Result<ScheduleEntity>.Success(scheduleConfig) : Result<ScheduleEntity>.Failure(errors.ToString());
        }
    }

}
