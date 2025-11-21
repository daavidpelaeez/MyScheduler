
using System.Text;
using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Enums;
using MyScheduler.Domain.Validators;

namespace MyScheduler.OnceTests
{
    public class OnceValidatorTests
    {
        [Fact]
        public void Validate_AllBranches_AddsAllErrors()
        {
            var schedule = new ScheduleEntity
            {
                OnceTypeDateExecution = null, 
                Recurrence = 1, 
                DailyFrequencyRangeCheckbox = true, 
                DailyFrequencyOnceCheckbox = true, 
                WeeklyRecurrence = 1, // > 0
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday }, 
                DailyOnceExecutionTime = TimeSpan.FromHours(8), // != null
                TimeUnit = TimeUnit.Hours, // != null
                TimeUnitNumberOf = 1, // > 0
                DailyStartTime = TimeSpan.FromHours(9), // != null
                DailyEndTime = TimeSpan.FromHours(10) // != null
            };
            var errors = new StringBuilder();
            OnceValidator.Validate(schedule, errors);
            var errorText = errors.ToString();
            Assert.Contains("OnceTypeDateExecution is required for OneTime tasks.", errorText);
            Assert.Contains("OneTime tasks cannot have a recurrence.", errorText);
            Assert.Contains("OneTime tasks cannot have daily frequency", errorText);
            Assert.Contains("OneTime tasks cannot have weekly configuration", errorText);
            Assert.Contains("OneTime tasks cannot have execution once on daily frequency", errorText);
            Assert.Contains("OneTime tasks cannot have daily frequency configuration", errorText);
        }

        [Fact]
        public void Validate_NoErrors_WhenValidOnce()
        {
            var schedule = new ScheduleEntity
            {
                OnceTypeDateExecution = DateTimeOffset.Now,
                Recurrence = 0,
                DailyFrequencyRangeCheckbox = false,
                DailyFrequencyOnceCheckbox = false,
                WeeklyRecurrence = 0,
                DaysOfWeek = null,
                DailyOnceExecutionTime = null,
                TimeUnit = null,
                TimeUnitNumberOf = 0,
                DailyStartTime = null,
                DailyEndTime = null
            };
            var errors = new StringBuilder();
            OnceValidator.Validate(schedule, errors);
            Assert.Equal(string.Empty, errors.ToString());
        }
    }
}
