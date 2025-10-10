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
            task.TypeTask = TypeTask.Once;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);

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
            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 1;

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
            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 1;

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
            task.TypeTask = TypeTask.Once;
            task.CurrentDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 6, 0, 0, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 2;

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsFailure);
            Assert.Contains("once", result.Error.ToLower());
        }

        [Fact]
        public void GetNextExecutionRecurring_RecurrenceZero_ShouldFailValidation()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 0;

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsFailure);
            Assert.Contains("recurring tasks must have a recurrence", result.Error.ToLower());
        }

        [Fact]
        public void GetNextExecutionRecurring_StartDateAfterEndDate_ShouldFailValidation()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 4, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.EndDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 1;

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsFailure);
            Assert.Contains("start date", result.Error.ToLower());
        }

        [Fact]
        public void GetNextExecutionRecurring_NoEndDate_ShouldReturnNext()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 2;

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsSuccess, result.Error);
            Assert.Equal(new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero), result.Value.executionTime);
        }

        [Fact]
        public void GetNextExecution_InvalidType()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.TypeTask = TypeTask.Unsupported;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsFailure);
            Assert.Equal("Task type not supported", result.Error);
        }

        [Fact]
        public void GetNextExecution_ValidationFailure()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.TypeTask = TypeTask.Once;
            task.CurrentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsFailure);
            Assert.Contains("event date", result.Error.ToLower());
        }

        [Fact]
        public void GetNextExecutionOnce_DescriptionFormat()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.TypeTask = TypeTask.Once;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);

            var result = taskManager.GetNextExecutionOnce(task);

            Assert.True(result.IsSuccess);
            Assert.Equal("Occurs once. Schedule will be used on 10/10/2025 at 16:30 starting on 07/10/2025", result.Value.description);
        }

        [Fact]
        public void GetNextExecutionRecurring_DescriptionFormat()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 2;

            var result = taskManager.GetNextExecutionRecurring(task);

            Assert.True(result.IsSuccess);
            Assert.Equal("Occurs every 2 day/s. Schedule will be used on 09/10/2025 starting on 05/10/2025 ", result.Value.description);
        }
        
        [Fact]
        public void GetRecurrentDays_Exactly100Occurrences()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 2;

            var result = taskManager.GetRecurrentDays(
                task,
                10
            );

            Assert.Equal(10, result.Count);
            Assert.Equal(new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero), result[0]);
            Assert.Equal(new DateTimeOffset(2025, 10, 27, 0, 0, 0, TimeSpan.Zero), result[9]);
        }

        [Fact]
        public void GetNextExecutionRecurring_RecurrenceIsOne_ShouldReturnToday()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 1;

            var result = taskManager.GetNextExecutionRecurring(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero), result.Value.executionTime);
        }

        [Fact]
        public void GetNextExecutionRecurring_CurrentEqualsStart()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 3;

            var result = taskManager.GetNextExecutionRecurring(task);

            Assert.True(result.IsSuccess);
            Assert.Equal(new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero), result.Value.executionTime);
        }

        [Fact]
        public void GetRecurrentDays_CheckLeap()
        {
            var taskManager = new TaskManager();


            var task = new TaskEntity();
            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2024, 2, 28, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2024, 2, 28, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 3;

            var result = taskManager.GetRecurrentDays(
                task,
                3
            );

            Assert.Equal(3, result.Count);
            Assert.Equal(new DateTimeOffset(2024, 2, 28, 0, 0, 0,TimeSpan.Zero), result[0]);
            Assert.Equal(new DateTimeOffset(2024, 3, 2, 0, 0, 0,TimeSpan.Zero),  result[1]);
            Assert.Equal(new DateTimeOffset(2024, 3, 5, 0, 0, 0, TimeSpan.Zero), result[2]);
            
        }

        [Fact]
        public void CheckYearJump()
        {
            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.TypeTask = TypeTask.Recurring;
            task.CurrentDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            task.Recurrence = 3;

            var result = taskManager.GetRecurrentDays(
                task,
                3
             );

            Assert.Equal(3, result.Count);
            Assert.Equal(new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero), result[0]);
            Assert.Equal(new DateTimeOffset(2026, 1, 3, 0, 0, 0, TimeSpan.Zero),   result[1]);
            Assert.Equal(new DateTimeOffset(2026, 1, 6, 0, 0, 0, TimeSpan.Zero),   result[2]);

        }

        [Fact]
        public void EventDateSameAsEndDate()
        {

            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.TypeTask = TypeTask.Once;
            task.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);
            task.EndDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);

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
            task.TypeTask = TypeTask.Once;
            task.StartDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);
            task.CurrentDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsSuccess, result.Error);
            Assert.Equal(new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero), result.Value.executionTime);
            Assert.Equal("Occurs once. Schedule will be used on 10/10/2025 at 16:30 starting on 10/10/2025", result.Value.description);

        }

        [Fact]
        public void EndDateSameAsStartDate()
        {

            var taskManager = new TaskManager();

            var task = new TaskEntity();
            task.TypeTask = TypeTask.Once;
            task.CurrentDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);
            task.StartDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);
            task.EventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);
           
            task.EndDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);

            var result = taskManager.GetNextExecution(task);

            Assert.True(result.IsSuccess, result.Error);
            Assert.Equal(new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero), result.Value.executionTime);
            Assert.Equal("Occurs once. Schedule will be used on 10/10/2025 at 16:30 starting on 10/10/2025", result.Value.description);

        }

        



    }
}
