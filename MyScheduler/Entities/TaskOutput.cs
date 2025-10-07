using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Entities
{
    public class TaskOutput
    {
        public DateTimeOffset executionTime { get; set; }
        public string description { get; set; }

    }
}
