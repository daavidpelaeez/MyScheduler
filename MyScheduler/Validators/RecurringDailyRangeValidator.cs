using MyScheduler.Entities;
using System.Text;

namespace MyScheduler.Validators
{
    public class RecurringDailyRangeValidator
    {
        public static void Validate(ScheduleEntity scheduleConfig, StringBuilder errors)
        {
            DailyFrequencyCommonRules.CheckDailyCommonRules(scheduleConfig, errors);
        }
    }
}
