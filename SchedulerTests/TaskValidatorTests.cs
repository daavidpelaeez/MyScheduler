using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Services;

namespace MyScheduler
{
    public class TaskValidatorTests
    {
        [Fact]
        public void EventSuccessWithoutEndDate_OnceTask()
        {
            var task = new TaskEntity();

            task.TypeTask = TypeTask.Once;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(task, result.Value);
        }

        [Fact]
        public void EventDateNull_ShouldFail()
        {
            var task = new TaskEntity();

            task.TypeTask = TypeTask.Once;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsFailure);
            Assert.Contains("The event date is required for tasks of type Once.", result.Error);
        }

        [Fact]
        public void RecurrenceLessThanOne_ShouldFail()
        {
            var task = new TaskEntity();

            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 0;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsFailure);
            Assert.Contains("Recurring tasks must have a recurrence greater than 0.", result.Error);
        }

        [Fact]
        public void RecurrenceLessThan0_ShouldFail()
        {
            var task = new TaskEntity();

            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = -1;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsFailure);
            Assert.Contains("Recurring tasks must have a recurrence greater than 0.", result.Error);
        }

        [Fact]
        public void EventDateBeforeTheStartDate_ShouldFail()
        {
            var task = new TaskEntity();

            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 2;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsFailure);
            Assert.Contains("The event date cannot be before the start date.", result.Error);
        }

        [Fact]
        public void EvenDateLaterThanEndDate_ShouldFail()
        {
            var task = new TaskEntity();

            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.EndDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 2;

            var result = TaskValidator.ValidateTask(task);
            Assert.True(result.IsFailure);
            Assert.Contains("The event date cannot be after the end date.", result.Error);
        }

        [Fact]
        public void StartDateAfterEndDate_ShouldFail()
        {
            var task = new TaskEntity();

            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero);
            task.EndDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 2;

            var result = TaskValidator.ValidateTask(task);
            Assert.True(result.IsFailure, result.Error);
            Assert.Contains("The start date cannot be after the end date.", result.Error);
        }

        [Fact]
        public void EventDateInThePast_ShouldFail()
        {
            var task = new TaskEntity();

            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 2;

            var result = TaskValidator.ValidateTask(task);
            Assert.True(result.IsFailure);
            Assert.Contains("The event date must be after the current date.", result.Error);
        }

        [Fact]
        public void OnceTask_WithEndDateAfterEventDate_ShouldPass()
        {
            var task = new TaskEntity();
            task.TypeTask = TypeTask.Once;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);
            task.EndDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero);

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(task, result.Value);
        }

        [Fact]
        public void OnceTask_EventDateAfterEndDate_ShouldFail()
        {
            var task = new TaskEntity();
            task.TypeTask = TypeTask.Once;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 12, 0, 0, 0, TimeSpan.Zero);
            task.EndDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero);

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsFailure);
            Assert.Contains("after the end date", result.Error.ToLower());
        }

        [Fact]
        public void RecurringTask_EndDateBeforeCurrentDate_ShouldFail()
        {
            var task = new TaskEntity();
            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.EndDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 1;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsFailure);
            Assert.Contains("end date", result.Error.ToLower());
        }

        [Fact]
        public void RecurringTask_WithoutEventDate_WithoutEndDate_RecurrencePositive_ShouldPass()
        {
            var task = new TaskEntity();
            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 3;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(task, result.Value);
        }

        [Fact]
        public void RecurringTask_StartDateAfterEndDate_ShouldFail()
        {
            var task = new TaskEntity();
            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 4, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero);
            task.EndDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 2;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsFailure);
            Assert.Contains("start date cannot be after the end date", result.Error.ToLower());
        }

        [Fact]
        public void OnceTask_WithRecurrenceGreaterThanZero_ShouldFail()
        {
            var task = new TaskEntity();
            task.TypeTask = TypeTask.Once;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 1;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsFailure);
            Assert.Contains("once", result.Error.ToLower());
        }

        [Fact]
        public void EventDateInPast_ShouldFail()
        {
            var task = new TaskEntity();
            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 1;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsFailure);
            Assert.Contains("event date must be after the current date", result.Error.ToLower());
        }

        [Fact]
        public void RecurringTask_EndDateEqualsStartDate_ShouldPass()
        {
            var task = new TaskEntity();

            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.EndDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 1;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(task, result.Value);
        }

        [Fact]
        public void RecurringTask_WithoutEventDate_ShouldPass()
        {
            var task = new TaskEntity();

            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 2;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(task, result.Value);
        }

        [Fact]
        public void EventDateEqualsCurrentDate_ShouldPass()
        {
            var task = new TaskEntity();

            task.TypeTask = TypeTask.Once;
            task.CurrentDate = new DateTimeOffset(2025, 10, 10, 10, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 10, 9, 0, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 10, 10, 0, 0, TimeSpan.Zero);

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(task, result.Value);
        }

        [Fact]
        public void EndDateEqualsCurrentDate_ShouldPass()
        {
            var task = new TaskEntity();

            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.EndDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 1;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(task, result.Value);
        }

        [Fact]
        public void MultipleErrors_ShouldReturnAllErrorMessages()
        {
            var task = new TaskEntity();

            task.TypeTask = TypeTask.Once;
            task.CurrentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 1;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsFailure);
            Assert.Contains("The event date must be after the current date.", result.Error);
            Assert.Contains("The event date cannot be before the start date.", result.Error);
            Assert.Contains("Once tasks cannot have a recurrence", result.Error);
        }


        [Fact]
        public void EventDateEqualsEndDate_ShouldPass()
        {
            var task = new TaskEntity();

            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.EndDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 1;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(task, result.Value);
        }

        [Fact]
        public void Recurrence_MoreThanLimit_ShouldFail()
        {
            var task = new TaskEntity();

            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 1200;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsFailure);
            Assert.Contains("The task recurrence cannot be more than 1000", result.Error);
        }

        [Fact]
        public void StartDate_MoreThanOneYearAgo_ShouldFail()
        {
            var task = new TaskEntity();

            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2024, 10, 7, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 10;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsFailure);
            Assert.Contains("1 year ago", result.Error);
        }

        [Fact]
        public void EndDate_More5YearsInFuture_ShouldFail()
        {
            var task = new TaskEntity();

            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero);
            task.EndDate = new DateTimeOffset(2045,10,9,0,0,0,TimeSpan.Zero);
            task.Recurrence = 10;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsFailure);
            Assert.Contains("10 years in the future", result.Error);
        }


    }
}
