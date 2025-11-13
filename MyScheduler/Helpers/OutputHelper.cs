using MyScheduler.Entities;
using System;

namespace MyScheduler.Helpers
{
    public static class OutputHelper
    {
        public static ScheduleOutput OutputBuilder(DateTimeOffset executionTime, string description)
        {
            var output = new ScheduleOutput();
            output.ExecutionTime = executionTime;
            output.Description = description;

            return output;

        }

    }
}
