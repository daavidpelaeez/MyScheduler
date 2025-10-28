using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Services;
using MyScheduler.Validators;
using System.Text;



namespace MyScheduler
{
    public class WeeklyValidatorTests
    {
        //Common
        [Fact]
        public void WeeklyOnce_ShouldFail_WhenDaysOfWeekIsEmpty()
        {
            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyOnce;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("DaysOfWeek must be selected for RecurringWeeklyOnce.", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenDaysOfWeekIsEmpty()
        {
            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("DaysOfWeek must be selected for RecurringWeeklyRange.", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenRecurrenceIsEmpty()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyOnce;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("WeeklyRecurrence must be at least 1.", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenRecurrenceIsEmpty()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("WeeklyRecurrence must be at least 1.", result.Error);
        }


        // OneTime

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenExecutionTimeOfOneDayIsNull()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyOnce;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.ExecutionTimeOfOneDay = null;
            schedulerConfig.DailyFrequencyOnce = true;
            schedulerConfig.ExecutionTimeOfOneDay = null;
            schedulerConfig.Enabled = true;

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("ExecutionTimeOfOneDay is required", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenDailyStartTimeSet()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyOnce;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            schedulerConfig.ExecutionTimeOfOneDay = null;
            schedulerConfig.DailyFrequencyOnce = true;
            schedulerConfig.ExecutionTimeOfOneDay = new TimeSpan(13,30,0);
            schedulerConfig.Enabled = true;

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("Daily start time cannot be set for a daily frequency one task", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenDailyEndTimeSet()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyOnce;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 21, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 1;
            schedulerConfig.DailyFrequencyOnce = true;
            schedulerConfig.DailyEndTime = new TimeSpan(13, 30, 0);
            schedulerConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);
            schedulerConfig.Enabled = true;

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("Daily end time cannot be set for a daily frequency one task", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenTimeUnitSet()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyOnce;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.ExecutionTimeOfOneDay = null;
            schedulerConfig.DailyEndTime = new TimeSpan(13, 30, 0);
            schedulerConfig.TimeUnit = TimeUnit.Hours;
            schedulerConfig.DailyFrequencyOnce = true;
            schedulerConfig.Enabled = true;

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("Time unit cannot be set for a daily frequency one task", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenTimeUnitNumberOfSet()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyOnce;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.ExecutionTimeOfOneDay = null;
            schedulerConfig.DailyEndTime = new TimeSpan(13, 30, 0);
            schedulerConfig.TimeUnit = TimeUnit.Hours;
            schedulerConfig.TimeUnitNumberOf = 1;
            schedulerConfig.Enabled = true;
            schedulerConfig.DailyFrequencyOnce = true;

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("Time unit cannot be set for a daily frequency one task", result.Error);
        }

        //Every
        [Fact]
        public void WeeklyEvery_ShouldFail_WhenTimeUnitNotSet()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.DailyEndTime = new TimeSpan(13, 30, 0);
            schedulerConfig.TimeUnitNumberOf = 1;

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("TimeUnit is required for RecurringWeeklyRange.", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenTimeUnitNumberOf_NotSet()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.DailyEndTime = new TimeSpan(13, 30, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("TimeUnitNumberOf must be a positive number for RecurringWeeklyRange.", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenDailyStartTime_NotSet()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.DailyEndTime = new TimeSpan(13, 30, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("DailyStartTime is required for RecurringWeeklyRange.", result.Error);
        }


        [Fact]
        public void WeeklyEvery_ShouldFail_WhenDailyEndTime_NotSet()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            
            

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("DailyEndTime is required for RecurringWeeklyRange.", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenExecutionOfOneDay_Set()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.DailyFrequencyEvery = true;
            schedulerConfig.TimeUnit = TimeUnit.Hours;
            schedulerConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);
            schedulerConfig.TimeUnitNumberOf = 2;
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.DailyStartTime = new TimeSpan(13, 30, 0); 
            schedulerConfig.DailyEndTime = new TimeSpan(17, 30, 0);
            schedulerConfig.Enabled = true;

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("Execution time of day cannot be set for a daily frequency every task", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenStartTimeGreaterThanEndTime()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.DailyStartTime = new TimeSpan(13,30,0);
            schedulerConfig.DailyEndTime = new TimeSpan(12,30,0);


            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("DailyStartTime cannot be after the DailyEndTime.", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenStartTimeIsSameAsEndTime()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            schedulerConfig.DailyEndTime = new TimeSpan(13, 30, 0);


            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("DailyStartTime cannot be the same as DailyEndTime.", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenNoExecutionsAvalaible()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.DailyFrequencyEvery = true;
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            schedulerConfig.DailyEndTime = new TimeSpan(13, 50, 0);
            schedulerConfig.TimeUnit = TimeUnit.Hours;
            schedulerConfig.TimeUnitNumberOf = 2;
            schedulerConfig.Enabled = true;

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("No next execution avaliable in that range", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenWeeklyRecurrenceIsZero()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 0;
            schedulerConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            schedulerConfig.DailyEndTime = new TimeSpan(13, 50, 0);
            schedulerConfig.TimeUnit = TimeUnit.Hours;
            schedulerConfig.TimeUnitNumberOf = 2;

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("WeeklyRecurrence must be at least 1.", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenTimeUnitNumberOf_IsZero()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            schedulerConfig.DailyEndTime = new TimeSpan(13, 50, 0);
            schedulerConfig.TimeUnit = TimeUnit.Hours;
            schedulerConfig.TimeUnitNumberOf = 0;

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("TimeUnitNumberOf must be a positive number for RecurringWeeklyRange.", result.Error);
        }


        //Working tests

        [Fact]
        public void WeeklyOnce_ShouldPass_WhenWorking()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyOnce;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.DailyFrequencyOnce = true;
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);
            schedulerConfig.Enabled = true;


            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsSuccess);
            Assert.Equal(new DateTimeOffset(2025,10,27,13,30,0,TimeSpan.Zero),result.Value.ExecutionTime);
            Assert.Equal("Occurs every 2 weeks on monday at 13:30:00, starting 15/10/2025", result.Value.Description);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenWorking()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.DailyFrequencyEvery = true;
            schedulerConfig.TimeUnit = TimeUnit.Hours;
            schedulerConfig.TimeUnitNumberOf = 1;
            schedulerConfig.DailyStartTime = new TimeSpan(13, 0, 0);
            schedulerConfig.DailyEndTime = new TimeSpan(17, 0, 0);
            schedulerConfig.Enabled = true;

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.True(result.IsSuccess, result.Error);
            Assert.Equal(new DateTimeOffset(2025, 10, 27, 13, 0, 0, TimeSpan.Zero), result.Value.ExecutionTime);
        }

        //dias repetidos

        [Fact]

        public void WeeklyEvery_ShouldFail_WhenSameWeekDay()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.TimeUnit = TimeUnit.Hours;
            schedulerConfig.TimeUnitNumberOf = 1;
            schedulerConfig.DailyStartTime = new TimeSpan(13, 0, 0);
            schedulerConfig.DailyEndTime = new TimeSpan(17, 0, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.Contains("Check days of the week they cant be repeated",result.Error);
        }


        [Fact]

        public void WeeklyOnce_ShouldFail_WhenSameWeekDay()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyOnce;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);
          

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.Contains("Check days of the week they cant be repeated", result.Error);
        }


        [Fact]

        public void WeeklyEvery_ShouldFail_WhenSimpleOccurrenceLowerThanTimeSpanZero()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);
            

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyOnce;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.ExecutionTimeOfOneDay = new TimeSpan(-1, 0, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.Contains("ExecutionTimeOfOneDay is wrong", result.Error);
            
        }


        [Fact]

        public void WeeklyEvery_ShouldFail_WhenNoEndateAndNoOccurrences()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.TimeUnit = TimeUnit.Hours;
            schedulerConfig.TimeUnitNumberOf = 1;
            schedulerConfig.DailyStartTime = new TimeSpan(13, 0, 0);
            schedulerConfig.DailyEndTime = new TimeSpan(17, 0, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, null);

            Assert.Contains("You must specified end date or num occurrences", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenStartDateLessThanMinValue()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = DateTimeOffset.MinValue;
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.TimeUnit = TimeUnit.Hours;
            schedulerConfig.TimeUnitNumberOf = 1;
            schedulerConfig.DailyStartTime = new TimeSpan(13, 0, 0);
            schedulerConfig.DailyEndTime = new TimeSpan(17, 0, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.Contains("StartDate cannot be less or equal than Min value", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenStartDateMoreThanMaxValue()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = DateTimeOffset.MaxValue;
            
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.TimeUnit = TimeUnit.Hours;
            schedulerConfig.TimeUnitNumberOf = 1;
            schedulerConfig.DailyStartTime = new TimeSpan(13, 0, 0);
            schedulerConfig.DailyEndTime = new TimeSpan(17, 0, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.Contains("StartDate cannot be more or equal than Max value", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenEndDateMoreThanMaxValue()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = DateTimeOffset.MaxValue;
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.TimeUnit = TimeUnit.Hours;
            schedulerConfig.TimeUnitNumberOf = 1;
            schedulerConfig.DailyStartTime = new TimeSpan(13, 0, 0);
            schedulerConfig.DailyEndTime = new TimeSpan(17, 0, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);

            Assert.Contains("EndDate cannot be more or equal than Max value.", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenNoEndDateAndNoOccurrences()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.TimeUnit = TimeUnit.Hours;
            schedulerConfig.TimeUnitNumberOf = 1;
            schedulerConfig.DailyStartTime = new TimeSpan(13, 0, 0);
            schedulerConfig.DailyEndTime = new TimeSpan(17, 0, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, -1);

            Assert.Contains("You must specified end date or num occurrences", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenEndDateBeforeCurrentDate()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.ScheduleType = Enums.ScheduleType.RecurringWeeklyRange;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025,10,14,0,0,0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.TimeUnit = TimeUnit.Hours;
            schedulerConfig.TimeUnitNumberOf = 1;
            schedulerConfig.DailyStartTime = new TimeSpan(13, 0, 0);
            schedulerConfig.DailyEndTime = new TimeSpan(17, 0, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, -1);

            Assert.Contains("The end date of a recurring scheduleConfig must be after the current date.", result.Error);
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
