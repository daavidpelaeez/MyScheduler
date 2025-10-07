using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Services;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyScheduler
{
    public class TaskValidatorTests
    {
        [Fact]
        public void EventSuccessWithoutEndDate_OnceTask()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Once;
            task.currentDate = new DateTimeOffset(2025,10,8,0,0,0,TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(task, result.Value);
        }

        [Fact]
        public void StartDateInThePast_ShouldFail()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Once;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsFailure);
            Assert.Equal("The start date cannot be in the past",result.Error);
        }

        [Fact]
        public void EventDateNull_ShouldFail()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Once;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsFailure);
            Assert.Equal("The event date is required!", result.Error);
        }

        [Fact]
        public void RecurrenceLessThanOne_ShouldFail()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Recurring;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 0;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsFailure);
            Assert.Equal("Recurrence must be at least 1", result.Error);
        }

        [Fact]
        public void EventDateBeforeTheStartDate_ShouldFail()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Recurring;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 2;

            var result = TaskValidator.ValidateTask(task);

            Assert.True(result.IsFailure);
            Assert.Equal("The event date cannot be before the start date", result.Error);
        }

        [Fact]
        public void EvenDateLaterThanEndDate_ShouldFail()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Recurring;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.endDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 2;

            var result = TaskValidator.ValidateTask(task);
            Assert.True(result.IsFailure);
            Assert.Equal("The event date must be between the start and end date", result.Error);
        }

        [Fact]
        public void StartDateAfterEndDate_ShouldFail()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Recurring;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero);
            task.endDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 2;

            var result = TaskValidator.ValidateTask(task);
            Assert.True(result.IsFailure);
            Assert.Equal("The start date cannot be after the end date", result.Error);
        }

        [Fact]
        public void EventDateInThePast_ShouldFail()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Recurring;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 2;

            var result = TaskValidator.ValidateTask(task);
            Assert.True(result.IsFailure);
            Assert.Equal("The event date cannot be in the past", result.Error);
        }

        [Fact]
        public void EventSuccessWithEndDate_RecurringTask()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Recurring;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.endDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 1;

            var result = TaskValidator.ValidateTask(task);
            Assert.True(result.IsSuccess);
            Assert.Equal(task, result.Value);
        }

        [Fact]
        public void TypeTaskNotSupported_ShouldFail()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Unsupported;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 0;

            var result = TaskValidator.ValidateTask(task);
            Assert.True(result.IsFailure);
            Assert.Equal("Type task not supported", result.Error);
        }


    }
}
