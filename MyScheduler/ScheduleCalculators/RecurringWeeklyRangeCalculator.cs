
using MyScheduler.Entities;
using MyScheduler.Helpers;
using System.Linq;

namespace MyScheduler.ScheduleCalculators
{
    public class RecurringWeeklyRangeCalculator
    {
        public Result<ScheduleOutput> GetOutput(ScheduleEntity scheduleConfig, int? maxOccurrences)
        {
            var dates = new AddHoursHelper().AddHourRangeToList(scheduleConfig,maxOccurrences, WeeklyScheduleHelper.GetMatchingDays(scheduleConfig, maxOccurrences));

            return (dates.Count > 0) ? Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(dates.First(), DescriptionGenerator.GetDescription(scheduleConfig))) :
                Result<ScheduleOutput>.Failure("No next execution found"); 
        }

    }
}
