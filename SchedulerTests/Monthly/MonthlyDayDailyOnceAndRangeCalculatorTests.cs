using System;
using Xunit;
using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.ScheduleCalculators;

namespace MyScheduler.Monthly
{
    public class MonthlyDayDailyOnceAndRangeCalculatorTests
    {

        [Fact]
        public void MonthlyDayDailyOnceCalculator_ReturnsSuccess_WhenDateExists()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                MonthlyDayNumber = 10,
                MonthlyDayRecurrence = 1,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.FromHours(8),
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 1, 31, 0, 0, 0, TimeSpan.Zero)
            };
            var calc = new MonthlyDayDailyOnceCalculator();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            
        }

        [Fact]
        public void MonthlyDayDailyRangeCalculator_ReturnsSuccess_WhenDateExists()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                MonthlyDayNumber = 10,
                MonthlyDayRecurrence = 1,
                DailyFrequencyRangeCheckbox = true,
                DailyStartTime = TimeSpan.FromHours(8),
                DailyEndTime = TimeSpan.FromHours(10),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 1, 31, 0, 0, 0, TimeSpan.Zero)
            };
            var calc = new MonthlyDayDailyRangeCalculator();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            
        }

        [Fact]
        public void MonthlyDayDailyOnceCalculator_ReturnsFailure_WhenNoDateExists()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                MonthlyDayNumber = 32, // Día inválido para cualquier mes
                MonthlyDayRecurrence = 1,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.FromHours(8),
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 1, 31, 0, 0, 0, TimeSpan.Zero)
            };
            var calc = new MonthlyDayDailyOnceCalculator();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsFailure);
            Assert.Equal("No next execution found", result.Error);
        }

        [Fact]
        public void MonthlyDayDailyRangeCalculator_ReturnsFailure_WhenNoDateExists()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                MonthlyDayNumber = 32, // Día inválido para cualquier mes
                MonthlyDayRecurrence = 1,
                DailyFrequencyRangeCheckbox = true,
                DailyStartTime = TimeSpan.FromHours(8),
                DailyEndTime = TimeSpan.FromHours(10),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 1, 31, 0, 0, 0, TimeSpan.Zero)
            };
            var calc = new MonthlyDayDailyRangeCalculator();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsFailure);
            Assert.Equal("No next execution found", result.Error);
        }
    }
}
