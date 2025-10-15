using MyScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class OnceTaskValidator
    {
        public static void Validate(TaskEntity task, StringBuilder errors)
        {
            if (!task.EventDate.HasValue)
                errors.AppendLine("EventDate is required for Once tasks.");

            if (task.Recurrence > 0)
                errors.AppendLine("Once tasks cannot have a recurrence.");
        }
    }

}
