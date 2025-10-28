using MyScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class DailyOnceValidator
    {
        public static void Validate(ScheduleEntity scheduleConfig, StringBuilder errors)
        {
            DailyFrequencyCommonRules.CheckDailyCommonRules(scheduleConfig, errors);

            if (scheduleConfig.Recurrence < 1)
                errors.AppendLine("DailyOnce tasks must have a recurrence greater than 0.");
            
        }
    }

}
