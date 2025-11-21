using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Services;
using MyScheduler.Domain.Services.Calculators;
using MyScheduler.Helpers;
using System.Linq;


namespace MyScheduler.Application.ScheduleCalculators.Monthly
{
    public class MonthlyDayDailyOnceOutput
    {
        public Result<ScheduleOutput> GetOutput(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var dates = new AddTime().AddHourToList(scheduleConfig, new MonthlyDayCalculator().CalculateExecutions(scheduleConfig, numOccurrences));

            return dates.Count > 0 ? Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(dates.First(), DescriptionGenerator.GetDescription(scheduleConfig)))
                : Result<ScheduleOutput>.Failure("No next execution found");
        }
        
    }
}
