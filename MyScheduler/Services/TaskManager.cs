using MyScheduler.Common;
using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Services.TaskCalculators;
using MyScheduler.Validators;

namespace MyScheduler.Services
{
    public class TaskManager
    {
        public Result<TaskOutput> GetNextExecution(TaskEntity taskConfig, int? numOccurrences)
        {
            var validation = TaskValidator.ValidateTask(taskConfig);

            if (validation.IsFailure)
            {
                return Result<TaskOutput>.Failure(validation.Error);
            }

            return taskConfig.TypeTask switch
            {
                TypeTask.Once => new OnceTaskCalculator().GetNextExecution(taskConfig),
                TypeTask.Recurring => new RecurringTaskCalculator().GetNextExecution(taskConfig, numOccurrences),
                TypeTask.WeeklyOnce => new WeeklyOnceTaskCalculator().GetNextExecution(taskConfig, numOccurrences),
                TypeTask.WeeklyEvery => new WeeklyEveryTaskCalculator().GetNextExecution(taskConfig, numOccurrences),
                _ => Result<TaskOutput>.Failure("taskConfig type not supported")
            };
        }
    }
}
