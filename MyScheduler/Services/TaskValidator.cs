using MyScheduler.Common;
using MyScheduler.Entities;
using MyScheduler.Enums;
using System;

namespace MyScheduler.Services
{
    public static class TaskValidator
    {
        public static Result<TaskEntity> ValidateTask(TaskEntity task)
        {
            Result<TaskEntity> typeValidation = task.typeTask switch
            {
                TypeTask.Once => ValidateOnceTask(task),
                TypeTask.Recurring => ValidateRecurringTask(task),
                _ => Result<TaskEntity>.Failure("Type task not supported")
            };

            if (typeValidation.IsFailure)
            {
                return typeValidation;
            }

            var dateValidation = ValidateTaskDateConsistency(task);

            if (dateValidation.IsFailure)
            {
                return Result<TaskEntity>.Failure(dateValidation.Error);
            }

            return Result<TaskEntity>.Success(task);

        }

        public static Result<TaskEntity> ValidateOnceTask(TaskEntity task)
        {

            if (task.startDate < task.currentDate)
            {
                return Result<TaskEntity>.Failure("The start date cannot be in the past");
            }

            if (!task.eventDate.HasValue)
            {
                return Result<TaskEntity>.Failure("The event date is required!");
            }

            return Result<TaskEntity>.Success(task);

        }

        public static Result<TaskEntity> ValidateRecurringTask(TaskEntity task)
        {
            if (task.recurrence < 1)
            {
                return Result<TaskEntity>.Failure("Recurrence must be at least 1");
            }

            return Result<TaskEntity>.Success(task);

        }

        public static Result<TaskEntity> ValidateTaskDateConsistency(TaskEntity task)
        { 

            if (task.eventDate < task.startDate)
            {
                return Result<TaskEntity>.Failure("The event date cannot be before the start date");
            }
                
            if (task.endDate.HasValue)
            {
                if (task.startDate > task.endDate.Value)
                {
                    return Result<TaskEntity>.Failure("The start date cannot be after the end date");
                }
                  
                if (task.eventDate > task.endDate.Value)
                {
                    return Result<TaskEntity>.Failure("The event date must be between the start and end date");
                }
            }
            else
            {
                if (task.eventDate < task.currentDate)
                {
                    return Result<TaskEntity>.Failure("The event date cannot be in the past");
                }
                    
            }

            return Result<TaskEntity>.Success(task);
        }

    }
}
