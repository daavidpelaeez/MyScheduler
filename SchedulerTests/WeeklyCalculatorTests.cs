using System;
using System.Collections.Generic;
using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Helpers;
using MyScheduler.ScheduleCalculators;
using MyScheduler.Services;
using Xunit;

namespace MyScheduler
{
    public class WeeklyCalculatorTests
    {

        [Fact]
        public void WeeklyOnce_ShouldPass_WhenCheckingCorrectDescription()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Sunday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = new TimeSpan(13, 30, 0),
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 4,
                StartDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero)
            };

            var result = DescriptionGenerator.GetDescription(taskConfig);
            string expectedDescription = "Occurs every 4 week(s) on monday, tuesday and sunday at 13:30:00, starting 16/10/2025";

            Assert.Equal(expectedDescription, result);
        }

        [Fact]
        public void WeeklyOnce_ShouldPass_WhenCheckingCorrectOutput()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Sunday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = new TimeSpan(13, 30, 0),
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 4,
                StartDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero),
                Enabled = true
            };

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(taskConfig, 10);

            string expectedDescription = "Occurs every 4 week(s) on monday, tuesday and sunday at 13:30:00, starting 16/10/2025";
            DateTimeOffset expectedExecutionTime = new DateTimeOffset(2025, 10, 19, 13, 30, 0, TimeSpan.Zero);

            Assert.Equal(expectedDescription, result.Value.Description);
            Assert.Equal(expectedExecutionTime, result.Value.ExecutionTime);
        }


        [Fact]
        public void WeeklyOnce_ShouldPass_WhenUsingOccurrences()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Sunday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = new TimeSpan(13, 30, 0),
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 3,
                StartDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero)
            };

            var calculator = new RecurringWeeklyOnceCalculator();
            var result = calculator.CalculateExecutions(taskConfig, 5);

            var expectedExecutions = new List<DateTimeOffset>
            {
                new DateTimeOffset(2025, 10, 19, 13, 30, 0, TimeSpan.Zero),
                new DateTimeOffset(2025, 11, 3, 13, 30, 0, TimeSpan.Zero),
                new DateTimeOffset(2025, 11, 4, 13, 30, 0, TimeSpan.Zero),
                new DateTimeOffset(2025, 11, 9, 13, 30, 0, TimeSpan.Zero),
                new DateTimeOffset(2025, 11, 24, 13, 30, 0, TimeSpan.Zero)
            };

            Assert.Equal(expectedExecutions, result);
        }


        [Fact]
        public void WeeklyEvery_ShouldPass_WhenCheckingCorrectOuput()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 1,
                StartDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 21, 0, 0, 0, TimeSpan.Zero),
                DailyStartTime = new TimeSpan(13, 30, 0),
                DailyEndTime = new TimeSpan(19, 30, 0),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 2,
                Enabled = true
            };

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(taskConfig, 10);

            string expectedDescription = "Occurs every 1 week(s) on monday every 2 hours between 13:30:00 and 19:30:00, starting 17/10/2025";
            DateTimeOffset expectedExecutionTime = new DateTimeOffset(2025, 10, 20, 13, 30, 0, TimeSpan.FromHours(2));

            Assert.Equal(expectedDescription, result.Value.Description);
            Assert.Equal(expectedExecutionTime, result.Value.ExecutionTime);
        }


        [Fact]
        public void WeeklyEvery_ShouldPass_WhenCalculateConfig()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Sunday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 4,
                StartDate = new DateTimeOffset(2025, 10, 19, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 19, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero),
                DailyStartTime = new TimeSpan(13, 30, 0),
                DailyEndTime = new TimeSpan(19, 30, 0),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 2
            };

            var weManager = new RecurringWeeklyRangeCalculator();
            var result = weManager.CalculateExecutions(taskConfig, 10);

            var expected = new List<DateTimeOffset>
            {
                new DateTimeOffset(2025, 10, 19, 13, 30, 0, TimeSpan.FromHours(2)),
                new DateTimeOffset(2025, 10, 19, 15, 30, 0, TimeSpan.FromHours(2)),
                new DateTimeOffset(2025, 10, 19, 17, 30, 0, TimeSpan.FromHours(2)),
                new DateTimeOffset(2025, 10, 19, 19, 30, 0, TimeSpan.FromHours(2))
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenJumpIs45MinutesStartingAtHalfHour()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Sunday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                DailyOnceExecutionTime = new TimeSpan(13, 30, 0),
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 4,
                StartDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero),
                DailyStartTime = new TimeSpan(13, 30, 0),
                DailyEndTime = new TimeSpan(15, 30, 0),
                TimeUnit = TimeUnit.Minutes,
                TimeUnitNumberOf = 45
            };

            var weManager = new RecurringWeeklyRangeCalculator();
            var result = weManager.CalculateExecutions(taskConfig, 10);

            var expected = new List<DateTimeOffset>
            {
                new DateTimeOffset(2025, 10, 19, 13, 30, 0, TimeSpan.FromHours(2)),
                new DateTimeOffset(2025, 10, 19, 14, 15, 0, TimeSpan.FromHours(2)),
                new DateTimeOffset(2025, 10, 19, 15, 0, 0, TimeSpan.FromHours(2))
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenJumpIs27Seconds()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Sunday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 4,
                StartDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero),
                DailyStartTime = new TimeSpan(13, 59, 0),
                DailyEndTime = new TimeSpan(14, 0, 0),
                TimeUnit = TimeUnit.Seconds,
                TimeUnitNumberOf = 27
            };

            var weManager = new RecurringWeeklyRangeCalculator();
            var result = weManager.CalculateExecutions(taskConfig, 10);

            var expected = new List<DateTimeOffset>
            {
                new DateTimeOffset(2025, 10, 19, 13, 59, 0, TimeSpan.FromHours(2)),
                new DateTimeOffset(2025, 10, 19, 13, 59, 27, TimeSpan.FromHours(2)),
                new DateTimeOffset(2025, 10, 19, 13, 59, 54, TimeSpan.FromHours(2))
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyEvery_ShouldSkipTodayAndTakeNextDay()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 2,
                StartDate = new DateTimeOffset(2025, 10, 20, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero),
                DailyStartTime = new TimeSpan(16, 0, 0),
                DailyEndTime = new TimeSpan(20, 0, 0),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 2
            };

            var weManager = new RecurringWeeklyRangeCalculator();
            var result = weManager.CalculateExecutions(taskConfig, 10);

            var expected = new List<DateTimeOffset>
            {
                new DateTimeOffset(2025, 10, 20, 16, 0, 0, TimeSpan.FromHours(2)),
                new DateTimeOffset(2025, 10, 20, 18, 0, 0, TimeSpan.FromHours(2)),
                new DateTimeOffset(2025, 10, 20, 20, 0, 0, TimeSpan.FromHours(2))
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenCalculateUsingOccurrences()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Sunday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 4,
                StartDate = new DateTimeOffset(2025, 10, 19, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 19, 0, 0, 0, TimeSpan.Zero),
                DailyStartTime = new TimeSpan(13, 30, 0),
                DailyEndTime = new TimeSpan(19, 30, 0),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 2
            };

            var weManager = new RecurringWeeklyRangeCalculator();
            var result = weManager.CalculateExecutions(taskConfig, 2);

            var expected = new List<DateTimeOffset>
            {
                new DateTimeOffset(2025, 10, 19, 13, 30, 0, TimeSpan.FromHours(2)),
                new DateTimeOffset(2025, 10, 19, 15, 30, 0, TimeSpan.FromHours(2))
            };

            Assert.Equal(expected, result);
        }

        // Common
        [Fact]
        public void GetMatchingDays_ShouldAdvanceCorrectly_WhenGivenSundayJump()
        {
            var startDate = new DateTimeOffset(new DateTime(2025, 10, 12));
            var currentDate = startDate;
            var daysOfWeek = new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday };

            var taskConfig = new ScheduleEntity
            {
                StartDate = startDate,
                CurrentDate = currentDate,
                WeeklyRecurrence = 1,
                DaysOfWeek = daysOfWeek,
                EndDate = startDate.AddDays(15)
            };

            int? maxOccurrences = 10;

            var result = WeeklyScheduleHelper.GetMatchingDays(taskConfig, maxOccurrences);

            Assert.NotEmpty(result);

            for (int i = 1; i < result.Count; i++)
            {
                Assert.True(result[i] > result[i - 1]);
                Assert.NotEqual(result[i].Date, result[i - 1].Date);
            }
        }

        [Fact]
        public void WeeklyEvery_ShouldReturnAfterStartdate_WhenCurrentDateIsBefore()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Sunday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 4,
                StartDate = new DateTimeOffset(2025, 10, 19, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 13, 0, 0, 0, TimeSpan.Zero),
                DailyStartTime = new TimeSpan(13, 30, 0),
                DailyEndTime = new TimeSpan(19, 30, 0),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 2
            };

            var weManager = new RecurringWeeklyRangeCalculator();
            var result = weManager.CalculateExecutions(taskConfig, 2);

            var expected = new List<DateTimeOffset>
            {
                new DateTimeOffset(2025, 10, 19, 13, 30, 0, TimeSpan.FromHours(2)),
                new DateTimeOffset(2025, 10, 19, 15, 30, 0, TimeSpan.FromHours(2))
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenCurrentDateIsAfterEndDate()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Sunday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 4,
                StartDate = new DateTimeOffset(2025, 10, 19, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 20, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 19, 0, 0, 0, TimeSpan.Zero),
                DailyStartTime = new TimeSpan(13, 30, 0),
                DailyEndTime = new TimeSpan(19, 30, 0),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 2
            };

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(taskConfig, 10);

            Assert.True(result.IsFailure, result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenDatesAtTheEndOfYearWithOccurrences()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Sunday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 4,
                StartDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero),
                DailyStartTime = new TimeSpan(13, 30, 0),
                DailyEndTime = new TimeSpan(19, 30, 0),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 2
            };

            var weManager = new RecurringWeeklyRangeCalculator();
            var result = weManager.CalculateExecutions(taskConfig, 5);

            var expected = new List<DateTimeOffset>
            {
                new DateTimeOffset(2026, 1, 4, 13, 30, 0, TimeSpan.FromHours(1)),
                new DateTimeOffset(2026, 1, 4, 15, 30, 0, TimeSpan.FromHours(1)),
                new DateTimeOffset(2026, 1, 4, 17, 30, 0, TimeSpan.FromHours(1)),
                new DateTimeOffset(2026, 1, 4, 19, 30, 0, TimeSpan.FromHours(1)),
                new DateTimeOffset(2026, 1, 26, 13, 30, 0, TimeSpan.FromHours(1))
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenDatesAtTheEndOfYearWithEndDate()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Sunday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 4,
                StartDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2026, 1, 5, 0, 0, 0, TimeSpan.Zero),
                DailyStartTime = new TimeSpan(13, 30, 0),
                DailyEndTime = new TimeSpan(19, 30, 0),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 2
            };

            var weManager = new RecurringWeeklyRangeCalculator();
            var result = weManager.CalculateExecutions(taskConfig, 500);

            var expected = new List<DateTimeOffset>
            {
                new DateTimeOffset(2026, 1, 4, 13, 30, 0, TimeSpan.FromHours(1)),
                new DateTimeOffset(2026, 1, 4, 15, 30, 0, TimeSpan.FromHours(1)),
                new DateTimeOffset(2026, 1, 4, 17, 30, 0, TimeSpan.FromHours(1)),
                new DateTimeOffset(2026, 1, 4, 19, 30, 0, TimeSpan.FromHours(1))
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenExecutionAtTheEndDate()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Sunday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 4,
                StartDate = new DateTimeOffset(2025, 10, 27, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 27, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 28, 0, 0, 0, TimeSpan.Zero),
                DailyStartTime = new TimeSpan(13, 30, 0),
                DailyEndTime = new TimeSpan(19, 30, 0),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 2
            };

            var weManager = new RecurringWeeklyRangeCalculator();
            var result = weManager.CalculateExecutions(taskConfig, 500);

            var expected = new List<DateTimeOffset>
            {
                new DateTimeOffset(2025, 10, 27, 13, 30, 0, TimeSpan.FromHours(1)),
                new DateTimeOffset(2025, 10, 27, 15, 30, 0, TimeSpan.FromHours(1)),
                new DateTimeOffset(2025, 10, 27, 17, 30, 0, TimeSpan.FromHours(1)),
                new DateTimeOffset(2025, 10, 27, 19, 30, 0, TimeSpan.FromHours(1)),
                new DateTimeOffset(2025, 10, 28, 13, 30, 0, TimeSpan.FromHours(1)),
                new DateTimeOffset(2025, 10, 28, 15, 30, 0, TimeSpan.FromHours(1)),
                new DateTimeOffset(2025, 10, 28, 17, 30, 0, TimeSpan.FromHours(1)),
                new DateTimeOffset(2025, 10, 28, 19, 30, 0, TimeSpan.FromHours(1))
            };

            Assert.Equal(expected, result);
        }

        // Recurring Weekly - Once (execution at end date)
        [Fact]
        public void WeeklyOnce_ShouldPass_WhenExecutionAtTheEndDate()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Sunday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyOnceCheckbox = true,
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 2,
                StartDate = new DateTimeOffset(2025, 10, 27, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 27, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 28, 0, 0, 0, TimeSpan.Zero),
                DailyOnceExecutionTime = new TimeSpan(13, 30, 0)
            };

            var onceCalc = new RecurringWeeklyOnceCalculator();
            var result = onceCalc.CalculateExecutions(taskConfig, 500);

            var expected = new List<DateTimeOffset>
            {
                new DateTimeOffset(2025, 10, 27, 13, 30, 0, TimeSpan.Zero),
                new DateTimeOffset(2025, 10, 28, 13, 30, 0, TimeSpan.Zero)
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyOnce_ShouldPass_JumpWhenDayItsNotSelected()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyOnceCheckbox = true,
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 1,
                StartDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 21, 0, 0, 0, TimeSpan.Zero),
                DailyOnceExecutionTime = new TimeSpan(13, 30, 0)
            };

            var onceCalc = new RecurringWeeklyOnceCalculator();
            var result = onceCalc.CalculateExecutions(taskConfig, 500);

            var expected = new List<DateTimeOffset>
            {
                new DateTimeOffset(2025, 10, 20, 13, 30, 0, TimeSpan.Zero)
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_JumpWhenDayItsNotSelected()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 1,
                StartDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 10, 21, 0, 0, 0, TimeSpan.Zero),
                DailyStartTime = new TimeSpan(13, 30, 0),
                DailyEndTime = new TimeSpan(19, 30, 0),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 2
            };

            var weManager = new RecurringWeeklyRangeCalculator();
            var result = weManager.CalculateExecutions(taskConfig, 500);

            var expected = new List<DateTimeOffset>
            {
                new DateTimeOffset(2025, 10, 20, 13, 30, 0, TimeSpan.FromHours(2)),
                new DateTimeOffset(2025, 10, 20, 15, 30, 0, TimeSpan.FromHours(2)),
                new DateTimeOffset(2025, 10, 20, 17, 30, 0, TimeSpan.FromHours(2)),
                new DateTimeOffset(2025, 10, 20, 19, 30, 0, TimeSpan.FromHours(2))
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyOnce_ShouldPass_WhenNoDatesAvailable()
        {
            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyOnceCheckbox = true,
                DaysOfWeek = new List<DayOfWeek>() { DayOfWeek.Sunday},
                WeeklyRecurrence = 1,
                StartDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                DailyOnceExecutionTime = new TimeSpan(13, 30, 0),
                EndDate = new DateTimeOffset(2025, 10, 18, 0, 0, 0, TimeSpan.Zero),
                Enabled = true
            };

            var onceCalc = new ScheduleManager();
            var result = onceCalc.GetNextExecution(taskConfig, 10);

            Assert.True(result.IsFailure, result.Error);
            Assert.Contains("No next execution", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ShouldPass_WhenUsingMaxOccurrences()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday };

            var taskConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Weekly,
                DailyFrequencyOnceCheckbox = true,
                DaysOfWeek = listOfDays,
                WeeklyRecurrence = 2,
                StartDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero),
                DailyOnceExecutionTime = new TimeSpan(13, 30, 0)
            };

            var onceCalc = new RecurringWeeklyOnceCalculator();

            var result = onceCalc.CalculateExecutions(taskConfig, 3);

            var expectedDates = new List<DateTimeOffset>
            {
                new DateTimeOffset(2025, 10, 27, 13, 30, 0, TimeSpan.Zero),
                new DateTimeOffset(2025, 10, 28, 13, 30, 0, TimeSpan.Zero),
                new DateTimeOffset(2025, 11, 10, 13, 30, 0, TimeSpan.Zero)
            };

            Assert.Equal(expectedDates, result);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenGetWeeklyDescription2Days()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday };

            var result = DescriptionGenerator.GetWeeklyDayList(listOfDays);

            Assert.Equal("monday and tuesday", result);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenGetWeeklyDescription0Days()
        {
            var listOfDays = new List<DayOfWeek>();

            var result = DescriptionGenerator.GetWeeklyDayList(listOfDays);

            Assert.Equal("no days specified", result);
        }
    }
}
