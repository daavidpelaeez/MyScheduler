using MyScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class RecurringDailyOnceValidator
    {
        public static void Validate(ScheduleEntity scheduleConfig, StringBuilder errors)
        {
            DailyFrequencyCommonRules.CheckDailyCommonRules(scheduleConfig, errors);

            if (scheduleConfig.Recurrence < 1)
                errors.AppendLine("RecurringDailyOnce tasks must have a recurrence greater than 0.");
            
        }
    }

}
