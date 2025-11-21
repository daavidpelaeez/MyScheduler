
using MyScheduler.Application.Services;
using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Enums;


namespace MyScheduler.Services
{
    public class ScheduleManagerTests
    {
        [Fact]
        public void GetNextExecution_ShouldReturnFailure_WhenValidationFails()
        {
            var manager = new ScheduleManager();
            var schedule = new ScheduleEntity();
            var result = manager.GetOutput(schedule, null);
            Assert.True(result.IsFailure);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public void GetNextExecution_ShouldReturnOnceResult_WhenScheduleTypeIsOnce()
        {
            var manager = new ScheduleManager();
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Once,
                Enabled = true,
                StartDate = DateTimeOffset.Now.AddDays(1),
                EndDate = DateTimeOffset.Now.AddDays(2),
                CurrentDate = DateTimeOffset.Now,
                OnceTypeDateExecution = DateTimeOffset.Now.AddDays(1)
            };
            var result = manager.GetOutput(schedule, null);
            Assert.False(result.IsFailure);

        }

        [Fact]
        public void GetNextExecution_ShouldReturnRecurringDailyOnceResult()
        {
            var manager = new ScheduleManager();
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Daily,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.FromHours(8),
                Enabled = true,
                StartDate = DateTimeOffset.Now.AddDays(1),
                EndDate = DateTimeOffset.Now.AddDays(10),
                Recurrence = 1,
                CurrentDate = DateTimeOffset.Now
            };
            schedule.DailyFrequencyRangeCheckbox = false;
            var result = manager.GetOutput(schedule, 1);
            Assert.False(result.IsFailure);
        }

        [Fact]
        public void GetNextExecution_ShouldReturnRecurringDailyRangeResult()
        {
            var manager = new ScheduleManager();
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Daily,
                DailyFrequencyRangeCheckbox = true,
                DailyStartTime = TimeSpan.FromHours(8),
                DailyEndTime = TimeSpan.FromHours(10),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                Enabled = true,
                StartDate = DateTimeOffset.Now.AddDays(1),
                EndDate = DateTimeOffset.Now.AddDays(10),
                Recurrence = 1,
                CurrentDate = DateTimeOffset.Now
            };
            var result = manager.GetOutput(schedule, 1);
            Assert.False(result.IsFailure);

        }

        [Fact]
        public void GetNextExecution_ShouldReturnRecurringWeeklyOnceResult()
        {
            var manager = new ScheduleManager();
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.FromHours(8),
                Enabled = true,
                StartDate = DateTimeOffset.Now.AddDays(1),
                EndDate = DateTimeOffset.Now.AddDays(10),
                Recurrence = 1,
                WeeklyRecurrence = 1,
                CurrentDate = DateTimeOffset.Now,
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday }
            };
            var result = manager.GetOutput(schedule, 1);
            Assert.False(result.IsFailure);
        }

        [Fact]
        public void GetNextExecution_ShouldReturnRecurringWeeklyRangeResult()
        {
            var manager = new ScheduleManager();
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                DailyStartTime = TimeSpan.FromHours(8),
                DailyEndTime = TimeSpan.FromHours(10),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                Enabled = true,
                StartDate = DateTimeOffset.Now.AddDays(1),
                EndDate = DateTimeOffset.Now.AddDays(10),
                Recurrence = 1,
                WeeklyRecurrence = 1,
                CurrentDate = DateTimeOffset.Now,
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday }
            };
            var result = manager.GetOutput(schedule, 1);
            Assert.False(result.IsFailure);

        }

        [Fact]
        public void GetNextExecution_ShouldReturnMonthlyDayDailyOnceResult()
        {
            var manager = new ScheduleManager();
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                DailyFrequencyOnceCheckbox = true,
                MonthlyDayNumber = 1,
                MonthlyDayRecurrence = 1,
                DailyOnceExecutionTime = TimeSpan.FromHours(8),
                Enabled = true,
                StartDate = DateTimeOffset.Now.AddDays(1)
            };
            var result = manager.GetOutput(schedule, 1);
            Assert.False(result.IsFailure);
        }

        [Fact]
        public void GetNextExecution_ShouldReturnMonthlyDayDailyRangeResult()
        {
            var manager = new ScheduleManager();
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                DailyFrequencyRangeCheckbox = true,
                MonthlyDayNumber = 1,
                MonthlyDayRecurrence = 1,
                DailyStartTime = TimeSpan.FromHours(8),
                DailyEndTime = TimeSpan.FromHours(10),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                Enabled = true,
                StartDate = DateTimeOffset.Now.AddDays(1)
            };
            var result = manager.GetOutput(schedule, 1);
            Assert.False(result.IsFailure);

        }

        [Fact]
        public void GetNextExecution_ShouldReturnMonthlyTheDailyOnceResult()
        {
            var manager = new ScheduleManager();
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Monthly,
                MonthlyFrequencyTheCheckbox = true,
                DailyFrequencyOnceCheckbox = true,
                MonthlyTheOrder = MonthlyTheOrder.First,
                MonthlyTheDayOfWeek = MonthlyDayOfWeek.Monday,
                MonthlyTheRecurrence = 1,
                DailyOnceExecutionTime = TimeSpan.FromHours(8),
                Enabled = true,
                StartDate = DateTimeOffset.Now.AddDays(1)
            };
            var result = manager.GetOutput(schedule, 1);
            Assert.False(result.IsFailure);

        }

        [Fact]
        public void GetNextExecution_ShouldReturnMonthlyTheDailyRangeResult()
        {
            var manager = new ScheduleManager();
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Monthly,
                MonthlyFrequencyTheCheckbox = true,
                DailyFrequencyRangeCheckbox = true,
                MonthlyTheOrder = MonthlyTheOrder.First,
                MonthlyTheDayOfWeek = MonthlyDayOfWeek.Monday,
                MonthlyTheRecurrence = 1,
                DailyStartTime = TimeSpan.FromHours(8),
                DailyEndTime = TimeSpan.FromHours(10),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                Enabled = true,
                StartDate = DateTimeOffset.Now.AddDays(1)
            };
            var result = manager.GetOutput(schedule, 1);
            Assert.False(result.IsFailure);

        }

        [Fact]
        public void GetNextExecution_ShouldReturnMonthlyDayCalculatorResult()
        {
            var manager = new ScheduleManager();
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                MonthlyDayNumber = 1,
                MonthlyDayRecurrence = 1,
                Enabled = true,
                StartDate = DateTimeOffset.Now.AddDays(1)
            };
            var result = manager.GetOutput(schedule, 1);
            Assert.False(result.IsFailure);

        }

        [Fact]
        public void GetNextExecution_ShouldReturnMonthlyTheOutputResult()
        {
            var manager = new ScheduleManager();
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Monthly,
                MonthlyFrequencyTheCheckbox = true,
                MonthlyTheOrder = MonthlyTheOrder.First,
                MonthlyTheDayOfWeek = MonthlyDayOfWeek.Monday,
                MonthlyTheRecurrence = 1,
                Enabled = true,
                StartDate = DateTimeOffset.Now.AddDays(1)
            };
            var result = manager.GetOutput(schedule, 1);
            Assert.False(result.IsFailure);
        }

        [Fact]
        public void GetNextExecution_ShouldReturnFailure_WhenScheduleTypeInvalid()
        {
            var manager = new ScheduleManager();
            var schedule = new ScheduleEntity
            {
                ScheduleType = (ScheduleType)999,
                Enabled = true,
                StartDate = DateTimeOffset.Now.AddDays(1)
            };
            var result = manager.GetOutput(schedule, 1);
            Assert.True(result.IsFailure);
            Assert.Contains("Schedule type should be once or recurring", result.Error);
        }

        [Fact]
        public void GetNextExecution_ShouldReturnFailure_WhenOccursInvalid()
        {
            var manager = new ScheduleManager();
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = (Occurs)999,
                Enabled = true,
                StartDate = DateTimeOffset.Now.AddDays(1)
            };
            var result = manager.GetOutput(schedule, 1);
            Assert.True(result.IsFailure);
            Assert.Contains("daily frequency once", result.Error);
        }
    }
}
