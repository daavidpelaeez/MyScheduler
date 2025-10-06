using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Services;
using Xunit;

namespace MyScheduler
{
    public class TaskManagerTests
    {
        [Fact]
        public void CheckGetNextExecution()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Once;
            task.currentDate = DateTimeOffset.Now;
            task.startDate = DateTime.Now;
            task.eventDate = DateTimeOffset.Now.AddDays(1);
            task.endDate = null;

            var taskManager = new TaskManager(task);

            var ex = Record.Exception(() => taskManager.GetNextExecution());

            Assert.Null(ex);
        }

        [Fact]
        public void GetNextExecutionOnce_WithoutEventDate_ThrowsException()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Once;
            task.currentDate = DateTimeOffset.Now;
            task.startDate = DateTimeOffset.Now.AddDays(1);
            task.eventDate = null;
            task.endDate = null;

            var taskManager = new TaskManager(task);

            var ex = Assert.Throws<Exception>(() => taskManager.GetNextExecutionOnce());

            Assert.Equal("EventDate is required!", ex.Message);
        }

        [Fact]
        public void GetNextExecutionOnce_SetsExecutionTimeAndDescriptionCorrectly()
        {
            var expectedEventDate = new DateTimeOffset(2025, 10, 5, 14, 30, 0, TimeSpan.Zero);
            var expectedStartDate = new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero);

            var task = new TaskEntity();

            task.typeTask = TypeTask.Once;
            task.currentDate = new DateTimeOffset(2025, 9, 1, 0, 0, 0, TimeSpan.Zero);
            task.startDate = expectedStartDate;
            task.eventDate = expectedEventDate;
            task.endDate = null;

            var taskManager = new TaskManager(task);
            var expectedDescription = "Occurs once. Schedule will be used on 05/10/2025 at 14:30 starting on 01/10/2025";

            taskManager.GetNextExecutionOnce();

            Assert.Equal(expectedEventDate, task.executionTime);
            Assert.Equal(expectedDescription, task.description);
        }

        [Fact]
        public void GetNextExecutionRecurring_WhenStartDateIsInFuture_SetsExecutionTimeToStartDate()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Recurring;
            task.currentDate = new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 2;

            var taskManager = new TaskManager(task);

            taskManager.GetNextExecutionRecurring();
            var expectedDescription = $"Occurs every {task.recurrence} day/s. Schedule will be used on 05/10/2025 starting on 05/10/2025 ";

            Assert.Equal(task.startDate, task.executionTime);
            Assert.Equal(expectedDescription, task.description);
        }

        [Fact]
        public void GetNextExecutionRecurring_WhenCurrentDateIsOnRecurrencePeriod_SetsNextExecutionCorrectly()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Recurring;
            task.currentDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 2;

            var taskManager = new TaskManager(task);

            taskManager.GetNextExecutionRecurring();

            var expectedNextExecution = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero);

            Assert.Equal(expectedNextExecution, task.executionTime);
        }

        [Fact]
        public void GetNextExecutionRecurring_WhenCurrentDateEqualsStartDate_SetsNextExecution()
        {
            var start = new DateTimeOffset(2025, 10, 3, 0, 0, 0, TimeSpan.Zero);

            var task = new TaskEntity();

            task.typeTask = TypeTask.Recurring;
            task.currentDate = start;
            task.startDate = start;
            task.recurrence = 1;

            var taskManager = new TaskManager(task);

            taskManager.GetNextExecutionRecurring();

            var expected = start.AddDays(1);

            Assert.Equal(expected, task.executionTime);
        }

        [Fact]
        public void GetNextExecutionRecurring_WhenRecurrenceIsZero_ThrowsException()
        {
            var start = new DateTimeOffset(2025, 10, 3, 0, 0, 0, TimeSpan.Zero);

            var task = new TaskEntity();

            task.typeTask = TypeTask.Recurring;
            task.currentDate = start;
            task.startDate = start;
            task.recurrence = 0;

            var taskManager = new TaskManager(task);

            var ex = Assert.Throws<Exception>(() => taskManager.GetNextExecution());

            Assert.Equal("Start date and recurrence are required!", ex.Message);
        }

        [Fact]
        public void GetNextExecution_WhenTaskIsRecurring()
        {
            var start = new DateTimeOffset(2025, 10, 3, 0, 0, 0, TimeSpan.Zero);

            var task = new TaskEntity();

            task.typeTask = TypeTask.Recurring;
            task.currentDate = start;
            task.startDate = start;
            task.recurrence = 1;

            var taskManager = new TaskManager(task);

            taskManager.GetNextExecution();

            var expected = start.AddDays(1);

            Assert.Equal(expected, task.executionTime);
        }

        [Fact]
        public void GetNextExecution_WhenEventDateIsBeforeStartDate_ThrowsException()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Recurring;
            task.eventDate = new DateTimeOffset(2025, 10, 6, 0, 0, 0, TimeSpan.Zero);
            task.currentDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 1;

            var taskManager = new TaskManager(task);

            var ex = Assert.Throws<Exception>(() => taskManager.GetNextExecution());

            Assert.Equal("The event date cannot be before the start date", ex.Message);
        }

        [Fact]
        public void GetNextExecution_WhenCurrentDateIsAfterEndDate_ThrowsException()
        {
            var task = new TaskEntity();

            task.typeTask = TypeTask.Recurring;
            task.eventDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero);
            task.currentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.endDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 1;

            var taskManager = new TaskManager(task);

            var ex = Assert.Throws<Exception>(() => taskManager.GetNextExecution());

            Assert.Equal("The end date must be after the current date", ex.Message);
        }

        [Fact]
        public void GetRecurrentDays_WithoutEndDate_ReturnsMaxOccurrences()
        {
            var task = new TaskEntity();
            task.startDate = new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero);
            
            var startFrom = task.startDate;
            int recurrenceDays = 3;
            DateTimeOffset? endDate = null;

            var taskManager = new TaskManager(task);

            
            SortedSet<DateTimeOffset> result = taskManager.GetRecurrentDays(startFrom, recurrenceDays, endDate);

   
            Assert.NotNull(result);
            Assert.Equal(10, result.Count);

            DateTimeOffset expectedDate = startFrom;
            foreach (var date in result)
            {
                Assert.Equal(expectedDate, date);
                expectedDate = expectedDate.AddDays(recurrenceDays);
            }
        }

        [Fact]
        public void GetRecurrentDays_WithEndDate()
        {
            var task = new TaskEntity();
            task.startDate = new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero);
            task.endDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 3;

            var taskManager = new TaskManager(task);


            SortedSet<DateTimeOffset> result = taskManager.GetRecurrentDays(task.startDate, task.recurrence, task.endDate);


            Assert.NotNull(result);
            Assert.Equal(4, result.Count);

            DateTimeOffset expectedDate = task.startDate;
            foreach (var date in result)
            {
                Assert.Equal(expectedDate, date);
                expectedDate = expectedDate.AddDays(task.recurrence);
            }
        }

        [Fact]
        public void GetRecurrentDays_WithRemainder_Equals0()
        {
            var task = new TaskEntity();
            task.startDate = new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero);
            task.endDate = null;
            task.currentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 3;

            var taskManager = new TaskManager(task);


            SortedSet<DateTimeOffset> result = taskManager.GetRecurrentDays(task.startDate, task.recurrence, task.endDate);


            Assert.NotNull(result);
            Assert.Equal(10, result.Count);

            DateTimeOffset expectedDate = task.startDate;
            foreach (var date in result)
            {
                Assert.Equal(expectedDate, date);
                expectedDate = expectedDate.AddDays(task.recurrence);
            }
        }
    }
}
