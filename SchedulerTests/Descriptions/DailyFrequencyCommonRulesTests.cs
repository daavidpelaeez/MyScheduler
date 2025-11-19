using System;
using System.Text;
using Xunit;
using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Validators;

namespace MyScheduler.Descriptions
{
    public class DailyFrequencyCommonRulesTests
    {
        [Fact]
        public void BothCheckboxesTrue_AddsOnceAndEveryError()
        {
            var schedule = new ScheduleEntity
            {
                DailyFrequencyOnceCheckbox = true,
                DailyFrequencyRangeCheckbox = true
            };
            var errors = new StringBuilder();
            DailyFrequencyCommonRules.CheckDailyCommonRules(schedule, errors);
            Assert.Contains("Daily frequency cannot be once and every at the same time", errors.ToString());
        }

        [Fact]
        public void BothCheckboxesFalse_AddsEmptyError()
        {
            var schedule = new ScheduleEntity
            {
                DailyFrequencyOnceCheckbox = false,
                DailyFrequencyRangeCheckbox = false
            };
            var errors = new StringBuilder();
            DailyFrequencyCommonRules.CheckDailyCommonRules(schedule, errors);
            Assert.Contains("Daily frequency cannot be empty for daily tasks", errors.ToString());
        }

        [Fact]
        public void OnlyOnceCheckboxTrue_AddsAllOnceErrors()
        {
            var schedule = new ScheduleEntity
            {
                DailyFrequencyOnceCheckbox = true,
                DailyFrequencyRangeCheckbox = false,
                TimeUnitNumberOf = 2,
                TimeUnit = TimeUnit.Hours,
                DailyStartTime = TimeSpan.FromHours(8),
                DailyEndTime = TimeSpan.FromHours(10),
                DailyOnceExecutionTime = null,
                ScheduleType = ScheduleType.Recurring
            };
            var errors = new StringBuilder();
            DailyFrequencyCommonRules.CheckDailyCommonRules(schedule, errors);
            var text = errors.ToString();
            Assert.Contains("Daily tasks cannot have timeUnitNumberOf", text);
            Assert.Contains("Time unit cannot be set for a daily frequency once task", text);
            Assert.Contains("Daily start time cannot be set for a daily frequency once task", text);
            Assert.Contains("Daily end time cannot be set for a daily frequency once task", text);
            Assert.Contains("DailyOnceExecutionTime is required", text);
            Assert.DoesNotContain("You cannot set daily frequency once in a daily every task type", text);
        }

        [Fact]
        public void OnlyRangeCheckboxTrue_AddsAllRangeErrors()
        {
            var schedule = new ScheduleEntity
            {
                DailyFrequencyOnceCheckbox = false,
                DailyFrequencyRangeCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.FromHours(8),
                TimeUnit = null,
                TimeUnitNumberOf = 0,
                DailyStartTime = null,
                DailyEndTime = null,
                ScheduleType = ScheduleType.Recurring
            };
            var errors = new StringBuilder();
            DailyFrequencyCommonRules.CheckDailyCommonRules(schedule, errors);
            var text = errors.ToString();
            Assert.Contains("Execution time of day cannot be set for a daily frequency every task", text);
            Assert.Contains("You need to set a time unit for daily every configurations", text);
            Assert.Contains("You need to set a time unit number of greater than 0", text);
            Assert.Contains("You need to set a daily start time", text);
            Assert.Contains("You need to set a daily end time", text);
            Assert.DoesNotContain("You cannot set daily frequency every in a daily once task type", text);
        }

        [Fact]
        public void ValidRange_NoErrors()
        {
            var schedule = new ScheduleEntity
            {
                DailyFrequencyOnceCheckbox = false,
                DailyFrequencyRangeCheckbox = true,
                DailyOnceExecutionTime = null,
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                DailyStartTime = TimeSpan.FromHours(8),
                DailyEndTime = TimeSpan.FromHours(10),
                ScheduleType = ScheduleType.Recurring
            };
            var errors = new StringBuilder();
            DailyFrequencyCommonRules.CheckDailyCommonRules(schedule, errors);
            Assert.Equal(string.Empty, errors.ToString());
        }

        [Fact]
        public void BothCheckboxesTrue_AllErrors()
        {
            var schedule = new ScheduleEntity
            {
                DailyFrequencyOnceCheckbox = true,
                DailyFrequencyRangeCheckbox = true,
                TimeUnitNumberOf = 2,
                TimeUnit = TimeUnit.Hours,
                DailyStartTime = TimeSpan.FromHours(8),
                DailyEndTime = TimeSpan.FromHours(10),
                DailyOnceExecutionTime = TimeSpan.FromHours(9),
                ScheduleType = ScheduleType.Recurring
            };
            var errors = new StringBuilder();
            DailyFrequencyCommonRules.CheckDailyCommonRules(schedule, errors);
            var text = errors.ToString();
            Assert.Contains("Daily frequency cannot be once and every at the same time", text);
            Assert.Contains("Daily tasks cannot have timeUnitNumberOf", text);
            Assert.Contains("Time unit cannot be set for a daily frequency once task", text);
            Assert.Contains("Daily start time cannot be set for a daily frequency once task", text);
            Assert.Contains("Daily end time cannot be set for a daily frequency once task", text);
            Assert.DoesNotContain("DailyOnceExecutionTime is required", text);
            Assert.Contains("You cannot set daily frequency once in a daily every task type", text);
            Assert.Contains("Execution time of day cannot be set for a daily frequency every task", text);
            Assert.DoesNotContain("You need to set a time unit for daily every configurations", text);
            Assert.DoesNotContain("You need to set a time unit number of greater than 0", text);
            Assert.DoesNotContain("You need to set a daily start time", text);
            Assert.DoesNotContain("You need to set a daily end time", text);
            Assert.Contains("You cannot set daily frequency every in a daily once task type", text);
        }
    }
}
