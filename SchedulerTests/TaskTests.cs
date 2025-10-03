using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyScheduler
{
    public class TaskTests
    {

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

            var exception = Record.Exception(() => taskManager.CheckDate());
            Assert.Null(exception);

        }

        [Fact]
        public void CheckDate_StartDateAfterEndDate_ThrowsException()
        {
            var task = new TaskEntity
            {
                typeTask = TypeTask.Once,
                startDate = DateTime.Now.AddDays(3),
                eventDate = DateTimeOffset.Now.AddDays(1),
                endDate = DateTimeOffset.Now.AddDays(2),
                currentDate = DateTimeOffset.Now
            };

            var taskManager = new TaskManager(task);

            var ex = Assert.Throws<Exception>(() => taskManager.CheckDate());

            Assert.Equal("The start date cannot be after the end date", ex.Message);
        }

        [Fact]
        public void CheckDate_EventDateAfterEndDate_ThrowsException()
        {
            var task = new TaskEntity
            {
                typeTask = TypeTask.Once,
                startDate = DateTime.Now.AddDays(1),
                eventDate = DateTimeOffset.Now.AddDays(6),
                endDate = DateTimeOffset.Now.AddDays(2),
                currentDate = DateTimeOffset.Now
            };

            var taskManager = new TaskManager(task);

            var ex = Assert.Throws<Exception>(() => taskManager.CheckDate());

            Assert.Equal("The event date must be between start date and end date", ex.Message);
        }

        [Fact]
        public void CheckDate_EventDateBeforeStartDate_ThrowsException()
        {
            var task = new TaskEntity
            {
                typeTask = TypeTask.Once,
                startDate = DateTime.Now.AddDays(3),
                eventDate = DateTimeOffset.Now.AddDays(1),
                endDate = DateTimeOffset.Now.AddDays(5),
                currentDate = DateTimeOffset.Now
            };

            var taskManager = new TaskManager(task);

            var ex = Assert.Throws<Exception>(() => taskManager.CheckDate());

            Assert.Equal("The event date must be between start date and end date", ex.Message);
        }

        [Fact]
        public void CheckDate_WithoutEndDate_StartDateInPast_ThrowsException()
        {
            var task = new TaskEntity
            {
                typeTask = TypeTask.Once,
                startDate = DateTime.Now.AddDays(-1),
                eventDate = DateTimeOffset.Now.AddDays(1),
                endDate = null,
                currentDate = DateTimeOffset.Now
            };

            var taskManager = new TaskManager(task);

            var ex = Assert.Throws<Exception>(() => taskManager.CheckDate());
            Assert.Equal("The start date cannot be in the past", ex.Message);
        }

        [Fact]
        public void CheckDate_WithoutEndDate_EventDateInPast_ThrowsException()
        {
            var task = new TaskEntity
            {
                typeTask = TypeTask.Once,
                eventDate = DateTimeOffset.Now.AddDays(-1),
                endDate = null,
                currentDate = DateTimeOffset.Now,
                startDate = DateTime.Now
            };

            var taskManager = new TaskManager(task);
            var ex = Assert.Throws<Exception>(() => taskManager.CheckDate());

            Assert.Equal("The event date cannot be in the past", ex.Message);
        }

        [Fact]
        public void CheckDate_WithoutEndDate_ValidDates_DoesNotThrow()
        {
            var task = new TaskEntity
            {
                typeTask = TypeTask.Once,
                currentDate = DateTimeOffset.Now,
                startDate = DateTime.Now,
                eventDate = DateTimeOffset.Now.AddDays(1),
                endDate = null,

            };
            var taskManager = new TaskManager(task);

            var ex = Record.Exception(() => taskManager.CheckDate());
            Assert.Null(ex);
        }

        [Fact]
        public void CheckGetNextExecution()
        {
            var task = new TaskEntity
            {
                typeTask = TypeTask.Once,
                currentDate = DateTimeOffset.Now,
                startDate = DateTime.Now,
                eventDate = DateTimeOffset.Now.AddDays(1),
                endDate = null,

            };
            var taskManager = new TaskManager(task);

            var ex = Record.Exception(() => taskManager.GetNextExecution());
            Assert.Null(ex);
        }

        [Fact]
        public void GetNextExecutionOnce_WithoutEventDate_ThrowsException()
        {

            var task = new TaskEntity
            {
                typeTask = TypeTask.Once,
                currentDate = DateTimeOffset.Now,
                startDate = DateTimeOffset.Now.AddDays(1),
                eventDate = null,
                endDate = null,

            };

            var taskManager = new TaskManager(task);

            var ex = Assert.Throws<Exception>(() => taskManager.GetNextExecutionOnce());

            Assert.Equal("EventDate is required!", ex.Message);

        }

        [Fact]
        public void GetNextExecutionOnce_SetsExecutionTimeAndDescriptionCorrectly()
        {
            var expectedEventDate = new DateTimeOffset(2025, 10, 5, 14, 30, 0, TimeSpan.Zero);
            var expectedStartDate = new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero);

            var task = new TaskEntity
            {
                typeTask = TypeTask.Once,
                currentDate = new DateTimeOffset(2025, 09, 1, 0, 0, 0, TimeSpan.Zero),
                startDate = expectedStartDate,
                eventDate = expectedEventDate,
                endDate = null,

            };

            var taskManager = new TaskManager(task);
            var expectedDescription = "Occurs once. Schedule will be used on 05/10/2025 at 14:30 starting on 01/10/2025";

            taskManager.GetNextExecutionOnce();

            Assert.Equal(expectedEventDate, task.executionTime);
            Assert.Equal(expectedDescription, task.description);

        }

        [Fact]
        public void GetNextExecutionRecurring_WhenStartDateIsInFuture_SetsExecutionTimeToStartDate()
        {
            var expectedDescription = "Occurs every day. Schedule will be used on 05/10/2025 starting on 05/10/2025 ";
            var task = new TaskEntity
            {
                typeTask = TypeTask.Recurring,
                currentDate = new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero),
                startDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero),
                recurrence = 2 
            };

            var taskManager = new TaskManager(task);

            taskManager.GetNextExecutionRecurring();

            Assert.Equal(task.startDate, task.executionTime);
            Assert.Equal(expectedDescription, task.description);

        }

        [Fact]
        public void GetNextExecutionRecurring_WhenCurrentDateIsOnRecurrencePeriod_SetsNextExecutionCorrectly()
        {
            var task = new TaskEntity
            {
                typeTask = TypeTask.Recurring,
                currentDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero),
                startDate = new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero),
                recurrence = 2 
            };

            var taskManager = new TaskManager(task);

            taskManager.GetNextExecutionRecurring();

            var expectedNextExecution = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero);

            Assert.Equal(expectedNextExecution, task.executionTime);
        }

        [Fact]
        public void GetNextExecutionRecurring_WhenCurrentDateEqualsStartDate_SetsNextExecution()
        {
            var start = new DateTimeOffset(2025, 10, 3, 0, 0, 0, TimeSpan.Zero);
            var task = new TaskEntity
            {
                typeTask = TypeTask.Recurring,
                currentDate = start,
                startDate = start,
                recurrence = 1
            };

            var taskManager = new TaskManager(task);

            taskManager.GetNextExecutionRecurring();

            var expected = start.AddDays(1);

            Assert.Equal(expected, task.executionTime);
        }

        [Fact]
        public void GetNextExecutionRecurring_WhenRecurrenceIsZero_ThrowsException()
        {
            var start = new DateTimeOffset(2025, 10, 3, 0, 0, 0, TimeSpan.Zero);
            var task = new TaskEntity
            {
                typeTask = TypeTask.Recurring,
                currentDate = start,
                startDate = start,
                recurrence = 0
            };


            var taskManager = new TaskManager(task);

            var ex = Assert.Throws<Exception>(() => taskManager.GetNextExecution());

            Assert.Equal("Start date and recurrence are required!", ex.Message);
        }

        [Fact]
        public void GetNextExecution_WhenTaskIsRecurring()
        {
            var start = new DateTimeOffset(2025, 10, 3, 0, 0, 0, TimeSpan.Zero);
            var task = new TaskEntity
            {
                typeTask = TypeTask.Recurring,
                currentDate = start,
                startDate = start,
                recurrence = 1
            };

            var taskManager = new TaskManager(task);

            taskManager.GetNextExecution();

            var expected = start.AddDays(1);

            Assert.Equal(expected, task.executionTime);
        }

        [Fact]
        public void GetNextExecution_WhenEventDateIsBeforeStartDate_ThowsException()
        {

            var task = new TaskEntity
            {
                typeTask = TypeTask.Recurring,
                eventDate = new DateTimeOffset(2025, 10, 6, 0, 0, 0, TimeSpan.Zero),
                currentDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero),
                startDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero),
                recurrence = 1
            };

            var taskManager = new TaskManager(task);

            var ex = Assert.Throws<Exception>(() => taskManager.GetNextExecution());

            Assert.Equal("The event date cannot be before the start date", ex.Message);

        }

        [Fact]
        public void GetNextExecution_WhenCurrentDateIsAfterEndDate_ThowsException()
        {

            var task = new TaskEntity
            {
                typeTask = TypeTask.Recurring,
                eventDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero),
                currentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero),
                startDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero),
                endDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero),
                recurrence = 1
            };

            var taskManager = new TaskManager(task);

            var ex = Assert.Throws<Exception>(() => taskManager.GetNextExecution());

            Assert.Equal("The end date must be after the current date", ex.Message);

        }

        [Fact]
        public void ValidateTaskDateConsistency_WithValidDatesAndEndDate_DoesNotThrow()
        {
            var task = new TaskEntity
            {
                startDate = new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero),
                currentDate = new DateTimeOffset(2025, 10, 2, 0, 0, 0, TimeSpan.Zero),
                endDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero),
                eventDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero)
            };

            var manager = new TaskManager(task);

            var ex = Record.Exception(() => manager.ValidateTaskDateConsistency());

            Assert.Null(ex);
        }

        [Fact]
        public void ValidateTaskDateConsistency_EventDateBeforeCurrentDate_ThrowsException()
        {
            var task = new TaskEntity
            {
                currentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero),
                startDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero),
                eventDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero), // < currentDate
                endDate = null
            };

            var manager = new TaskManager(task);

            var ex = Assert.Throws<Exception>(() => manager.ValidateTaskDateConsistency());
            Assert.Equal("The event date cannot be in the past", ex.Message);
        }

        [Fact]
        public void ValidateTaskDateConsistency_EventDateBeforeStartDate_ThrowsException()
        {
            var task = new TaskEntity
            {
                currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero),
                startDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero),
                eventDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero), // < startDate
                endDate = null
            };

            var manager = new TaskManager(task);

            var ex = Assert.Throws<Exception>(() => manager.ValidateTaskDateConsistency());
            Assert.Equal("The event date cannot be before the start date", ex.Message);
        }

        [Fact]
        public void ValidateTaskDateConsistency_EventDateBeforeStartDateWithEndDate_ThrowsException()
        {
            var task = new TaskEntity
            {
                startDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero),
                currentDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero),
                endDate = new DateTimeOffset(2025, 10, 20, 0, 0, 0, TimeSpan.Zero),
                eventDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero) // < startDate
            };

            var manager = new TaskManager(task);

            var ex = Assert.Throws<Exception>(() => manager.ValidateTaskDateConsistency());
            Assert.Equal("The event date must be between start date and end date", ex.Message);
        }

        [Fact]
        public void ValidateTaskDateConsistency_EventDateAfterEndDate_ThrowsException()
        {
            var task = new TaskEntity
            {
                startDate = new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero),
                currentDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero),
                endDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero),
                eventDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero) // eventDate > endDate
            };

            var manager = new TaskManager(task);

            var ex = Assert.Throws<Exception>(() => manager.ValidateTaskDateConsistency());

            Assert.Equal("The event date must be between start date and end date", ex.Message);
        }

        [Fact]
        public void ValidateTaskDateConsistency_CurrentDateAfterEndDate_ThrowsException()
        {
            var task = new TaskEntity
            {
                startDate = new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero),
                currentDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero),
                endDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero),
                eventDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero)
            };

            var manager = new TaskManager(task);

            var ex = Assert.Throws<Exception>(() => manager.ValidateTaskDateConsistency());

            Assert.Equal("The end date must be after the current date", ex.Message);
        }





    }
}
