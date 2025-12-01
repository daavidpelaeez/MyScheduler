
using MyScheduler.Application.ScheduleCalculators;
using MyScheduler.Application.ScheduleCalculators.Monthly;
using MyScheduler.Application.ScheduleCalculators.Once;
using MyScheduler.Application.ScheduleOutputs.Monthly;
using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Enums;

namespace MyScheduler.Calculators
{
    public class CalculatorsTests
    {
        [Fact]
        public void OneTimeOutput_ReturnsSuccess_WhenDateExists()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Once,
                OnceTypeDateExecution = DateTimeOffset.Now.AddDays(1)
            };
            var calc = new OneTimeOutput();
            var result = calc.GetOnceOutput(schedule);
            Assert.True(result.IsSuccess);
            Assert.Equal("", result.Error);
        }

        [Fact]
        public void OneTimeOutput_ReturnsFailure_WhenNoDate()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Once
            };
            var calc = new OneTimeOutput();
            var result = calc.GetOnceOutput(schedule);
            Assert.True(result.IsFailure);
            Assert.Contains("No next execution found", result.Error);
        }


        [Fact]
        public void MonthlyTheOutput_ReturnsFailure_WhenNoDate()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Monthly,
                MonthlyFrequencyTheCheckbox = true,
                MonthlyTheOrder = MonthlyTheOrder.First,
                MonthlyTheDayOfWeek = MonthlyDayOfWeek.Monday,
                MonthlyTheRecurrence = 1,
                StartDate = DateTimeOffset.Now.AddYears(1), 
                EndDate = DateTimeOffset.Now,
                Enabled = true,
                MonthlyDayNumber = 0,
                MonthlyDayRecurrence = 0,
                DailyFrequencyOnceCheckbox = false,
                DailyFrequencyRangeCheckbox = false
            };
            var calc = new MonthlyTheOutput();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsFailure);
            Assert.Contains("No next execution found", result.Error);
        }

        [Fact]
        public void MonthlyTheDailyOnceOutput_ReturnsFailure_WhenNoDate()
        {
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
                StartDate = DateTimeOffset.Now.AddYears(1),
                EndDate = DateTimeOffset.Now,
                Enabled = true,
                MonthlyDayNumber = 0,
                MonthlyDayRecurrence = 0,
                DailyFrequencyRangeCheckbox = false
            };
            var calc = new MonthlyTheDailyOnceOutput();
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
                StartDate = new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, 28, 0, 0, 0, TimeSpan.Zero),
                Enabled = true,
                MonthlyDayNumber = 0,
                MonthlyDayRecurrence = 0,
                DailyFrequencyOnceCheckbox = false
            };
            var calc = new MonthlyTheDailyRangeOutput();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsSuccess);
            Assert.Equal("", result.Error);
        }

        [Fact]
        public void MonthlyTheDailyRangeOutput_ReturnsFailure_WhenNoDate()
        {
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
                StartDate = DateTimeOffset.Now.AddYears(1),
                EndDate = DateTimeOffset.Now,
                Enabled = true,
                MonthlyDayNumber = 0,
                MonthlyDayRecurrence = 0,
                DailyFrequencyOnceCheckbox = false
            };
            var calc = new MonthlyTheDailyRangeOutput();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsFailure);
            Assert.Contains("No next execution found", result.Error);
        }

        [Fact]
        public void RecurringDailyRangeCalculator_ReturnsSuccess_WhenDatesExist()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Daily,
                DailyFrequencyRangeCheckbox = true,
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                DailyStartTime = TimeSpan.FromHours(8),
                DailyEndTime = TimeSpan.FromHours(10),
                StartDate = new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, 2, 0, 0, 0, TimeSpan.Zero),
                Recurrence = 1
            };
            var calc = new RecurringDailyRangeOutput();
            var result = calc.GetOutput(schedule, 2);
            Assert.True(result.IsSuccess);

        }

        [Fact]
        public void RecurringDailyRangeCalculator_ReturnsFailure_WhenNoDates()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Daily,
                DailyFrequencyRangeCheckbox = true,
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                DailyStartTime = TimeSpan.FromHours(8),
                DailyEndTime = TimeSpan.FromHours(10),
                StartDate = new DateTimeOffset(2024, 6, 10, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2024, 6, 9, 0, 0, 0, TimeSpan.Zero), // EndDate anterior a StartDate
                Recurrence = 1
            };
            var calc = new RecurringDailyRangeOutput();
            var result = calc.GetOutput(schedule, 2);
            Assert.True(result.IsFailure);
            Assert.Equal("No next execution found", result.Error);
        }
    }
}
