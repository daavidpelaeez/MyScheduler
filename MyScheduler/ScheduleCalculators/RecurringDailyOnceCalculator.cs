using MyScheduler.Entities;
using MyScheduler.Helpers;
using System;
using System.Linq;



namespace MyScheduler.ScheduleCalculators
{
    public class RecurringDailyOnceCalculator
    {
        public Result<ScheduleOutput> GetOutput(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var nextExecution = DailySchedulerHelper.GetRecurrentDays(scheduleConfig, numOccurrences).First();

            scheduleConfig.EventDate = nextExecution;

            return Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(nextExecution, DescriptionGenerator.GetDescription(scheduleConfig)));
        }


    }
}
