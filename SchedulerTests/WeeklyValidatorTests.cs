using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Services;



namespace MyScheduler
{
    public class WeeklyValidatorTests
    {
        //Common
        [Fact]
        public void WeeklyOnce_ShouldFail_WhenDaysOfWeekIsEmpty()
        {
            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.Type = Enums.Type.WeeklyOnce;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("DaysOfWeek must be selected for WeeklyOnce.", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenDaysOfWeekIsEmpty()
        {
            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.Type = Enums.Type.WeeklyEvery;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("DaysOfWeek must be selected for WeeklyEvery.", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenRecurrenceIsEmpty()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.Type = Enums.Type.WeeklyOnce;
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
            schedulerConfig.Type = Enums.Type.WeeklyEvery;
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


        // Once

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenExecutionTimeOfOneDayIsNull()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.Type = Enums.Type.WeeklyOnce;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.ExecutionTimeOfOneDay = null;

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("ExecutionTimeOfOneDay is required for WeeklyOnce.", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenDailyStartTimeSet()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.Type = Enums.Type.WeeklyOnce;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            schedulerConfig.ExecutionTimeOfOneDay = null;

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("DailyStartTime should not be set for WeeklyOnce.", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenDailyEndTimeSet()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.Type = Enums.Type.WeeklyOnce;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.ExecutionTimeOfOneDay = null;
            schedulerConfig.DailyEndTime = new TimeSpan(13, 30, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("DailyEndTime should not be set for WeeklyOnce.", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenTimeUnitSet()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.Type = Enums.Type.WeeklyOnce;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.ExecutionTimeOfOneDay = null;
            schedulerConfig.DailyEndTime = new TimeSpan(13, 30, 0);
            schedulerConfig.TimeUnit = TimeUnit.Hours;

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("TimeUnit should not be set for WeeklyOnce.", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ShouldFail_WhenTimeUnitNumberOfSet()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.Type = Enums.Type.WeeklyOnce;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.ExecutionTimeOfOneDay = null;
            schedulerConfig.DailyEndTime = new TimeSpan(13, 30, 0);
            schedulerConfig.TimeUnit = TimeUnit.Hours;
            schedulerConfig.TimeUnitNumberOf = 1;

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("TimeUnitNumberOf should not be set for WeeklyOnce.", result.Error);
        }

        //Every
        [Fact]
        public void WeeklyEvery_ShouldFail_WhenTimeUnitNotSet()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.Type = Enums.Type.WeeklyEvery;
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
            Assert.Contains("TimeUnit is required for WeeklyEvery.", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenTimeUnitNumberOf_NotSet()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.Type = Enums.Type.WeeklyEvery;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.DailyEndTime = new TimeSpan(13, 30, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("TimeUnitNumberOf must be a positive number for WeeklyEvery.", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenDailyStartTime_NotSet()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.Type = Enums.Type.WeeklyEvery;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.DailyEndTime = new TimeSpan(13, 30, 0);

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("DailyStartTime is required for WeeklyEvery.", result.Error);
        }


        [Fact]
        public void WeeklyEvery_ShouldFail_WhenDailyEndTime_NotSet()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.Type = Enums.Type.WeeklyEvery;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            
            

            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("DailyEndTime is required for WeeklyEvery.", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenExecutionOfOneDay_Set()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.Type = Enums.Type.WeeklyEvery;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);


            var schedulerManager = new ScheduleManager();
            var result = schedulerManager.GetNextExecution(schedulerConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("ExecutionTimeOfOneDay should not be set for WeeklyEvery.", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenStartTimeGreaterThanEndTime()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.Type = Enums.Type.WeeklyEvery;
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
            schedulerConfig.Type = Enums.Type.WeeklyEvery;
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
            schedulerConfig.Type = Enums.Type.WeeklyEvery;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            schedulerConfig.DailyEndTime = new TimeSpan(13, 50, 0);
            schedulerConfig.TimeUnit = TimeUnit.Hours;
            schedulerConfig.TimeUnitNumberOf = 2;

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
            schedulerConfig.Type = Enums.Type.WeeklyEvery;
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
            schedulerConfig.Type = Enums.Type.WeeklyEvery;
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
            Assert.Contains("TimeUnitNumberOf must be a positive number for WeeklyEvery.", result.Error);
        }


        //Working tests

        [Fact]
        public void WeeklyOnce_ShouldPass_WhenWorking()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var schedulerConfig = new ScheduleEntity();
            schedulerConfig.Type = Enums.Type.WeeklyOnce;
            schedulerConfig.DaysOfWeek = listOfDays;
            schedulerConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero);
            schedulerConfig.WeeklyRecurrence = 2;
            schedulerConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);


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
            schedulerConfig.Type = Enums.Type.WeeklyEvery;
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
            schedulerConfig.Type = Enums.Type.WeeklyEvery;
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
            schedulerConfig.Type = Enums.Type.WeeklyOnce;
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
            schedulerConfig.Type = Enums.Type.WeeklyOnce;
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
            schedulerConfig.Type = Enums.Type.WeeklyEvery;
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
    }
}
