using System;
using Xunit;
using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.ScheduleCalculators;

namespace MyScheduler.Monthly
{
    public class MonthlyTheDailyOnceAndRangeCalculatorTests
    {
        [Fact]
        public void MonthlyTheDailyOnceCalculator_ReturnsFailure_WhenNoDates()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                MonthlyTheOrder = MonthlyTheOrder.First,
                MonthlyTheDayOfWeek = MonthlyDayOfWeek.Monday,
                MonthlyTheRecurrence = 1,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.FromHours(8),
                StartDate = new DateTimeOffset(2025, 2, 4, 0, 0, 0, TimeSpan.Zero), // Martes
                EndDate = new DateTimeOffset(2025, 2, 4, 0, 0, 0, TimeSpan.Zero)
            };
            var calc = new MonthlyTheDailyOnceCalculator();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsFailure);
            Assert.Contains("No next execution found", result.Error);
        }

        [Fact]
        public void MonthlyTheDailyOnceCalculator_ReturnsSuccess_WhenDateExists()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                MonthlyTheOrder = MonthlyTheOrder.First,
                MonthlyTheDayOfWeek = MonthlyDayOfWeek.Monday,
                MonthlyTheRecurrence = 1,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.FromHours(8),
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 1, 31, 0, 0, 0, TimeSpan.Zero)
            };
            var calc = new MonthlyTheDailyOnceCalculator();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("", result.Error);
        }

        [Fact]
        public void MonthlyTheDailyRangeCalculator_ReturnsFailure_WhenNoDates()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                MonthlyTheOrder = MonthlyTheOrder.First,
                MonthlyTheDayOfWeek = MonthlyDayOfWeek.Monday,
                MonthlyTheRecurrence = 1,
                DailyFrequencyRangeCheckbox = true,
                DailyStartTime = TimeSpan.FromHours(8),
                DailyEndTime = TimeSpan.FromHours(10),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                StartDate = new DateTimeOffset(2025, 2, 4, 0, 0, 0, TimeSpan.Zero), // Martes
                EndDate = new DateTimeOffset(2025, 2, 4, 0, 0, 0, TimeSpan.Zero)
            };
            var calc = new MonthlyTheDailyRangeCalculator();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsFailure);
            Assert.Contains("No next execution found", result.Error);
        }

        [Fact]
        public void MonthlyTheDailyRangeCalculator_ReturnsSuccess_WhenDateExists()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                MonthlyTheOrder = MonthlyTheOrder.First,
                MonthlyTheDayOfWeek = MonthlyDayOfWeek.Monday,
                MonthlyTheRecurrence = 1,
                DailyFrequencyRangeCheckbox = true,
                DailyStartTime = TimeSpan.FromHours(8),
                DailyEndTime = TimeSpan.FromHours(10),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 1, 31, 0, 0, 0, TimeSpan.Zero)
            };
            var calc = new MonthlyTheDailyRangeCalculator();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("", result.Error);
        }
    }
}
