using System;
using System.Collections.Generic;
using System.Text;
using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Services;
using MyScheduler.Validators;
using Xunit;

namespace MyScheduler
{
    public class WeeklyValidatorTests
    {
        // Common
        [Fact]
        public void WeeklyOnce_ShouldFail_WhenDaysOfWeekIsEmpty()
        {
            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                DailyOnceExecutionTime = new TimeSpan(13, 30, 0),
                DailyFrequencyOnceCheckbox = true,
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("DaysOfWeek", result.Error);
            Assert.Contains("must be selected", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenDaysOfWeekIsEmpty()
        {
            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                DailyOnceExecutionTime = new TimeSpan(13, 30, 0),
                DailyFrequencyEveryCheckbox = true,
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("DaysOfWeek", result.Error);
            Assert.Contains("must be selected", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenRecurrenceIsEmpty()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                DailyOnceExecutionTime = new TimeSpan(13, 30, 0),
                DailyFrequencyOnceCheckbox = true,
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("WeeklyRecurrence", result.Error);
            Assert.Contains("at least 1", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenRecurrenceIsEmpty()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                DailyOnceExecutionTime = new TimeSpan(13, 30, 0),
                DailyFrequencyEveryCheckbox = true,
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("WeeklyRecurrence", result.Error);
            Assert.Contains("at least 1", result.Error);
        }

        // Once (weekly) validations

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenExecutionTimeOfOneDayIsNull()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = null,
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("DailyOnceExecutionTime", result.Error);
            Assert.Contains("required", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenDailyStartTimeSet()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                DailyStartTime = new TimeSpan(13, 30, 0),
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = new TimeSpan(13, 30, 0),
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("Daily start time", result.Error);
            Assert.Contains("cannot be set", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenDailyEndTimeSet()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 21, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 1,
                DailyFrequencyOnceCheckbox = true,
                DailyEndTime = new TimeSpan(13, 30, 0),
                DailyOnceExecutionTime = new TimeSpan(13, 30, 0),
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("Daily end time", result.Error);
            Assert.Contains("cannot be set", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenTimeUnitSet()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                DailyFrequencyOnceCheckbox = true,
                TimeUnit = TimeUnit.Hours,
                DailyEndTime = new TimeSpan(13, 30, 0),
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("Time unit", result.Error);
            Assert.Contains("cannot be set", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenTimeUnitNumberOfSet()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                DailyFrequencyOnceCheckbox = true,
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                DailyEndTime = new TimeSpan(13, 30, 0),
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("Time unit", result.Error);
            Assert.Contains("cannot be set", result.Error);
        }

        // Every (weekly) validations

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenTimeUnitNotSet()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                DailyEndTime = new TimeSpan(13, 30, 0),
                TimeUnitNumberOf = 1,
                DailyFrequencyEveryCheckbox = true,
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("TimeUnit", result.Error);
            Assert.Contains("required", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenTimeUnitNumberOf_NotSet()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                DailyEndTime = new TimeSpan(13, 30, 0),
                DailyFrequencyEveryCheckbox = true,
                TimeUnit = TimeUnit.Hours,
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("TimeUnitNumberOf", result.Error);
            Assert.Contains("positive", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenDailyStartTime_NotSet()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                DailyEndTime = new TimeSpan(13, 30, 0),
                DailyFrequencyEveryCheckbox = true,
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("DailyStartTime", result.Error);
            Assert.Contains("required", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenDailyEndTime_NotSet()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                DailyFrequencyEveryCheckbox = true,
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                DailyStartTime = new TimeSpan(13, 30, 0),
                Enabled = true,
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("DailyEndTime", result.Error);
            Assert.Contains("required", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenExecutionOfOneDay_Set()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                DailyFrequencyEveryCheckbox = true,
                TimeUnit = TimeUnit.Hours,
                DailyOnceExecutionTime = new TimeSpan(13, 30, 0),
                TimeUnitNumberOf = 2,
                WeeklyRecurrence = 2,
                DailyStartTime = new TimeSpan(13, 30, 0),
                DailyEndTime = new TimeSpan(17, 30, 0),
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("Execution time of day", result.Error);
            Assert.Contains("cannot be set", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenStartTimeGreaterThanEndTime()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                DailyStartTime = new TimeSpan(13, 30, 0),
                DailyEndTime = new TimeSpan(12, 30, 0),
                DailyFrequencyEveryCheckbox = true,
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("DailyStartTime", result.Error);
            Assert.Contains("after", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenStartTimeIsSameAsEndTime()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                DailyStartTime = new TimeSpan(13, 30, 0),
                DailyEndTime = new TimeSpan(13, 30, 0),
                DailyFrequencyEveryCheckbox = true,
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("DailyStartTime", result.Error);
            Assert.Contains("same as DailyEndTime", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenNoExecutionsAvalaible()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                DailyFrequencyEveryCheckbox = true,
                WeeklyRecurrence = 2,
                DailyStartTime = new TimeSpan(13, 30, 0),
                DailyEndTime = new TimeSpan(13, 50, 0),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 2,
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("No next execution", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenWeeklyRecurrenceIsZero()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                DailyFrequencyEveryCheckbox = true,
                CurrentDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 0,
                DailyStartTime = new TimeSpan(13, 30, 0),
                DailyEndTime = new TimeSpan(13, 50, 0),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 2,
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("WeeklyRecurrence", result.Error);
            Assert.Contains("at least 1", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenTimeUnitNumberOf_IsZero()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                DailyFrequencyEveryCheckbox = true,
                CurrentDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                DailyStartTime = new TimeSpan(13, 30, 0),
                DailyEndTime = new TimeSpan(13, 50, 0),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 0,
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("TimeUnitNumberOf", result.Error);
            Assert.Contains("positive", result.Error);
        }

        // Working tests

        [Fact]
        public void WeeklyOnce_ShouldPass_WhenWorking()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero),
                DailyFrequencyOnceCheckbox = true,
                WeeklyRecurrence = 2,
                DailyOnceExecutionTime = new TimeSpan(13, 30, 0),
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsSuccess);
            Assert.Equal(new DateTimeOffset(2025, 10, 27, 13, 30, 0, TimeSpan.Zero), result.Value.ExecutionTime);
            Assert.Contains("occurs every 2 weeks on monday", result.Value.Description.ToLower());
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenWorking()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                DailyFrequencyEveryCheckbox = true,
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                DailyStartTime = new TimeSpan(13, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0),
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsSuccess, result.Error);
            Assert.Equal(new DateTimeOffset(2025, 10, 27, 13, 0, 0, TimeSpan.Zero), result.Value.ExecutionTime);
        }

        // Duplicate days

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenSameWeekDay()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Monday, DayOfWeek.Monday, DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero),
                DailyFrequencyEveryCheckbox = true,
                WeeklyRecurrence = 2,
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                DailyStartTime = new TimeSpan(13, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0),
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("Check days of the week", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenSameWeekDay()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Monday, DayOfWeek.Monday, DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = new TimeSpan(13, 30, 0),
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("Check days of the week", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenSimpleOccurrenceLowerThanTimeSpanZero()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = new TimeSpan(-1, 0, 0),
                Enabled = true
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("DailyOnceExecutionTime", result.Error);
            Assert.Contains("wrong", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenNoEndateAndNoOccurrences()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Monday, DayOfWeek.Monday, DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                DailyStartTime = new TimeSpan(13, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0)
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, null);

            Assert.True(result.IsFailure);
            Assert.Contains("end date", result.Error);
            Assert.Contains("num occurrences", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenStartDateLessThanMinValue()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = DateTimeOffset.MinValue,
                EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                DailyStartTime = new TimeSpan(13, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0)
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("StartDate", result.Error);
            Assert.Contains("Min value", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenStartDateMoreThanMaxValue()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = DateTimeOffset.MaxValue,
                WeeklyRecurrence = 2,
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                DailyStartTime = new TimeSpan(13, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0)
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("StartDate", result.Error);
            Assert.Contains("Max value", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenEndDateMoreThanMaxValue()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = DateTimeOffset.MaxValue,
                WeeklyRecurrence = 2,
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                DailyStartTime = new TimeSpan(13, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0)
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("EndDate", result.Error);
            Assert.Contains("Max value", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenNoEndDateAndNoOccurrences_WithNegativeOccurrences()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                DailyStartTime = new TimeSpan(13, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0)
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, -1);

            Assert.True(result.IsFailure);
            Assert.Contains("end date", result.Error);
            Assert.Contains("num occurrences", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenEndDateBeforeCurrentDate()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var schedulerConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DaysOfWeek = listOfDays,
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 14, 0, 0, 0, TimeSpan.Zero),
                WeeklyRecurrence = 2,
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                DailyStartTime = new TimeSpan(13, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0)
            };

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, -1);

            Assert.True(result.IsFailure);
            Assert.Contains("end date", result.Error);
            Assert.Contains("after the current date", result.Error);
        }

        [Fact]
        public void Validate_ShouldFail_WhenEndDateBeforeCurrentDate()
        {
            var errors = new StringBuilder();
            var config = new ScheduleEntity
            {
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero)
            };

            CommonRules.Validate(config, errors, 1);

            Assert.Contains("The end date of a recurring scheduleConfig must be after the current date.", errors.ToString());
        }

        [Fact]
        public void Validate_ShouldPass_WhenEndDateAfterCurrentDate()
        {
            var errors = new StringBuilder();
            var config = new ScheduleEntity
            {
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 20, 0, 0, 0, TimeSpan.Zero)
            };

            CommonRules.Validate(config, errors, 1);

            Assert.DoesNotContain("The end date of a recurring scheduleConfig must be after the current date.", errors.ToString());
        }

        [Fact]
        public void Validate_ShouldIgnore_WhenEndDateIsNull()
        {
            var errors = new StringBuilder();
            var config = new ScheduleEntity
            {
                CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero),
                EndDate = null
            };

            CommonRules.Validate(config, errors, 1);

            Assert.DoesNotContain("The end date of a recurring scheduleConfig must be after the current date.", errors.ToString());
        }
    }
}
