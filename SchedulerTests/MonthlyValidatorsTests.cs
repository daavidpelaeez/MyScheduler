using System;
using System.Text;
using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Validators;
using Xunit;

namespace MyScheduler
{
    public class MonthlyValidatorsTests
    {
        [Fact]
        public void ShouldPass_When_MonthlyDay_Valid()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 5;
            schedule.MonthlyDayRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.True(errors.Length == 0);
        }

        [Fact]
        public void ShouldFail_When_BothDayAndTheChecked()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.Contains("You cannot set both Day and The options on monthly configuration", errors.ToString());
        }

        [Fact]
        public void ShouldFail_When_NeitherDayNorTheChecked()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.Contains("You need to check at least one option (Day or The) for monthly configuration", errors.ToString());
        }

        [Fact]
        public void ShouldFail_When_MonthlyDayNumberOutOfRange()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 0; // invalid
            schedule.MonthlyDayRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.Contains("Monthly day number must be between 1 and 31", errors.ToString());
        }

        [Fact]
        public void ShouldFail_When_MonthlyDayRecurrenceIsZero()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 5;
            schedule.MonthlyDayRecurrence = 0; // invalid
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.Contains("Monthly day recurrence must be greater than 0", errors.ToString());
        }

        [Fact]
        public void ShouldFail_When_TheOrderSetWithDay()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 10;
            schedule.MonthlyDayRecurrence = 1;
            schedule.MonthlyTheOrder = MonthlyTheOrder.First;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.Contains("You cannot set monthly order (The Order) when using Day configuration", errors.ToString());
        }

        [Fact]
        public void ShouldFail_When_MonthlyTheDayOfWeekSetWithDay()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 10;
            schedule.MonthlyDayRecurrence = 1;
            schedule.MonthlyTheDayOfWeek = MonthlyDayOfWeek.Monday;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.Contains("You cannot set monthly day of week when using Day configuration", errors.ToString());
        }

        [Fact]
        public void ShouldFail_When_TheRecurrenceSetWithDay()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 10;
            schedule.MonthlyDayRecurrence = 1;
            schedule.MonthlyTheRecurrence = 2;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.Contains("You cannot set The recurrence when using Day configuration", errors.ToString());
        }

        [Fact]
        public void ShouldFail_When_DailyOnceExecutionTimeMissing_DayOnce()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 10;
            schedule.MonthlyDayRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = null;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.Contains("DailyOnceExecutionTime is required for Monthly Day Once configuration", errors.ToString());
        }

        [Fact]
        public void ShouldFail_When_DayRangeFieldsMissing()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 10;
            schedule.MonthlyDayRecurrence = 1;
            schedule.DailyFrequencyRangeCheckbox = true;
            schedule.DailyStartTime = null;
            schedule.DailyEndTime = null;
            schedule.TimeUnit = null;
            schedule.TimeUnitNumberOf = 0;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.Contains("DailyStartTime is required for Monthly Day Range configuration", errors.ToString());
            Assert.Contains("DailyEndTime is required for Monthly Day Range configuration", errors.ToString());
            Assert.Contains("TimeUnit is required for Monthly Day Range configuration", errors.ToString());
            Assert.Contains("TimeUnitNumberOf must be greater than 0 for Monthly Day Range configuration", errors.ToString());
        }

        [Fact]
        public void ShouldFail_When_DayRange_StartEqualsEnd()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 10;
            schedule.MonthlyDayRecurrence = 1;
            schedule.DailyFrequencyRangeCheckbox = true;
            schedule.DailyStartTime = new TimeSpan(9, 0, 0);
            schedule.DailyEndTime = new TimeSpan(9, 0, 0);
            schedule.TimeUnit = TimeUnit.Minutes;
            schedule.TimeUnitNumberOf = 15;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.Contains("DailyStartTime cannot be the same as DailyEndTime", errors.ToString());
        }

        [Fact]
        public void ShouldFail_When_DayRange_StartAfterEnd()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 10;
            schedule.MonthlyDayRecurrence = 1;
            schedule.DailyFrequencyRangeCheckbox = true;
            schedule.DailyStartTime = new TimeSpan(18, 0, 0);
            schedule.DailyEndTime = new TimeSpan(8, 0, 0);
            schedule.TimeUnit = TimeUnit.Hours;
            schedule.TimeUnitNumberOf = 1;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.Contains("DailyStartTime cannot be after the DailyEndTime", errors.ToString());
        }

        [Fact]
        public void ShouldFail_When_DayRange_TimeUnitNumberOfLessThanOne()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 5;
            schedule.MonthlyDayRecurrence = 1;
            schedule.DailyFrequencyRangeCheckbox = true;
            schedule.DailyStartTime = new TimeSpan(9, 0, 0);
            schedule.DailyEndTime = new TimeSpan(10, 0, 0);
            schedule.TimeUnit = TimeUnit.Minutes;
            schedule.TimeUnitNumberOf = 0;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.Contains("TimeUnitNumberOf must be greater than 0 for Monthly Day Range configuration", errors.ToString());
        }

        [Fact]
        public void ShouldFail_When_TheOrderAndDayOfWeekMissing_AndRecurrenceZero()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = null;
            schedule.MonthlyTheDayOfWeek = null;
            schedule.MonthlyTheRecurrence = 0;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = null;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.Contains("Monthly order (First, Second, Third, Fourth, Last) is required for The configuration", errors.ToString());
            Assert.Contains("Monthly day of week is required for The configuration", errors.ToString());
            Assert.Contains("Monthly The recurrence must be greater than 0", errors.ToString());
        }

        [Fact]
        public void ShouldFail_When_TheOnce_MissingExecutionTime()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = MonthlyTheOrder.First;
            schedule.MonthlyTheDayOfWeek = MonthlyDayOfWeek.Monday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = null;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.Contains("DailyOnceExecutionTime is required for Monthly The Once configuration", errors.ToString());
        }

        [Fact]
        public void ShouldFail_When_TheRangeFieldsMissing()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = MonthlyTheOrder.Last;
            schedule.MonthlyTheDayOfWeek = MonthlyDayOfWeek.Friday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyRangeCheckbox = true;
            schedule.DailyStartTime = null;
            schedule.DailyEndTime = null;
            schedule.TimeUnit = null;
            schedule.TimeUnitNumberOf = 0;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.Contains("DailyStartTime is required for Monthly The Range configuration", errors.ToString());
            Assert.Contains("DailyEndTime is required for Monthly The Range configuration", errors.ToString());
            Assert.Contains("TimeUnit is required for Monthly The Range configuration", errors.ToString());
            Assert.Contains("TimeUnitNumberOf must be greater than 0 for Monthly The Range configuration", errors.ToString());
        }

        [Fact]
        public void ShouldFail_When_The_MonthlyDayNumberSet()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = MonthlyTheOrder.First;
            schedule.MonthlyTheDayOfWeek = MonthlyDayOfWeek.Monday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.MonthlyDayNumber = 5;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.Contains("You cannot set monthly day number when using The configuration", errors.ToString());
        }

        [Fact]
        public void ShouldFail_When_The_MonthlyDayRecurrenceSet()
        {
            var schedule = new ScheduleEntity();
            schedule.ScheduleType = ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = MonthlyTheOrder.First;
            schedule.MonthlyTheDayOfWeek = MonthlyDayOfWeek.Monday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.MonthlyDayRecurrence = 2;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            var errors = new StringBuilder();
            RecurringMonthlyValidator.Validate(schedule, errors);
            Assert.Contains("You cannot set Day recurrence when using The configuration", errors.ToString());
        }
    }
}
