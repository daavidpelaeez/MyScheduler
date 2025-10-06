using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Services;
using Xunit;

namespace MyScheduler
{
    public class TaskValidatorTests
    {
        [Fact]
        public void CheckDate_StartDateAfterEndDate_ThrowsException()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Once;
            task.startDate = DateTime.Now.AddDays(3);
            task.eventDate = DateTimeOffset.Now.AddDays(1);
            task.endDate = DateTimeOffset.Now.AddDays(2);
            task.currentDate = DateTimeOffset.Now;

            var taskValidator = new TaskValidator(task);

            var ex = Assert.Throws<Exception>(() => taskValidator.ValidateTask());

            Assert.Equal("The start date cannot be after the end date", ex.Message);
        }

        [Fact]
        public void CheckDate_EventDateAfterEndDate_ThrowsException()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Once;
            task.startDate = DateTime.Now.AddDays(1);
            task.eventDate = DateTimeOffset.Now.AddDays(6);
            task.endDate = DateTimeOffset.Now.AddDays(2);
            task.currentDate = DateTimeOffset.Now;

            var taskValidator = new TaskValidator(task);

            var ex = Assert.Throws<Exception>(() => taskValidator.ValidateTask());

            Assert.Equal("The event date must be between start date and end date", ex.Message);
        }

        [Fact]
        public void CheckDate_EventDateBeforeStartDate_ThrowsException()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Once;
            task.startDate = DateTime.Now.AddDays(3);
            task.eventDate = DateTimeOffset.Now.AddDays(1);
            task.endDate = DateTimeOffset.Now.AddDays(5);
            task.currentDate = DateTimeOffset.Now;

            var taskValidator = new TaskValidator(task);

            var ex = Assert.Throws<Exception>(() => taskValidator.ValidateTask());

            Assert.Equal("The event date must be between start date and end date", ex.Message);
        }

        [Fact]
        public void CheckDate_WithoutEndDate_StartDateInPast_ThrowsException()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Once;
            task.startDate = DateTime.Now.AddDays(-1);
            task.eventDate = DateTimeOffset.Now.AddDays(1);
            task.endDate = null;
            task.currentDate = DateTimeOffset.Now;

            var taskValidator = new TaskValidator(task);

            var ex = Assert.Throws<Exception>(() => taskValidator.ValidateTask());

            Assert.Equal("The start date cannot be in the past", ex.Message);
        }

        [Fact]
        public void CheckDate_WithoutEndDate_EventDateInPast_ThrowsException()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Once;
            task.eventDate = DateTimeOffset.Now.AddDays(-1);
            task.endDate = null;
            task.currentDate = DateTimeOffset.Now;
            task.startDate = DateTime.Now;

            var taskValidator = new TaskValidator(task);

            var ex = Assert.Throws<Exception>(() => taskValidator.ValidateTask());

            Assert.Equal("The event date cannot be in the past", ex.Message);
        }

        [Fact]
        public void CheckDate_WithoutEndDate_ValidDates_DoesNotThrow()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Once;
            task.eventDate = DateTimeOffset.Now.AddDays(1);
            task.endDate = null;
            task.currentDate = DateTimeOffset.Now;
            task.startDate = DateTime.Now;

            var taskValidator = new TaskValidator(task);

            var ex = Record.Exception(() => taskValidator.ValidateTask());

            Assert.Null(ex);
        }

        [Fact]
        public void ValidateTaskDateConsistency_WithValidDatesAndEndDate_DoesNotThrow()
        {
            var task = new TaskEntity();

            task.startDate = new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero);
            task.currentDate = new DateTimeOffset(2025, 10, 2, 0, 0, 0, TimeSpan.Zero);
            task.endDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);

            var taskValidator = new TaskValidator(task);

            var ex = Record.Exception(() => taskValidator.ValidateTaskDateConsistency());

            Assert.Null(ex);
        }

        [Fact]
        public void ValidateTaskDateConsistency_EventDateBeforeCurrentDate_ThrowsException()
        {
            var task = new TaskEntity();

            task.currentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.endDate = null;

            var taskValidator = new TaskValidator(task);

            var ex = Assert.Throws<Exception>(() => taskValidator.ValidateTaskDateConsistency());

            Assert.Equal("The event date cannot be in the past", ex.Message);
        }

        [Fact]
        public void ValidateTaskDateConsistency_EventDateBeforeStartDate_ThrowsException()
        {
            var task = new TaskEntity();

            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.endDate = null;

            var taskValidator = new TaskValidator(task);

            var ex = Assert.Throws<Exception>(() => taskValidator.ValidateTaskDateConsistency());

            Assert.Equal("The event date cannot be before the start date", ex.Message);
        }

        [Fact]
        public void ValidateTaskDateConsistency_EventDateBeforeStartDateWithEndDate_ThrowsException()
        {
            var task = new TaskEntity();

            task.startDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.currentDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.endDate = new DateTimeOffset(2025, 10, 20, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);

            var taskValidator = new TaskValidator(task);

            var ex = Assert.Throws<Exception>(() => taskValidator.ValidateTaskDateConsistency());

            Assert.Equal("The event date must be between start date and end date", ex.Message);
        }

        [Fact]
        public void ValidateTaskDateConsistency_EventDateAfterEndDate_ThrowsException()
        {
            var task = new TaskEntity();

            task.startDate = new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero);
            task.currentDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.endDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);

            var taskValidator = new TaskValidator(task);

            var ex = Assert.Throws<Exception>(() => taskValidator.ValidateTaskDateConsistency());

            Assert.Equal("The event date must be between start date and end date", ex.Message);
        }

        [Fact]
        public void ValidateTaskDateConsistency_CurrentDateAfterEndDate_ThrowsException()
        {
            var task = new TaskEntity();

            task.startDate = new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero);
            task.currentDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero);
            task.endDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);

            var taskValidator = new TaskValidator(task);

            var ex = Assert.Throws<Exception>(() => taskValidator.ValidateTaskDateConsistency());

            Assert.Equal("The end date must be after the current date", ex.Message);
        }

        [Fact]
        public void ValidateOnceTaskDate_WithoutEventDate_ThrowsException()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Once;
            task.startDate = new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero);
            task.currentDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero);
            task.endDate = new DateTimeOffset(2025, 10, 12, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = null;

            var taskValidator = new TaskValidator(task);

            var ex = Assert.Throws<Exception>(() => taskValidator.ValidateOnceTask());

            Assert.Equal("EventDate is required!", ex.Message);
        }

        [Fact]
        public void ValidateRecurringTask()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Recurring;
            task.startDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.currentDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.endDate = new DateTimeOffset(2025, 10, 12, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero);

            var taskValidator = new TaskValidator(task);

            var ex = Assert.Throws<Exception>(() => taskValidator.ValidateTask());

            Assert.Equal("Start date and recurrence are required!", ex.Message);
        }

        [Fact]
        public void ValidateTask_TaskTypeNotSupported_ThrowsException()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Unsupported;
            task.startDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.currentDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.endDate = new DateTimeOffset(2025, 10, 12, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero);

            var taskValidator = new TaskValidator(task);

            var ex = Assert.Throws<Exception>(() => taskValidator.ValidateTask());

            Assert.Equal("Task type not supported", ex.Message);
        }


        [Fact]
        public void ShouldCheckDate_WithValidOnceTask_DoesNotThrow()
        {
            var task = new TaskEntity
            {
                typeTask = TypeTask.Once,
                eventDate = DateTimeOffset.Now.AddDays(1),
                endDate = DateTimeOffset.Now.AddDays(2),
                currentDate = DateTimeOffset.Now,
                startDate = DateTime.Now
            };

            var taskManager = new TaskManager(task);

            var exception = Record.Exception(() => taskManager);
            Assert.Null(exception);

        }
    }
}
