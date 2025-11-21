using MyScheduler.Domain.Entities;
using System.Text;

namespace MyScheduler.Domain.Validators
{
    public static class RecurringDailyOnceValidator
    {
        public static void Validate(ScheduleEntity scheduleConfig, StringBuilder errors)
        {
            DailyFrequencyCommonRules.CheckDailyCommonRules(scheduleConfig, errors);

            if (scheduleConfig.Recurrence < 1)
                errors.AppendLine("RecurringDailyOnce tasks must have a recurrence greater than 0.");

            if (!scheduleConfig.DailyOnceExecutionTime.HasValue)
                errors.AppendLine("DailyOnceExecution time its required!");
        }
    }

}
