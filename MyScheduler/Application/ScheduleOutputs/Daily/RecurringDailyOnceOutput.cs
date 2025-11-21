using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Services.Calculators;
using MyScheduler.Helpers;
using System.Linq;



namespace MyScheduler.Application.ScheduleCalculators.Daily
{
    public class RecurringDailyOnceOutput
    {
        public Result<ScheduleOutput> GetOutput(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var nextExecution = RecurrenceCalculator.GetRecurrentDays(scheduleConfig, numOccurrences).First();

            scheduleConfig.OnceTypeDateExecution = nextExecution;

            return Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(nextExecution, DescriptionGenerator.GetDescription(scheduleConfig)));
        }
    }
}
