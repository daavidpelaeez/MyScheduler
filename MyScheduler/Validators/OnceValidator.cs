using MyScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class OnceValidator
    {
        public static void Validate(ScheduleEntity scheduleConfig, StringBuilder errors)
        {
            if (!scheduleConfig.EventDate.HasValue)
                errors.AppendLine("EventDate is required for OneTime tasks.");

            if (scheduleConfig.Recurrence > 0)
                errors.AppendLine("OneTime tasks cannot have a recurrence.");
        }
    }

}
