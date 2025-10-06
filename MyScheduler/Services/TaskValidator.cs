using MyScheduler.Entities;
using MyScheduler.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MyScheduler.Services
{
    public class TaskValidator
    {
        private TaskEntity taskEntity;

        public TaskValidator(TaskEntity taskEntity)
        {
            this.taskEntity = taskEntity;
        }

        public void ValidateTask()
        {
            if (taskEntity.typeTask == TypeTask.Once)
            {
                ValidateOnceTask();

            }
            else if (taskEntity.typeTask == TypeTask.Recurring)
            {
                ValidateRecurringTask();

            }
            else
            {
                throw new Exception("Task type not supported");
            }

            ValidateTaskDateConsistency();

        }

        public void ValidateOnceTask()
        {
            if (taskEntity.eventDate == null)
                throw new Exception("EventDate is required!");

            if (taskEntity.startDate < taskEntity.currentDate)
                throw new Exception("The start date cannot be in the past");
        }

        public void ValidateRecurringTask()
        {
            if (taskEntity.startDate == null || taskEntity.recurrence < 1)
                throw new Exception("Start date and recurrence are required!");
        }

        public void ValidateTaskDateConsistency()
        {
            if (taskEntity.endDate.HasValue)
            {
                if (taskEntity.startDate > taskEntity.endDate)
                {
                    throw new Exception("The start date cannot be after the end date");
                }

                if (taskEntity.currentDate > taskEntity.endDate)
                {
                    throw new Exception("The end date must be after the current date");
                }

                if (taskEntity.eventDate < taskEntity.startDate || taskEntity.eventDate > taskEntity.endDate)
                {
                    throw new Exception("The event date must be between start date and end date");
                }

            }
            else
            {
                if (taskEntity.eventDate < taskEntity.currentDate)
                {
                    throw new Exception("The event date cannot be in the past");
                }

                if (taskEntity.eventDate < taskEntity.startDate)
                {
                    throw new Exception("The event date cannot be before the start date");
                }
            }

        }
    }
}
