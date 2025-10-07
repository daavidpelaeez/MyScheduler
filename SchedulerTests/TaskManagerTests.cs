using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Services;
using System.Threading.Tasks;
using Xunit;

namespace MyScheduler
{
    public class TaskManagerTests
    {
        [Fact]
        public void GetNextExecutionOnce_ShouldReturnCorrectOutput()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();

            task.typeTask = TypeTask.Once;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero); 
            
            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(task.eventDate.Value, result.Value.executionTime);

            string expectedDescription = "Occurs once. Schedule will be used on 10/10/2025 at 16:30 starting on 09/10/2025";

            Assert.Equal(expectedDescription, result.Value.description);
        }

        [Fact]
        public void NextExecutionRecurring_StartDateAfterCurrent()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity
            {
                typeTask = TypeTask.Recurring,
                currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero),
                startDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero),
                recurrence = 1,
            };

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsSuccess, result.Error); 
            Assert.Equal(task.startDate, result.Value.executionTime);

            string expectedDescription = "Occurs every 1 day/s. Schedule will be used on 09/10/2025 starting on 09/10/2025 ";

            Assert.Equal(expectedDescription, result.Value.description);
        }

        [Fact]
        public void NextExecutionRecurring_StartDateBeforeCurrent()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();

            task.typeTask = TypeTask.Recurring;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);
            task.recurrence = 1;

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero), result.Value.executionTime);

            string expectedDescription = "Occurs every 1 day/s. Schedule will be used on 08/10/2025 starting on 05/10/2025 ";

            Assert.Equal(expectedDescription, result.Value.description);
        }


        [Fact]
        public void GetNextExecution_InvalidType()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();

            task.typeTask = TypeTask.Unsupported;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);
            task.recurrence = 1;

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsFailure);
            Assert.Equal("Type task not supported", result.Error);
        }

        [Fact]
        public void GetNextExecution_ValidationFailure()
        {
            var taskManager = new TaskManager();

            
            var task = new TaskEntity
            {
                typeTask = TypeTask.Once,
                currentDate = DateTimeOffset.UtcNow,
                startDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero),
                eventDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero),
            };

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsFailure);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public void GetNextExecutionOnce_DescriptionFormat()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity
            {
                typeTask = TypeTask.Once,
                currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero),
                startDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero),
                eventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero)
            };

            var result = taskManager.GetNextExecutionOnce(task);

            Assert.True(result.IsSuccess);

            string expectedDescription = $"Occurs once. Schedule will be used on {task.eventDate.Value.Date.ToShortDateString()} at {task.eventDate.Value.ToLocalTime().DateTime.ToShortTimeString()} starting on {task.startDate.Date.ToShortDateString()}";
            Assert.Equal(expectedDescription, result.Value.description);
        }

        [Fact]
        public void GetNextExecutionRecurring_DescriptionFormat()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity
            {
                typeTask = TypeTask.Recurring,
                currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero),
                startDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero),
                recurrence = 2
            };

            var result = taskManager.GetNextExecutionRecurring(task);

            Assert.True(result.IsSuccess);

            string expectedDescription = $"Occurs every {task.recurrence} day/s. Schedule will be used on {result.Value.executionTime.Date.ToShortDateString()} starting on {task.startDate.Date.ToShortDateString()} ";
            Assert.Equal(expectedDescription, result.Value.description);
        }

        [Fact]
        public void GetRecurrentDays_Exactly10Occurrences()
        {
            var taskManager = new TaskManager();

            var startFrom = new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero);
            int recurrenceDays = 1;
            DateTimeOffset? endDate = null; 

            var result = taskManager.GetRecurrentDays(startFrom, recurrenceDays, endDate);

            Assert.Equal(10, result.Count);
            Assert.Contains(startFrom, result);
            Assert.Contains(startFrom.AddDays(9), result);
        }

        [Fact]
        public void GetNextExecutionRecurring_RecurrenceIsOne()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity
            {
                typeTask = TypeTask.Recurring,
                currentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero),
                startDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero),
                recurrence = 1
            };

            var result = taskManager.GetNextExecutionRecurring(task);

            Assert.True(result.IsSuccess);
           
            Assert.Equal(task.currentDate, result.Value.executionTime);
        }

        [Fact]
        public void GetNextExecutionRecurring_CurrentEqualsStart()
        {
            var taskManager = new TaskManager();

            var now = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            var task = new TaskEntity
            {
                typeTask = TypeTask.Recurring,
                currentDate = now,
                startDate = now,
                recurrence = 3
            };

            var result = taskManager.GetNextExecutionRecurring(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(now, result.Value.executionTime);
        }

        
    }

}
