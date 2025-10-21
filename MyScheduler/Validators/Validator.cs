using MyScheduler.Common;
using MyScheduler.Entities;
using MyScheduler.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class Validator
    {
        public static Result<ScheduleEntity> ValidateTask(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var errors = new StringBuilder();

            CommonRules.Validate(scheduleConfig, errors,numOccurrences);

            switch (scheduleConfig.Type)
            {
                case Enums.Type.Once:
                    OnceValidator.Validate(scheduleConfig, errors);
                    break;

                case Enums.Type.Recurring:
                    RecurringValidator.Validate(scheduleConfig, errors);
                    break;

                case Enums.Type.WeeklyOnce:
                    WeeklyOnceValidator.Validate(scheduleConfig, errors);
                    break;

                case Enums.Type.WeeklyEvery:
                    WeeklyEveryValidator.Validate(scheduleConfig, errors);
                    break;

                default:
                    errors.AppendLine("Unsupported scheduleConfig type.");
                    break;
            }

            return errors.Length == 0  ? Result<ScheduleEntity>.Success(scheduleConfig) : Result<ScheduleEntity>.Failure(errors.ToString());
        }
    }

}
