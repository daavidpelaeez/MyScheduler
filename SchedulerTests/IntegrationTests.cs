using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace MyScheduler
{
    public class IntegrationTests
    {
        [Fact]
        public void OnceSchedule_HappyPath_ReturnsExpectedOutput()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.Once;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.EventDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsSuccess);
            var expectedTime = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero);
            var expectedDescription = "Occurs once. Schedule on 31/10/2025 at 00:00, starting 25/10/2025";

            Assert.Equal(expectedTime, result.Value.ExecutionTime);
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Fact]
        public void Recurring_Daily_Once_HappyPath()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.Recurring;
            config.Occurs = Occurs.Daily;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 28, 0, 0, 0, TimeSpan.Zero);
            config.Recurrence = 2;
            config.DailyFrequencyOnceCheckbox = true;
            config.DailyOnceExecutionTime = new TimeSpan(13, 30, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsSuccess);
            var expectedTime = new DateTimeOffset(2025, 10, 29, 13, 30, 0, TimeSpan.Zero);
            var expectedDescription = "Occurs every 2 day(s). Next on 29/10/2025, starting 25/10/2025";

            Assert.Equal(expectedTime, result.Value.ExecutionTime);
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Fact]
        public void Recurring_Daily_Every_HappyPath()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.Recurring;
            config.Occurs = Occurs.Daily;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.Recurrence = 2;
            config.DailyFrequencyEveryCheckbox = true;
            config.TimeUnit = TimeUnit.Hours;
            config.TimeUnitNumberOf = 2;
            config.DailyStartTime = new TimeSpan(13, 30, 0);
            config.DailyEndTime = new TimeSpan(15, 30, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsSuccess);
            var expectedTime = new DateTimeOffset(2025, 10, 25, 13, 30, 0, TimeSpan.Zero);
            var expectedDescription = "Occurs every 2 day(s) from 13:30:00 to 15:30:00 every 2 hours";

            Assert.Equal(expectedTime, result.Value.ExecutionTime);
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Fact]
        public void Recurring_Weekly_Once_HappyPath()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.Recurring;
            config.Occurs = Occurs.Weekly;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.WeeklyRecurrence = 1;
            config.DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday };
            config.DailyFrequencyOnceCheckbox = true;
            config.DailyOnceExecutionTime = new TimeSpan(10, 0, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 5);

            Assert.True(result.IsSuccess);
            var expectedTime = new DateTimeOffset(2025, 10, 27, 10, 0, 0, TimeSpan.Zero);
            var expectedDescription = "Occurs every 1 weeks on monday and wednesday at 10:00:00, starting 25/10/2025";

            Assert.Equal(expectedTime, result.Value.ExecutionTime);
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Fact]
        public void Recurring_Weekly_Every_HappyPath()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.Recurring;
            config.Occurs = Occurs.Weekly;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.WeeklyRecurrence = 1;
            config.DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday };
            config.DailyFrequencyEveryCheckbox = true;
            config.DailyStartTime = new TimeSpan(9, 0, 0);
            config.DailyEndTime = new TimeSpan(12, 0, 0);
            config.TimeUnit = TimeUnit.Hours;
            config.TimeUnitNumberOf = 2;

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 5);

            Assert.True(result.IsSuccess, result.Error);
            var expectedTime = new DateTimeOffset(2025, 10, 27, 9, 0, 0, TimeSpan.Zero);
            var expectedDescription = "Occurs every 1 weeks on monday every 2 hours between 09:00:00 and 12:00:00, starting 25/10/2025";

            Assert.Equal(expectedTime, result.Value.ExecutionTime);
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Fact]
        public void DisabledSchedule_ReturnsFailure()
        {
            var config = new ScheduleEntity();
            config.Enabled = false;
            config.ScheduleType = ScheduleType.Recurring;
            config.Occurs = Occurs.Daily;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 26, 0, 0, 0, TimeSpan.Zero);
            config.Recurrence = 1;
            config.DailyFrequencyOnceCheckbox = true;
            config.DailyOnceExecutionTime = new TimeSpan(13, 30, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("enabled", result.Error.ToLower());
        }

        [Fact]
        public void DailyOnce_MissingExecutionTime_ReturnsFailure()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.Recurring;
            config.Occurs = Occurs.Daily;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.Recurrence = 1;
            config.DailyFrequencyOnceCheckbox = true;
            config.DailyOnceExecutionTime = null;

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("execution", result.Error.ToLower());
        }

        [Fact]
        public void DailyEvery_MissingTimeUnit_ReturnsFailure()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.Recurring;
            config.Occurs = Occurs.Daily;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.Recurrence = 1;
            config.DailyFrequencyEveryCheckbox = true;
            config.TimeUnit = null;
            config.TimeUnitNumberOf = 2;
            config.DailyStartTime = new TimeSpan(9, 0, 0);
            config.DailyEndTime = new TimeSpan(12, 0, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("time unit", result.Error.ToLower());
        }

        [Fact]
        public void WeeklyOnce_MissingDaysOfWeek_ReturnsFailure()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.Recurring;
            config.Occurs = Occurs.Weekly;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.WeeklyRecurrence = 1;
            config.DaysOfWeek = new List<DayOfWeek>();
            config.DailyFrequencyOnceCheckbox = true;
            config.DailyOnceExecutionTime = new TimeSpan(10, 0, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 5);

            Assert.True(result.IsFailure);
            Assert.Contains("daysofweek", result.Error.ToLower());
        }

        [Fact]
        public void EndDateBeforeCurrent_ReturnsFailure()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.Recurring;
            config.Occurs = Occurs.Daily;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 28, 0, 0, 0, TimeSpan.Zero);
            config.EndDate = new DateTimeOffset(2025, 10, 27, 0, 0, 0, TimeSpan.Zero);
            config.Recurrence = 1;
            config.DailyFrequencyOnceCheckbox = true;
            config.DailyOnceExecutionTime = new TimeSpan(13, 30, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("end date", result.Error.ToLower());
            Assert.Contains("after", result.Error.ToLower());
        }

        [Fact]
        public void DailyOnce_EndOfMonthAndLeapYear_ReturnsExpectedNextExecution()
        {
            var manager = new ScheduleManager();

            var configLeap = new ScheduleEntity();
            configLeap.Enabled = true;
            configLeap.ScheduleType = ScheduleType.Recurring;
            configLeap.Occurs = Occurs.Daily;
            configLeap.StartDate = new DateTimeOffset(2024, 2, 28, 0, 0, 0, TimeSpan.Zero);
            configLeap.CurrentDate = new DateTimeOffset(2024, 2, 28, 0, 0, 0, TimeSpan.Zero);
            configLeap.Recurrence = 1;
            configLeap.DailyFrequencyOnceCheckbox = true;
            configLeap.DailyOnceExecutionTime = new TimeSpan(10, 0, 0);

            var resLeap = manager.GetNextExecution(configLeap, 1);
            Assert.True(resLeap.IsSuccess);
            Assert.Equal(new DateTimeOffset(2024, 2, 28, 10, 0, 0, TimeSpan.Zero), resLeap.Value.ExecutionTime);

            var configYearEnd = new ScheduleEntity();
            configYearEnd.Enabled = true;
            configYearEnd.ScheduleType = ScheduleType.Recurring;
            configYearEnd.Occurs = Occurs.Daily;
            configYearEnd.StartDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            configYearEnd.CurrentDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            configYearEnd.Recurrence = 1;
            configYearEnd.DailyFrequencyOnceCheckbox = true;
            configYearEnd.DailyOnceExecutionTime = new TimeSpan(23, 59, 0);

            var resYear = manager.GetNextExecution(configYearEnd, 1);
            Assert.True(resYear.IsSuccess);
            Assert.Equal(new DateTimeOffset(2025, 12, 31, 23, 59, 0, TimeSpan.Zero), resYear.Value.ExecutionTime);
        }

        [Fact]
        public void Recurring_Daily_Every_StartsAtStartTime_ReturnsStartTime()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.Recurring;
            config.Occurs = Occurs.Daily;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.Recurrence = 1;
            config.DailyFrequencyEveryCheckbox = true;
            config.TimeUnit = TimeUnit.Minutes;
            config.TimeUnitNumberOf = 30;
            config.DailyStartTime = new TimeSpan(8, 15, 0);
            config.DailyEndTime = new TimeSpan(9, 45, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsSuccess, result.Error);
            Assert.Equal(new DateTimeOffset(2025, 10, 25, 8, 15, 0, TimeSpan.Zero), result.Value.ExecutionTime);
            Assert.Contains("every 1 day", result.Value.Description.ToLower());
        }

        [Fact]
        public void Recurring_Daily_Every_TimeUnitNumberOfZero_ReturnsFailure()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.Recurring;
            config.Occurs = Occurs.Daily;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.Recurrence = 1;
            config.DailyFrequencyEveryCheckbox = true;
            config.TimeUnit = TimeUnit.Hours;
            config.TimeUnitNumberOf = 0;
            config.DailyStartTime = new TimeSpan(9, 0, 0);
            config.DailyEndTime = new TimeSpan(12, 0, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("you need to set a time unit number of", result.Error.ToLower());
        }

        [Fact]
        public void Recurring_Daily_BothFlagsTrue_ReturnsFailure()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.Recurring;
            config.Occurs = Occurs.Daily;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.Recurrence = 1;
            config.DailyFrequencyOnceCheckbox = true;
            config.DailyFrequencyEveryCheckbox = true;
            config.DailyOnceExecutionTime = new TimeSpan(10, 0, 0);
            config.TimeUnit = TimeUnit.Hours;
            config.TimeUnitNumberOf = 1;
            config.DailyStartTime = new TimeSpan(9, 0, 0);
            config.DailyEndTime = new TimeSpan(17, 0, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("daily frequency", result.Error.ToLower());
        }

        [Fact]
        public void Recurring_Weekly_Every_NoDaysSelected_ReturnsFailure()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.Recurring;
            config.Occurs = Occurs.Weekly;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.WeeklyRecurrence = 1;
            config.DaysOfWeek = new List<DayOfWeek>();
            config.DailyFrequencyEveryCheckbox = true;
            config.TimeUnit = TimeUnit.Hours;
            config.TimeUnitNumberOf = 2;
            config.DailyStartTime = new TimeSpan(9, 0, 0);
            config.DailyEndTime = new TimeSpan(12, 0, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("daysofweek", result.Error.ToLower());
        }

        [Fact]
        public void Recurring_Weekly_Every_TimeUnitMissing_ReturnsFailure()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.Recurring;
            config.Occurs = Occurs.Weekly;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.WeeklyRecurrence = 1;
            config.DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday };
            config.DailyFrequencyEveryCheckbox = true;
            config.TimeUnit = null;
            config.TimeUnitNumberOf = 2;
            config.DailyStartTime = new TimeSpan(9, 0, 0);
            config.DailyEndTime = new TimeSpan(12, 0, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("timeunit", result.Error.ToLower());
        }
    }
}
