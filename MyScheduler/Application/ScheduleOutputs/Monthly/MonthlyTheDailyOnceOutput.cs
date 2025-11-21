using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Services;
using MyScheduler.Helpers;
using System.Linq;
using MyScheduler.Domain.Services.Calculators;

namespace MyScheduler.Application.ScheduleOutputs.Monthly
{
    public class MonthlyTheDailyOnceOutput
    {
        public Result<ScheduleOutput> GetOutput(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var dates = new AddTime().AddHourToList(scheduleConfig, new MonthlyTheCalculator().CalculateExecutions(scheduleConfig, numOccurrences));

            return dates.Count > 0 ? Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(dates.First(), DescriptionGenerator.GetDescription(scheduleConfig)))
                : Result<ScheduleOutput>.Failure("No next execution found");
        }
    }
}
