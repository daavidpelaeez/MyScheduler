using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Services;
using MyScheduler.Domain.Services.Calculators;
using MyScheduler.Helpers;
using System.Linq;


namespace MyScheduler.Application.ScheduleCalculators
{
    public class RecurringDailyRangeOutput
    {
        public Result<ScheduleOutput> GetOutput(ScheduleEntity scheduleConfig, int? maxOccurrences)
        {
            var dates = new AddTime().AddHourRangeToList(scheduleConfig, maxOccurrences, RecurrenceCalculator.GetRecurrentDays(scheduleConfig, maxOccurrences));

            return dates.Count > 0 ? Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(dates.First(),DescriptionGenerator.GetDescription(scheduleConfig))) 
                : Result<ScheduleOutput>.Failure("No next execution found"); 
        }
    }
}
