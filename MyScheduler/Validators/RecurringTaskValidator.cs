using MyScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class RecurringTaskValidator
    {
        public static void Validate(TaskEntity task, StringBuilder errors)
        {
            if (task.Recurrence < 1)
                errors.AppendLine("Recurring tasks must have a recurrence greater than 0.");
        }
    }

}
