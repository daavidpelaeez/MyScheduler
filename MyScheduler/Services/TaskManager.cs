using MyScheduler.Entities;
using MyScheduler.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyScheduler.Services
{
    public class TaskManager
    {
        private TaskEntity taskEntity;
        private TaskValidator validator;

        public TaskManager(TaskEntity attributes)
        {
            this.taskEntity = attributes;
            this.validator = new TaskValidator(attributes);
        }

        public void GetNextExecution()
        {
            if (taskEntity.typeTask == TypeTask.Once)
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
            validator.ValidateTask();

            taskEntity.executionTime = (DateTimeOffset)taskEntity.eventDate;

            taskEntity.description = $"Occurs once. Schedule will be used on {taskEntity.eventDate:dd/MM/yyyy} at {taskEntity.eventDate:HH:mm}" +
                $" starting on {taskEntity.startDate:dd/MM/yyyy}";

        }

        public void GetNextExecutionRecurring()
        {
            validator.ValidateTask();
            DateTimeOffset nextExecution;

            if (taskEntity.startDate > taskEntity.currentDate)
            {
                nextExecution = taskEntity.startDate;
            }
            else
            {
                int daysPassed = (taskEntity.currentDate - taskEntity.startDate).Days; // 10 - 8 = 2

                int remainder = daysPassed % taskEntity.recurrence; // 2%3 = 2

                int daysToAdd = remainder == 0 ? taskEntity.recurrence : (taskEntity.recurrence - remainder); //3-2 = 1

                nextExecution = taskEntity.currentDate.AddDays(daysToAdd); 

            }

            taskEntity.executionTime = nextExecution;

            taskEntity.description = $"Occurs every {taskEntity.recurrence} day/s. Schedule will be used on {taskEntity.executionTime:dd/MM/yyyy} " +
                $"starting on {taskEntity.startDate:dd/MM/yyyy} ";

        }

        public SortedSet<DateTimeOffset> GetRecurrentDays(DateTimeOffset startFrom, int recurrenceDays, DateTimeOffset? endDate)
        {
            SortedSet<DateTimeOffset> listOfDays = new SortedSet<DateTimeOffset>();
            DateTimeOffset current = startFrom;
            int maxOcurrences = 10;
            int count = 0;

            while (count < maxOcurrences)
            {
                if (endDate.HasValue && current > endDate.Value)
                {
                    break;
                }

                listOfDays.Add(current);
                current = current.AddDays(recurrenceDays);
                count++;

            }

            return listOfDays;

        }
    }
}
