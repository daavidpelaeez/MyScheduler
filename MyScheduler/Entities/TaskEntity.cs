using MyScheduler.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Entities
{
    public class TaskEntity
    {
        public DateTimeOffset CurrentDate { get; set; }
        public TypeTask TypeTask { get; set; }
        public DateTimeOffset? EventDate { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int Recurrence { get; set; }
       
    }
}
