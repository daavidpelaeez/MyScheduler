using MyScheduler.Common;
using MyScheduler.Entities;
using MyScheduler.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class TaskValidator
    {
        public static Result<TaskEntity> ValidateTask(TaskEntity task)
        {
            var errors = new StringBuilder();

            TaskCommonRules.Validate(task, errors);

            switch (task.TypeTask)
            {
                case TypeTask.Once:
                    OnceTaskValidator.Validate(task, errors);
                    break;

                case TypeTask.Recurring:
                    RecurringTaskValidator.Validate(task, errors);
                    break;

                case TypeTask.WeeklyOnce:
                    WeeklyOnceTaskValidator.Validate(task, errors);
                    break;

                case TypeTask.WeeklyEvery:
                    WeeklyEveryTaskValidator.Validate(task, errors);
                    break;

                default:
                    errors.AppendLine("Unsupported task type.");
                    break;
            }

            return errors.Length == 0  ? Result<TaskEntity>.Success(task) : Result<TaskEntity>.Failure(errors.ToString());
        }
    }

}
