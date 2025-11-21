
using MyScheduler.Application.ScheduleCalculators.Monthly;
using MyScheduler.Application.ScheduleOutputs.Monthly;
using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Enums;

namespace MyScheduler.Monthly
{
    public class MonthlyTheDailyOnceAndRangeCalculatorTests
    {
        [Fact]
        public void MonthlyTheDailyOnceOutput_ReturnsFailure_WhenNoDates()
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
            var calc = new MonthlyTheDailyOnceOutput();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsFailure);
            Assert.Contains("No next execution found", result.Error);
        }

        [Fact]
        public void MonthlyTheDailyOnceOutput_ReturnsSuccess_WhenDateExists()
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
            var calc = new MonthlyTheDailyOnceOutput();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsSuccess);

            Assert.Equal("", result.Error);
        }

        [Fact]
        public void MonthlyTheDailyRangeOutput_ReturnsFailure_WhenNoDates()
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
            var calc = new MonthlyTheDailyRangeOutput();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsFailure);
            Assert.Contains("No next execution found", result.Error);
        }

        [Fact]
        public void MonthlyTheDailyRangeOutput_ReturnsSuccess_WhenDateExists()
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
            var calc = new MonthlyTheDailyRangeOutput();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsSuccess);
            Assert.Equal("", result.Error);
        }
    }
}
