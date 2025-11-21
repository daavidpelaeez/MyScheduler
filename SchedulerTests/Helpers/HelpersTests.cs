using System.Text;
using MyScheduler.Domain.Enums;
using MyScheduler.Domain.Validators;
using MyScheduler.Domain.Services;
using MyScheduler.Domain.Services.Calculators;
using MyScheduler.Domain.Entities;
using MyScheduler.Infraestructure.Localizer;

namespace MyScheduler.Helpers
{
    public class HelpersTests
    {
        [Fact]
        public void AddHourToList_AddsTimeToDates()
        {
            var schedule = new ScheduleEntity { DailyOnceExecutionTime = TimeSpan.FromHours(8) };
            var dates = new List<DateTimeOffset> { DateTimeOffset.Now.Date };
            var helper = new AddTime();
            var result = helper.AddHourToList(schedule, dates);
            Assert.Single(result);
            Assert.Equal(8, result[0].Hour);
        }

        [Fact]
        public void AddHourRangeToList_AddsRangeOfTimes()
        {
            var schedule = new ScheduleEntity {
                DailyStartTime = TimeSpan.FromHours(8),
                DailyEndTime = TimeSpan.FromHours(10),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                EndDate = DateTimeOffset.Now.AddDays(1)
            };
            var dates = new List<DateTimeOffset> { DateTimeOffset.Now.Date };
            var helper = new AddTime();
            var result = helper.AddHourRangeToList(schedule, 3, dates);
            Assert.Contains(result, d => d.Hour == 8);
            Assert.Contains(result, d => d.Hour == 9);
            Assert.Contains(result, d => d.Hour == 10);
        }

        [Fact]
        public void AddHourRangeToList_CoversCurrentTimeCondition()
        {
            var schedule = new ScheduleEntity {
                DailyStartTime = TimeSpan.FromHours(8),
                DailyEndTime = TimeSpan.FromHours(10),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                EndDate = DateTimeOffset.Now.AddDays(1),
                TimeZoneID = "W. Europe Standard Time"
            };
            var dates = new List<DateTimeOffset> { DateTimeOffset.Now.Date };
            var helper = new AddTime();
            var result = helper.AddHourRangeToList(schedule, 3, dates);
            // Debe cubrir: currentTime == startTime, currentTime < endTime, currentTime > endTime
            Assert.Contains(result, d => d.Hour == 8);
            Assert.Contains(result, d => d.Hour == 9);
            Assert.Contains(result, d => d.Hour == 10);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void AddHourRangeToList_DoesNotEnterLoop_WhenStartTimeGreaterThanEndTime()
        {
            var schedule = new ScheduleEntity {
                DailyStartTime = TimeSpan.FromHours(12),
                DailyEndTime = TimeSpan.FromHours(10),
                TimeUnit = TimeUnit.Hours,
                TimeUnitNumberOf = 1,
                EndDate = DateTimeOffset.Now.AddDays(1),
                TimeZoneID = "W. Europe Standard Time"
            };
            var dates = new List<DateTimeOffset> { DateTimeOffset.Now.Date };
            var helper = new AddTime();
            var result = helper.AddHourRangeToList(schedule, 3, dates);
            Assert.Empty(result);
        }

        [Fact]
        public void DailySchedulerHelper_GetRecurrentDays_ReturnsDays()
        {
            var schedule = new ScheduleEntity {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Daily,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.FromHours(8),
                StartDate = DateTimeOffset.Now.Date,
                EndDate = DateTimeOffset.Now.Date.AddDays(2),
                Recurrence = 1,
                CurrentDate = DateTimeOffset.Now.Date
            };
            var result = RecurrenceCalculator.GetRecurrentDays(schedule, null);
            Assert.True(result.Count >= 2);
        }


        [Fact]
        public void DescriptionGenerator_GetDescription_OnceType()
        {
            var schedule = new ScheduleEntity {
                ScheduleType = ScheduleType.Once,
                OnceTypeDateExecution = DateTimeOffset.Now,
                StartDate = DateTimeOffset.Now
            };
            var desc = DescriptionGenerator.GetDescription(schedule);
            Assert.Contains("Occurs once", desc);
        }

        [Fact]
        public void DescriptionGenerator_GetWeeklyDayList_MultipleDays()
        {
            var days = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetWeeklyDayList(days, localizer, "en-US");
            Assert.Contains("monday", result);
            Assert.Contains("friday", result);
        }

        [Fact]
        public void DescriptionGenerator_GetDescription_DefaultCase()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = (ScheduleType)999 // Valor inválido para forzar el default
            };
            var desc = DescriptionGenerator.GetDescription(schedule);
            Assert.Equal("Description not available for this task type.", desc);
        }

        [Fact]
        public void OutputHelper_OutputBuilder_ReturnsOutput()
        {
            var dt = DateTimeOffset.Now;
            var desc = "Test";
            var output = OutputHelper.OutputBuilder(dt, desc);
            Assert.Equal(dt, output.ExecutionTime);
            Assert.Equal(desc, output.Description);
        }

        [Fact]
        public void Result_SuccessAndFailure_Behavior()
        {
            var success = Result.Success();
            Assert.True(success.IsSuccess);
            Assert.False(success.IsFailure);
            Assert.Equal(string.Empty, success.Error);

            var failure = Result.Failure("error");
            Assert.True(failure.IsFailure);
            Assert.Equal("error", failure.Error);
        }

        [Fact]
        public void ResultT_SuccessAndFailure_Behavior()
        {
            var success = Result<string>.Success("ok");
            Assert.True(success.IsSuccess);
            Assert.Equal("ok", success.Value);
            var failure = Result<string>.Failure("fail");
            Assert.True(failure.IsFailure);
            Assert.Equal("fail", failure.Error);
            Assert.Throws<InvalidOperationException>(() => { var v = failure.Value; });
        }

        [Fact]
        public void WeeklyScheduleHelper_GetMatchingDays_ReturnsDays()
        {
            var schedule = new ScheduleEntity {
                StartDate = DateTimeOffset.Now.Date,
                EndDate = DateTimeOffset.Now.Date.AddDays(7),
                CurrentDate = DateTimeOffset.Now.Date,
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday },
                WeeklyRecurrence = 1
            };
            var result = WeeklyScheduleHelper.GetMatchingDays(schedule, 2);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void WeeklyScheduleHelper_SameWeekDayChecker_DetectsDuplicates()
        {
            var days = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Monday };
            Assert.True(WeeklyScheduleHelper.SameWeekDayChecker(days));
            var uniqueDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday };
            Assert.False(WeeklyScheduleHelper.SameWeekDayChecker(uniqueDays));
        }

        [Fact]
        public void WeeklyScheduleHelper_IntervalCalculator_ReturnsCorrectInterval()
        {
            var schedule = new ScheduleEntity { TimeUnit = TimeUnit.Minutes, TimeUnitNumberOf = 15 };
            var interval = WeeklyScheduleHelper.IntervalCalculator(schedule);
            Assert.Equal(TimeSpan.FromMinutes(15), interval);
        }

        public class TestResult : Result
        {
            public TestResult(bool isSuccess, string error) : base(isSuccess, error) { }
        }

        [Fact]
        public void Result_ThrowsException_WhenSuccessWithError()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                var r = new TestResult(true, "error");
            });
            Assert.Contains("Success cant have an error", ex.Message);
        }

        [Fact]
        public void Result_ThrowsException_WhenFailureWithoutError()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                var r = new TestResult(false, "");
            });
            Assert.Contains("Failure should have almost 1 error", ex.Message);
        }

        [Fact]
        public void DescriptionGenerator_GetRecurringDescription_DefaultCase()
        {
            var schedule = new ScheduleEntity
            {
                Occurs = (Occurs)999, 
                Language = "en-US"
            };
            var localizer = new Localizer();
            var desc = DescriptionGenerator.GetRecurringDescription(schedule, localizer, schedule.Language);
            Assert.Equal("Recurring schedule description not available.", desc);
        }

        [Fact]
        public void DailyFrequencyCommonRules_EmptyFrequency_AddsError()
        {
            var schedule = new ScheduleEntity
            {
                DailyFrequencyOnceCheckbox = false,
                DailyFrequencyRangeCheckbox = false
            };
            var errors = new StringBuilder();
            DailyFrequencyCommonRules.CheckDailyCommonRules(schedule, errors);
            var errorText = errors.ToString();
            Assert.Contains("Daily frequency cannot be empty for daily tasks", errorText);
        }

        [Fact]
        public void DailyFrequencyCommonRules_OnlyOnceCheckbox_AddsErrors()
        {
            var schedule = new ScheduleEntity
            {
                DailyFrequencyOnceCheckbox = true,
                DailyFrequencyRangeCheckbox = false,
                TimeUnitNumberOf = 1,
                TimeUnit = TimeUnit.Hours,
                DailyStartTime = TimeSpan.FromHours(8),
                DailyEndTime = TimeSpan.FromHours(10),
                DailyOnceExecutionTime = null,
                ScheduleType = ScheduleType.Recurring
            };
            var errors = new StringBuilder();
            DailyFrequencyCommonRules.CheckDailyCommonRules(schedule, errors);
            var errorText = errors.ToString();
            Assert.Contains("Daily tasks cannot have timeUnitNumberOf", errorText);
            Assert.Contains("Time unit cannot be set for a daily frequency once task", errorText);
            Assert.Contains("Daily start time cannot be set for a daily frequency once task", errorText);
            Assert.Contains("Daily end time cannot be set for a daily frequency once task", errorText);
            Assert.Contains("DailyOnceExecutionTime is required", errorText);
            Assert.DoesNotContain("You cannot set daily frequency once in a daily every task type", errorText);
        }

        [Fact]
        public void DailyFrequencyCommonRules_OnlyRangeCheckbox_AddsErrors()
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
            var errorText = errors.ToString();
            Assert.Contains("Execution time of day cannot be set for a daily frequency every task", errorText);
            Assert.Contains("You need to set a time unit for daily every configurations", errorText);
            Assert.Contains("You need to set a time unit number of greater than 0", errorText);
            Assert.Contains("You need to set a daily start time", errorText);
            Assert.Contains("You need to set a daily end time", errorText);
            Assert.DoesNotContain("You cannot set daily frequency every in a daily once task type", errorText);
        }

        [Fact]
        public void DailyFrequencyCommonRules_ValidRange_NoErrors()
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
    }
    public class DescriptionGeneratorTests
    {
        [Fact]
        public void GetDescription_Once_ReturnsExpected()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Once,
                OnceTypeDateExecution = new DateTimeOffset(2024, 6, 1, 10, 30, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2024, 5, 1, 0, 0, 0, TimeSpan.Zero)
            };
            var desc = DescriptionGenerator.GetDescription(schedule);
            Assert.Contains("Occurs once", desc);
            Assert.Contains("2024", desc);
        }

        [Fact]
        public void GetDescription_Recurring_ReturnsExpected()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Daily,
                DailyFrequencyOnceCheckbox = true,
                Recurrence = 1,
                DailyOnceExecutionTime = TimeSpan.FromHours(8),
                StartDate = new DateTimeOffset(2024, 5, 1, 0, 0, 0, TimeSpan.Zero)
            };
            var desc = DescriptionGenerator.GetDescription(schedule);
            Assert.Contains("Occurs every", desc);
        }

        [Fact]
        public void GetDescription_Default_ReturnsExpected()
        {
            var schedule = new ScheduleEntity { ScheduleType = (ScheduleType)999 };
            var desc = DescriptionGenerator.GetDescription(schedule);
            Assert.Equal("Description not available for this task type.", desc);
        }

        [Fact]
        public void GetRecurringDescription_Daily_OnceCheckbox()
        {
            var s = new ScheduleEntity
            {
                Occurs = Occurs.Daily,
                DailyFrequencyOnceCheckbox = true,
                Recurrence = 2,
                DailyOnceExecutionTime = TimeSpan.FromHours(9),
                StartDate = new DateTimeOffset(2024, 5, 2, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var desc = DescriptionGenerator.GetRecurringDescription(s, localizer, s.Language);
            Assert.Contains("Occurs every 2 day(s) at", desc);
        }

        [Fact]
        public void GetRecurringDescription_Daily_RangeCheckbox()
        {
            var s = new ScheduleEntity
            {
                Occurs = Occurs.Daily,
                DailyFrequencyRangeCheckbox = true,
                Recurrence = 3,
                TimeUnitNumberOf = 1,
                TimeUnit = TimeUnit.Hours,
                DailyStartTime = TimeSpan.FromHours(8),
                DailyEndTime = TimeSpan.FromHours(10),
                StartDate = new DateTimeOffset(2024, 5, 3, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var desc = DescriptionGenerator.GetRecurringDescription(s, localizer, s.Language);
            Assert.Contains("Occurs every 3 day(s) every 1 hours between", desc);
        }

        [Fact]
        public void GetRecurringDescription_Weekly_OnceCheckbox()
        {
            var s = new ScheduleEntity
            {
                Occurs = Occurs.Weekly,
                DailyFrequencyOnceCheckbox = true,
                WeeklyRecurrence = 2,
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday },
                DailyOnceExecutionTime = TimeSpan.FromHours(7),
                StartDate = new DateTimeOffset(2024, 5, 4, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var desc = DescriptionGenerator.GetRecurringDescription(s, localizer, s.Language);
            Assert.Contains("Occurs every 2 week(s) on", desc);
            Assert.Contains("at", desc);
        }

        [Fact]
        public void GetRecurringDescription_Weekly_RangeCheckbox()
        {
            var s = new ScheduleEntity
            {
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                WeeklyRecurrence = 1,
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Friday },
                TimeUnitNumberOf = 2,
                TimeUnit = TimeUnit.Minutes,
                DailyStartTime = TimeSpan.FromHours(6),
                DailyEndTime = TimeSpan.FromHours(8),
                StartDate = new DateTimeOffset(2024, 5, 5, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var desc = DescriptionGenerator.GetRecurringDescription(s, localizer, s.Language);
            Assert.Contains("Occurs every 1 week(s) on", desc);
            Assert.Contains("every 2 minutes between", desc);
        }

        [Fact]
        public void GetRecurringDescription_Monthly_DayCheckbox_OnceAndRange()
        {
            var s = new ScheduleEntity
            {
                Occurs = Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                MonthlyDayNumber = 15,
                MonthlyDayRecurrence = 2,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.FromHours(10),
                DailyFrequencyRangeCheckbox = false,
                StartDate = new DateTimeOffset(2024, 5, 6, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var desc = DescriptionGenerator.GetRecurringDescription(s, localizer, s.Language);
            Assert.Contains("Occurs day 15 every 2 month(s) at", desc);
            Assert.Contains(", starting", desc);
        }

        [Fact]
        public void GetRecurringDescription_Monthly_TheCheckbox_OnceAndRange()
        {
            var s = new ScheduleEntity
            {
                Occurs = Occurs.Monthly,
                MonthlyFrequencyTheCheckbox = true,
                MonthlyTheOrder = MonthlyTheOrder.Second,
                MonthlyTheDayOfWeek = MonthlyDayOfWeek.Friday,
                MonthlyTheRecurrence = 3,
                DailyFrequencyOnceCheckbox = false,
                DailyFrequencyRangeCheckbox = true,
                TimeUnitNumberOf = 5,
                TimeUnit = TimeUnit.Seconds,
                DailyStartTime = TimeSpan.FromHours(11),
                DailyEndTime = TimeSpan.FromHours(12),
                StartDate = new DateTimeOffset(2024, 5, 7, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var desc = DescriptionGenerator.GetRecurringDescription(s, localizer, s.Language);
            Assert.Contains("Occurs the second Friday of every 3 month(s) every 5 seconds between", desc);
            Assert.Contains(", starting", desc);
        }

        [Fact]
        public void GetRecurringDescription_Default_ReturnsExpected()
        {
            var s = new ScheduleEntity { Occurs = (Occurs)999, Language = "en-US" };
            var localizer = new Localizer();
            var desc = DescriptionGenerator.GetRecurringDescription(s, localizer, s.Language);
            Assert.Equal("Recurring schedule description not available.", desc);
        }

        [Fact]
        public void GetWeeklyDayList_NullOrEmpty_ReturnsNoDaysSpecified()
        {
            var localizer = new Localizer();
            Assert.Equal("no days specified", DescriptionGenerator.GetWeeklyDayList(new List<DayOfWeek>(), localizer, "en-US"));
        }

        [Fact]
        public void GetWeeklyDayList_OneDay_ReturnsDay()
        {
            var days = new List<DayOfWeek> { DayOfWeek.Tuesday };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetWeeklyDayList(days, localizer, "en-US");
            Assert.Equal("tuesday", result);
        }

        [Fact]
        public void GetWeeklyDayList_TwoDays_ReturnsAnd()
        {
            var days = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Friday };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetWeeklyDayList(days, localizer, "en-US");
            Assert.Equal("monday and friday", result);
        }

        [Fact]
        public void GetWeeklyDayList_MoreThanTwoDays_ReturnsCommaAndAnd()
        {
            var days = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetWeeklyDayList(days, localizer, "en-US");
            Assert.Equal("monday, wednesday and friday", result);
        }
    }
}
