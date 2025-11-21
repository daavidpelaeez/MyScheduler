
using MyScheduler.Application.ScheduleCalculators.Monthly;
using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Enums;

namespace MyScheduler.Monthly
{
    public class MonthlyDayDailyOnceAndRangeCalculatorTests
    {

        [Fact]
        public void MonthlyDayDailyOnceOutput_ReturnsSuccess_WhenDateExists()
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
            var calc = new MonthlyDayDailyOnceOutput();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsSuccess);
            
        }

        [Fact]
        public void MonthlyDayDailyRangeOutput_ReturnsSuccess_WhenDateExists()
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
            var calc = new MonthlyDayDailyRangeOutput();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsSuccess);
            
        }

        [Fact]
        public void MonthlyDayDailyOnceOutput_ReturnsFailure_WhenNoDateExists()
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
            var calc = new MonthlyDayDailyOnceOutput();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsFailure);
            Assert.Equal("No next execution found", result.Error);
        }

        [Fact]
        public void MonthlyDayDailyRangeOutput_ReturnsFailure_WhenNoDateExists()
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
            var calc = new MonthlyDayDailyRangeOutput();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsFailure);
            Assert.Equal("No next execution found", result.Error);
        }
    }
}
