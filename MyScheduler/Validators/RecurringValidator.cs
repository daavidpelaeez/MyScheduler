using MyScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class RecurringValidator
    {
        public static void Validate(ScheduleEntity scheduleConfig, StringBuilder errors)
        {
            if (scheduleConfig.Recurrence < 1)
                errors.AppendLine("Recurring tasks must have a recurrence greater than 0.");
        }
    }

}
