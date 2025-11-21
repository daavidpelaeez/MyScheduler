using MyScheduler.Domain.Entities;
using System.Text;

namespace MyScheduler.Domain.Validators
{
    public class RecurringDailyRangeValidator
    {
        public static void Validate(ScheduleEntity scheduleConfig, StringBuilder errors)
        {
            DailyFrequencyCommonRules.CheckDailyCommonRules(scheduleConfig, errors);
        }
    }
}
