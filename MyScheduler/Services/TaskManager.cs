using MyScheduler.Entities;
using MyScheduler.Enums;
using System;

namespace MyScheduler.Services
{
    public class TaskManager
    {
        private TaskEntity attributes;

        public TaskManager(TaskEntity attributes)
        {
            this.attributes = attributes;
        }

        public void GetNextExecution()
        {
            if (attributes.typeTask == TypeTask.Once)
            {
                GetNextExecutionOnce();
            }
            else
            {
                GetNextExecutionRecurring();
            }
        }

        public void GetNextExecutionOnce()
        {
            CheckDate();
            attributes.executionTime = (DateTimeOffset)attributes.eventDate;
            attributes.description = $"Occurs once. Schedule will be used on {attributes.eventDate:dd/MM/yyyy} at {attributes.eventDate:HH:mm}" +
                $" starting on {attributes.startDate:dd/MM/yyyy}";

        }

        public void GetNextExecutionRecurring()
        {
            CheckDate();

            if (attributes.startDate > attributes.currentDate)
            {
                attributes.executionTime = attributes.startDate;
            }
            else
            {
                int daysSinceStarted = (attributes.currentDate - attributes.startDate).Days;

                int periods = daysSinceStarted / attributes.recurrence;

                attributes.executionTime = attributes.startDate.AddDays((periods + 1) * attributes.recurrence);
            }

            attributes.description = $"Occurs every day. Schedule will be used on {attributes.executionTime:dd/MM/yyyy} " +
                $"starting on {attributes.startDate:dd/MM/yyyy} ";

        }

        public void CheckDate()
        {

            if (attributes.typeTask == TypeTask.Once)
            {
                if (attributes.eventDate == null)
                {
                    throw new Exception("EventDate is required!");
                }

                if(attributes.startDate < attributes.currentDate)
                {
                    throw new Exception("The start date cannot be in the past");
                }
            }

            if (attributes.typeTask == TypeTask.Recurring)
            {
                if (attributes.startDate == null || attributes.recurrence < 1)
                {
                    throw new Exception("Start date and recurrence are required!");
                }

            }

            ValidateTaskDateConsistency();

        }

        public void ValidateTaskDateConsistency()
        {
            if (attributes.endDate.HasValue)
            {
                if (attributes.startDate > attributes.endDate)
                {
                    throw new Exception("The start date cannot be after the end date");
                }

                if (attributes.currentDate > attributes.endDate)
                {
                    throw new Exception("The end date must be after the current date");
                }

                if (attributes.eventDate < attributes.startDate || attributes.eventDate > attributes.endDate)
                {
                    throw new Exception("The event date must be between start date and end date");
                }

            }
            else
            {
                if (attributes.eventDate < attributes.currentDate)
                {
                    throw new Exception("The event date cannot be in the past");
                }

                if (attributes.eventDate < attributes.startDate)
                {
                    throw new Exception("The event date cannot be before the start date");
                }
            }

        }



    }
}
