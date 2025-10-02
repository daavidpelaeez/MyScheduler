using System;

namespace MyScheduler
{
    public class Task
    {
        public DateTime currentDate { get; set; }
        public TypeTask TypeTask { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int days { get; set; }
        public String description { get; set; }


        public void getNextExecution()
        {
            if (TypeTask.Equals(TypeTask.Once))
            {
                getNextExecutionOnce();
            }
            else
            {
                //getNextExecutionRecurring();
            }
        }

        public ExecutionResult getNextExecutionOnce()
        {
            return new ExecutionResult
            {
                executionTime = EventDate,
                description = description,
            };
        }




        //public ExecutionResult getNextExecutionRecurring()
        //{

        //}









    }
}
