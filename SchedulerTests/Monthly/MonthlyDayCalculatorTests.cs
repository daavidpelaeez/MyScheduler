
using MyScheduler.Application.ScheduleCalculators.Monthly;
using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Enums;

namespace MyScheduler.Monthly
{
    public class MonthlyDayCalculatorTests
    {
        [Fact]
        public void GetOutput_ReturnsFailure_WhenNoOccurrences()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                MonthlyDayNumber = 15,
                MonthlyDayRecurrence = 1,
                StartDate = new DateTimeOffset(2025, 1, 16, 0, 0, 0, TimeSpan.Zero), 
                EndDate = new DateTimeOffset(2025, 1, 20, 0, 0, 0, TimeSpan.Zero)
            };
            var calc = new MonthlyDayOutput();
            var result = calc.GetOutput(schedule, 1);
            Assert.True(result.IsFailure);
            Assert.Contains("No next execution found", result.Error);
        }

        
    }
}
