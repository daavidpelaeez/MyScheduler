using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Services;
using Xunit;

namespace MyScheduler
{
    public class OnceRecurringCalculator
    {
        [Fact]
        public void GetNextExecutionOnce_ShouldReturnCorrectOutput()
        {
            var taskManager = new ScheduleManager();

            var schedule = new ScheduleEntity
            {
                Type = Enums.Type.Once,
                CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero),
                EventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero)
            };

            var result = taskManager.GetNextExecution(schedule, 10);

            Assert.True(result.IsSuccess);
            Assert.Equal(new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero), result.Value.ExecutionTime);
            Assert.Equal("Occurs once. Schedule on 10/10/2025 at 14:30, starting 09/10/2025", result.Value.Description);
        }

        [Fact]
        public void NextExecutionRecurring_StartDateAfterCurrent()
        {
            var taskManager = new ScheduleManager();

            var schedule = new ScheduleEntity
            {
                Type = Enums.Type.Recurring,
                CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero),
                Recurrence = 1
            };

            var result = taskManager.GetNextExecution(schedule, 10);

            Assert.True(result.IsSuccess);
            Assert.Equal(new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero), result.Value.ExecutionTime);
            Assert.Equal("Occurs every 1 day(s). Next on 09/10/2025, starting 09/10/2025", result.Value.Description);
        }

        [Fact]
        public void NextExecutionRecurring_StartDateBeforeCurrent()
        {
            var taskManager = new ScheduleManager();

            var schedule = new ScheduleEntity
            {
                Type = Enums.Type.Recurring,
                CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero),
                
                Recurrence = 1
            };

            var result = taskManager.GetNextExecution(schedule, 10);

            Assert.True(result.IsSuccess);
            Assert.Equal(new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero), result.Value.ExecutionTime);
            Assert.Equal("Occurs every 1 day(s). Next on 08/10/2025, starting 05/10/2025", result.Value.Description);
        }

        [Fact]
        public void GetNextExecutionOnce_WithRecurrence_ShouldFailValidation()
        {
            var taskManager = new ScheduleManager();

            var schedule = new ScheduleEntity
            {
                Type = Enums.Type.Once,
                CurrentDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 6, 0, 0, 0, TimeSpan.Zero),
                EventDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero),
                Recurrence = 2
            };

            var result = taskManager.GetNextExecution(schedule, null);

            Assert.True(result.IsFailure);
            Assert.Contains("once", result.Error.ToLower());
        }

        [Fact]
        public void GetNextExecutionRecurring_RecurrenceZero_ShouldFailValidation()
        {
            var taskManager = new ScheduleManager();

            var schedule = new ScheduleEntity
            {
                Type = Enums.Type.Recurring,
                CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero),
                Recurrence = 0
            };

            var result = taskManager.GetNextExecution(schedule, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("recurring tasks must have a recurrence", result.Error.ToLower());
        }

        [Fact]
        public void GetNextExecutionRecurring_StartDateAfterEndDate_ShouldFailValidation()
        {
            var taskManager = new ScheduleManager();

            var schedule = new ScheduleEntity
            {
                Type = Enums.Type.Recurring,
                CurrentDate = new DateTimeOffset(2025, 10, 4, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero),
                Recurrence = 1
            };

            var result = taskManager.GetNextExecution(schedule, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("start date", result.Error.ToLower());
        }

        [Fact]
        public void GetNextExecutionRecurring_NoEndDate_ShouldReturnNext()
        {
            var taskManager = new ScheduleManager();

            var schedule = new ScheduleEntity
            {
                Type = Enums.Type.Recurring,
                CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero),
                Recurrence = 2
            };

            var result = taskManager.GetNextExecution(schedule, 10);

            Assert.True(result.IsSuccess, result.Error);
            Assert.Equal(new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero), result.Value.ExecutionTime);
        }

        [Fact]
        public void GetNextExecution_ValidationFailure()
        {
            var taskManager = new ScheduleManager();

            var schedule = new ScheduleEntity
            {
                Type = Enums.Type.Once,
                CurrentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero),
                EventDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero)
            };

            var result = taskManager.GetNextExecution(schedule, null);

            Assert.True(result.IsFailure);
            Assert.Contains("event date", result.Error.ToLower());
        }

        [Fact]
        public void GetNextExecutionOnce_DescriptionFormat()
        {
            var taskManager = new ScheduleManager();

            var schedule = new ScheduleEntity
            {
                Type = Enums.Type.Once,
                CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero),
                EventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero)
            };

            var result = taskManager.GetNextExecution(schedule, 10);

            Assert.True(result.IsSuccess);
            Assert.Equal("Occurs once. Schedule on 10/10/2025 at 14:30, starting 07/10/2025", result.Value.Description);
        }

        [Fact]
        public void GetNextExecutionRecurring_DescriptionFormat()
        {
            var taskManager = new ScheduleManager();

            var schedule = new ScheduleEntity
            {
                Type = Enums.Type.Recurring,
                CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero),
                Recurrence = 2
            };

            var result = taskManager.GetNextExecution(schedule, 10);

            Assert.True(result.IsSuccess);
            Assert.Equal("Occurs every 2 day(s). Next on 09/10/2025, starting 05/10/2025", result.Value.Description);
        }

        [Fact]
        public void GetNextExecutionRecurring_RecurrenceIsOne_ShouldReturnToday()
        {
            var taskManager = new ScheduleManager();

            var schedule = new ScheduleEntity
            {
                Type = Enums.Type.Recurring,
                CurrentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero),
                Recurrence = 1
            };

            var result = taskManager.GetNextExecution(schedule, 10);

            Assert.True(result.IsSuccess);
            Assert.Equal(new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero), result.Value.ExecutionTime);
        }

        [Fact]
        public void GetNextExecutionRecurring_CurrentEqualsStart()
        {
            var taskManager = new ScheduleManager();

            var schedule = new ScheduleEntity
            {
                Type = Enums.Type.Recurring,
                CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero),
                Recurrence = 3
            };

            var result = taskManager.GetNextExecution(schedule, 10);

            Assert.True(result.IsSuccess);
            Assert.Equal(new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero), result.Value.ExecutionTime);
        }

        [Fact]
        public void EventDateSameAsEndDate()
        {
            var taskManager = new ScheduleManager();

            var schedule = new ScheduleEntity
            {
                Type = Enums.Type.Once,
                CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero),
                EventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero)
            };

            var result = taskManager.GetNextExecution(schedule,10);

            Assert.True(result.IsSuccess);
            Assert.Equal(new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero), result.Value.ExecutionTime);
            Assert.Equal("Occurs once. Schedule on 10/10/2025 at 14:30, starting 09/10/2025", result.Value.Description);
        }

        [Fact]
        public void EventDateSameAsCurrentDateAndStartDate()
        {
            var taskManager = new ScheduleManager();

            var schedule = new ScheduleEntity
            {
                Type = Enums.Type.Once,
                StartDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero),
                EventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero)
            };

            var result = taskManager.GetNextExecution(schedule, 10);

            Assert.True(result.IsSuccess, result.Error);
            Assert.Equal(new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero), result.Value.ExecutionTime);
            Assert.Contains("Occurs once. Schedule on 10/10/2025 at 14:30", result.Value.Description);
        }

        [Fact]
        public void EndDateSameAsStartDate()
        {
            var taskManager = new ScheduleManager();

            var schedule = new ScheduleEntity
            {
                Type = Enums.Type.Once,
                CurrentDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero),
                EventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero)
            };

            var result = taskManager.GetNextExecution(schedule, null);

            Assert.True(result.IsSuccess, result.Error);
            Assert.Equal(new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero), result.Value.ExecutionTime);
            Assert.Equal("Occurs once. Schedule on 10/10/2025 at 14:30, starting 10/10/2025", result.Value.Description);

        }
    }
}
