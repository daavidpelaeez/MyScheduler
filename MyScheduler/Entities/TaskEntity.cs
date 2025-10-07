using MyScheduler.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Entities
{
    public class TaskEntity
    {
        public DateTimeOffset currentDate { get; set; }
        public TypeTask typeTask { get; set; }
        public DateTimeOffset? eventDate { get; set; }
        public DateTimeOffset startDate { get; set; }
        public DateTimeOffset? endDate { get; set; }
        public int recurrence { get; set; }
        //public TypeOccurs occurs { get; set; }
       
    }
}
