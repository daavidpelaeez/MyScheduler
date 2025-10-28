﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Entities
{
    public struct ScheduleOutput 
    {
        public DateTimeOffset ExecutionTime { get; set; } 
        public string Description { get; set; } 

        public ScheduleOutput(DateTimeOffset executionTime, string description)
        {
            ExecutionTime = executionTime;
            Description = description;
        }
    }
}
