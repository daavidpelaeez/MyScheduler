using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Services;


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
            Assert.Equal(new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero), result.Value.executionTime);
            Assert.Equal("Occurs once. Schedule will be used on 10/10/2025 at 16:30 starting on 09/10/2025", result.Value.description);
        }

        [Fact]
        public void NextExecutionRecurring_StartDateAfterCurrent()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.typeTask = TypeTask.Recurring;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 1;

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero), result.Value.executionTime);
            Assert.Equal("Occurs every 1 day/s. Schedule will be used on 09/10/2025 starting on 09/10/2025 ", result.Value.description);
        }

        [Fact]
        public void NextExecutionRecurring_StartDateBeforeCurrent()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.typeTask = TypeTask.Recurring;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 1;

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero), result.Value.executionTime);
            Assert.Equal("Occurs every 1 day/s. Schedule will be used on 08/10/2025 starting on 05/10/2025 ", result.Value.description);
        }

        [Fact]
        public void GetNextExecutionOnce_WithRecurrence_ShouldFailValidation()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.typeTask = TypeTask.Once;
            task.currentDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 6, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 2;

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsFailure);
            Assert.Contains("once", result.Error.ToLower());
        }

        [Fact]
        public void GetNextExecutionRecurring_RecurrenceZero_ShouldFailValidation()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.typeTask = TypeTask.Recurring;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 0;

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsFailure);
            Assert.Contains("recurrence", result.Error.ToLower());
        }

        [Fact]
        public void GetNextExecutionRecurring_StartDateAfterEndDate_ShouldFailValidation()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.typeTask = TypeTask.Recurring;
            task.currentDate = new DateTimeOffset(2025, 10, 4, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.endDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 1;

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsFailure);
            Assert.Contains("start date", result.Error.ToLower());
        }

        [Fact]
        public void GetNextExecutionRecurring_NoEndDate_ShouldReturnNext()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.typeTask = TypeTask.Recurring;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 2;

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsSuccess, result.Error);
            Assert.Equal(new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero), result.Value.executionTime);
        }

        [Fact]
        public void GetNextExecution_InvalidType()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.typeTask = TypeTask.Unsupported;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsFailure);
            Assert.Equal("Task type not supported", result.Error);
        }

        [Fact]
        public void GetNextExecution_ValidationFailure()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.typeTask = TypeTask.Once;
            task.currentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsFailure);
            Assert.Contains("event date", result.Error.ToLower());
        }

        [Fact]
        public void GetNextExecutionOnce_DescriptionFormat()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.typeTask = TypeTask.Once;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);

            var result = taskManager.GetNextExecutionOnce(task);

            Assert.True(result.IsSuccess);
            Assert.Equal("Occurs once. Schedule will be used on 10/10/2025 at 16:30 starting on 07/10/2025", result.Value.description);
        }

        [Fact]
        public void GetNextExecutionRecurring_DescriptionFormat()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.typeTask = TypeTask.Recurring;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 2;

            var result = taskManager.GetNextExecutionRecurring(task);

            Assert.True(result.IsSuccess);
            Assert.Equal("Occurs every 2 day/s. Schedule will be used on 09/10/2025 starting on 05/10/2025 ", result.Value.description);
        }

        [Fact]
        public void GetRecurrentDays_Exactly100Occurrences()
        {
            var taskManager = new TaskManager();

            var result = taskManager.GetRecurrentDays(
                new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero),
                1,
                null,
                100
            );

            Assert.Equal(100, result.Count);
            Assert.Equal(new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero), result[0]);
            Assert.Equal(new DateTimeOffset(2025, 10, 1, 0, 0, 0, TimeSpan.Zero).AddDays(99), result[99]);
        }

        [Fact]
        public void GetNextExecutionRecurring_RecurrenceIsOne_ShouldReturnToday()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.typeTask = TypeTask.Recurring;
            task.currentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 1;

            var result = taskManager.GetNextExecutionRecurring(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero), result.Value.executionTime);
        }

        [Fact]
        public void GetNextExecutionRecurring_CurrentEqualsStart()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.typeTask = TypeTask.Recurring;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.recurrence = 3;

            var result = taskManager.GetNextExecutionRecurring(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero), result.Value.executionTime);
        }

        [Fact]
        public void GetRecurrentDays_ShouldBreakWhenCurrentExceedsEndDate()
        {
            
            var taskManager = new TaskManager(); 
            DateTimeOffset startFrom = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            int recurrence = 1; 
            DateTimeOffset endDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            int limit = 10; 
           
            var result = taskManager.GetRecurrentDays(startFrom, recurrence, endDate, limit);
            
            Assert.Contains(endDate, result);
            Assert.DoesNotContain(endDate.AddDays(1), result);
            Assert.True(result.Count < limit); 
        }

        [Fact]
        public void GetRecurrentDays_CheckLeap()
        {
            var taskManager = new TaskManager();

            var result = taskManager.GetRecurrentDays(
                new DateTimeOffset(2024, 2, 28, 0, 0, 0, TimeSpan.Zero),
                1,
                null,
                5
            );

            Assert.Equal(5, result.Count);
            Assert.Equal(new DateTimeOffset(2024, 2, 28, 0, 0, 0,TimeSpan.Zero), result[0]);
            Assert.Equal(new DateTimeOffset(2024, 2, 29, 0, 0, 0,TimeSpan.Zero), result[1]);
            Assert.Equal(new DateTimeOffset(2024, 3, 1, 0, 0, 0, TimeSpan.Zero), result[2]);
            Assert.Equal(new DateTimeOffset(2024, 3, 2, 0, 0, 0, TimeSpan.Zero), result[3]);
            Assert.Equal(new DateTimeOffset(2024, 3, 3, 0, 0, 0, TimeSpan.Zero), result[4]);
  
        }

        [Fact]
        public void CheckYearJump()
        {
            var taskManager = new TaskManager();

            var result = taskManager.GetRecurrentDays(
                new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero),
                1,
                null,
                3
             );

            Assert.Equal(3, result.Count);
            Assert.Equal(new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero), result[0]);
            Assert.Equal(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero),   result[1]);
            Assert.Equal(new DateTimeOffset(2026, 1, 2, 0, 0, 0, TimeSpan.Zero),   result[2]);

        }

        [Fact]
        public void EventDateSameAsEndDate()
        {

            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.typeTask = TypeTask.Once;
            task.currentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.startDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);
            task.endDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero), result.Value.executionTime);
            Assert.Equal("Occurs once. Schedule will be used on 10/10/2025 at 16:30 starting on 09/10/2025", result.Value.description);

        }

        [Fact]
        public void EventDateSameAsCurrentDateAndStartDate()
        {

            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.typeTask = TypeTask.Once;
            task.startDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);
            task.eventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);
            task.currentDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsSuccess, result.Error);
            Assert.Equal(new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero), result.Value.executionTime);
            Assert.Equal("Occurs once. Schedule will be used on 10/10/2025 at 16:30 starting on 10/10/2025", result.Value.description);

        }



    }
}
