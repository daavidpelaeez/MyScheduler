using MyScheduler.Domain.Entities;
using MyScheduler.Helpers;

namespace MyScheduler.Application.ScheduleCalculators.Once
{
    public class OneTimeOutput
    {
        public Result<ScheduleOutput> GetOnceOutput(ScheduleEntity scheduleConfig)
        {
            var nextExecution = scheduleConfig.OnceTypeDateExecution;
            return nextExecution.HasValue
                ? Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(nextExecution.Value, DescriptionGenerator.GetDescription(scheduleConfig)))
                : Result<ScheduleOutput>.Failure("No next execution found");
        }
    }
}
