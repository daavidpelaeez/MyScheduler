
using MyScheduler.Entities;
using MyScheduler.Helpers;

namespace MyScheduler.ScheduleCalculators
{
    public class OneTimeCalculator
    {
        public Result<ScheduleOutput> GetOnceOutput(ScheduleEntity scheduleConfig)
        {
            var nextExecution = scheduleConfig.OnceTypeDateExecution;

            return Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(nextExecution!.Value,DescriptionGenerator.GetDescription(scheduleConfig)));

        }
    }
}
