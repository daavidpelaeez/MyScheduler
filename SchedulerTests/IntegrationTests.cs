using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Helpers;
using MyScheduler.Services;

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
        public void DailyOnce_HappyPath_ReturnsExpectedOutput()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.DailyOnce;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 28, 0, 0, 0, TimeSpan.Zero);
            config.DailyFrequencyOnce = true;
            config.Recurrence = 2;
            config.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsSuccess);
            var expectedTime = new DateTimeOffset(2025, 10, 29, 13, 30, 0, TimeSpan.Zero);
            var expectedDescription = "Occurs every 2 day(s). Next on 29/10/2025, starting 25/10/2025";

            Assert.Equal(expectedTime, result.Value.ExecutionTime);
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Fact]
        public void DailyEvery_HappyPath_ReturnsExpectedOutput()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.DailyEvery;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.DailyFrequencyEvery = true;
            config.Recurrence = 2;
            config.TimeUnit = TimeUnit.Hours;
            config.TimeUnitNumberOf = 2;
            config.DailyStartTime = new TimeSpan(13, 30, 0);
            config.DailyEndTime = new TimeSpan(15, 30, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsSuccess);
            var expectedTime = new DateTimeOffset(2025, 10, 25, 13, 30, 0, TimeSpan.Zero);
            var expectedDescription = "Occurs every 2 day(s) from 13:30:00 to 15:30:00 every 2 hours ";

            Assert.Equal(expectedTime, result.Value.ExecutionTime);
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Fact]
        public void WeeklyOnce_HappyPath_ReturnsExpectedOutput()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.WeeklyOnce;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.DailyFrequencyOnce = true;
            config.WeeklyRecurrence = 1;
            config.DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday };
            config.ExecutionTimeOfOneDay = new TimeSpan(10, 0, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 5);

            Assert.True(result.IsSuccess);
            var expectedTime = new DateTimeOffset(2025, 10, 27, 10, 0, 0, TimeSpan.Zero);
            var expectedDescription = "Occurs every 1 weeks on monday and wednesday at 10:00:00, starting 25/10/2025";

            Assert.Equal(expectedTime, result.Value.ExecutionTime);
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Fact]
        public void WeeklyEvery_HappyPath_ReturnsExpectedOutput()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.WeeklyEvery;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.WeeklyRecurrence = 1;
            config.DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday };
            config.DailyFrequencyEvery = true;
            config.DailyStartTime = new TimeSpan(9, 0, 0);
            config.DailyEndTime = new TimeSpan(12, 0, 0);
            config.TimeUnit = TimeUnit.Hours;
            config.TimeUnitNumberOf = 2;

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 5);

            Assert.True(result.IsSuccess, result.Error);
            var expectedTime = new DateTimeOffset(2025, 10, 27, 9, 0, 0, TimeSpan.Zero);
            var expectedDescription = "Occurs every 1 weeks on monday every 2 hours between 09:00:00 and 12:00:00 starting on 25/10/2025";

            Assert.Equal(expectedTime, result.Value.ExecutionTime);
            Assert.Equal(expectedDescription, result.Value.Description);
        }


        [Fact]
        public void DailyOnce_CurrentDateEqualsStart_ReturnsExpectedOutput()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.DailyOnce;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.DailyFrequencyOnce = true;
            config.Recurrence = 1;
            config.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsSuccess);
            var expectedTime = new DateTimeOffset(2025, 10, 25, 13, 30, 0, TimeSpan.Zero);
            Assert.Equal(expectedTime, result.Value.ExecutionTime);
        }

        [Fact]
        public void DailyOnce_CurrentDateAfterStart_ReturnsExpectedOutput()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.DailyOnce;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 27, 0, 0, 0, TimeSpan.Zero);
            config.DailyFrequencyOnce = true;
            config.Recurrence = 2;
            config.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsSuccess);
            var expectedTime = new DateTimeOffset(2025, 10, 27, 13, 30, 0, TimeSpan.Zero);
            Assert.Equal(expectedTime, result.Value.ExecutionTime);
        }

        [Fact]
        public void WeeklyOnce_OneDayDescription_OutputDescriptionMatchesExpected()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.WeeklyOnce;
            config.DailyFrequencyOnce = true;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.WeeklyRecurrence = 1;
            config.DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday };
            config.ExecutionTimeOfOneDay = new TimeSpan(10, 0, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 3);

            Assert.True(result.IsSuccess);
            var expectedDescription = "Occurs every 1 weeks on monday at 10:00:00, starting 25/10/2025";
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Fact]
        public void WeeklyOnce_TwoDaysDescription_OutputDescriptionMatchesExpected()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.WeeklyOnce;
            config.DailyFrequencyOnce = true;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.WeeklyRecurrence = 1;
            config.DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday };
            config.ExecutionTimeOfOneDay = new TimeSpan(10, 0, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 3);

            Assert.True(result.IsSuccess);
            var expectedDescription = "Occurs every 1 weeks on monday and wednesday at 10:00:00, starting 25/10/2025";
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Fact]
        public void WeeklyOnce_ThreePlusDaysDescription_OutputDescriptionMatchesExpected()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.WeeklyOnce;
            config.DailyFrequencyOnce = true;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.WeeklyRecurrence = 1;
            config.DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday };
            config.ExecutionTimeOfOneDay = new TimeSpan(10, 0, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 5);

            Assert.True(result.IsSuccess);
            var expectedDescription = "Occurs every 1 weeks on monday, wednesday and friday at 10:00:00, starting 25/10/2025";
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Fact]
        public void DailyOnce_MissingExecutionTime_ReturnsFailureWithExpectedErrorMessage()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.DailyOnce;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.DailyFrequencyOnce = true;
            config.Recurrence = 1;
            config.ExecutionTimeOfOneDay = null;

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("ExecutionTimeOfOneDay is required", result.Error);
        }

        [Fact]
        public void DailyFrequency_BothTrue_ReturnsFailureWithExpectedErrorMessage()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.DailyOnce;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.DailyFrequencyOnce = true;
            config.DailyFrequencyEvery = true;
            config.Recurrence = 1;
            config.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("Daily frequency cannot be once and every at the same time", result.Error);
        }

        [Fact]
        public void DailyEvery_MissingTimeUnit_ReturnsFailureWithExpectedErrorMessage()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.DailyEvery;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.DailyFrequencyEvery = true;
            config.Recurrence = 1;
            config.TimeUnit = null;
            config.TimeUnitNumberOf = 2;
            config.DailyStartTime = new TimeSpan(9, 0, 0);
            config.DailyEndTime = new TimeSpan(12, 0, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("You need to set a time unit for daily every configurations", result.Error);
        }

        [Fact]
        public void WeeklyOnce_MissingDaysOfWeek_ReturnsFailureWithExpectedErrorMessage()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.WeeklyOnce;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.WeeklyRecurrence = 1;
            config.DaysOfWeek = new List<DayOfWeek>();
            config.ExecutionTimeOfOneDay = new TimeSpan(10, 0, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 5);

            Assert.True(result.IsFailure);
            Assert.Contains("DaysOfWeek must be selected for WeeklyOnce.", result.Error);
        }

        [Fact]
        public void EndDateBeforeCurrentDate_ReturnsFailureWithExpectedErrorMessage()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.DailyOnce;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 28, 0, 0, 0, TimeSpan.Zero);
            config.EndDate = new DateTimeOffset(2025, 10, 27, 0, 0, 0, TimeSpan.Zero);
            config.DailyFrequencyOnce = true;
            config.Recurrence = 1;
            config.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("The end date of a recurring scheduleConfig must be after the current date.", result.Error);
        }

        [Fact]
        public void DisabledSchedule_ReturnsFailure()
        {
            var config = new ScheduleEntity();
            config.Enabled = false; 
            config.ScheduleType = ScheduleType.DailyOnce;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 26, 0, 0, 0, TimeSpan.Zero);
            config.Recurrence = 1;
            config.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);
            config.DailyFrequencyOnce = true;

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("The form its not enabled, check enable checkbox", result.Error);
        }

        [Fact]
        public void WeeklyEvery_MultipleDaysAndHours_ReturnsExpectedNextExecution()
        {
            var config = new ScheduleEntity();
            config.Enabled = true;
            config.ScheduleType = ScheduleType.WeeklyEvery;
            config.StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.CurrentDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);
            config.WeeklyRecurrence = 1;
            config.DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday };
            config.DailyFrequencyEvery = true;
            config.DailyStartTime = new TimeSpan(9, 0, 0);
            config.DailyEndTime = new TimeSpan(12, 0, 0);
            config.TimeUnit = TimeUnit.Hours;
            config.TimeUnitNumberOf = 3;

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 5);

            Assert.True(result.IsSuccess, result.Error);
            
            var expectedTime = new DateTimeOffset(2025, 10, 27, 9, 0, 0, TimeSpan.Zero);
            Assert.Equal(expectedTime, result.Value.ExecutionTime);
        }

        [Fact]
        public void DailyOnce_EndOfMonthAndLeapYear_ReturnsExpectedNextExecution()
        {
            // Fin de mes
            var config1 = new ScheduleEntity
            {
                Enabled = true,
                ScheduleType = ScheduleType.DailyOnce,
                StartDate = new DateTimeOffset(2024, 2, 28, 0, 0, 0, TimeSpan.Zero), // Leap year
                CurrentDate = new DateTimeOffset(2024, 2, 28, 0, 0, 0, TimeSpan.Zero),
                Recurrence = 1,
                DailyFrequencyOnce = true,
                ExecutionTimeOfOneDay = new TimeSpan(10, 0, 0)
            };

            var manager = new ScheduleManager();
            var result1 = manager.GetNextExecution(config1, 1);
            Assert.True(result1.IsSuccess);
            var expectedLeapTime = new DateTimeOffset(2024, 2, 28, 10, 0, 0, TimeSpan.Zero);
            Assert.Equal(expectedLeapTime, result1.Value.ExecutionTime);

            // Fin de año
            var config2 = new ScheduleEntity
            {
                Enabled = true,
                ScheduleType = ScheduleType.DailyOnce,
                StartDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero),
                Recurrence = 1,
                DailyFrequencyOnce = true,
                ExecutionTimeOfOneDay = new TimeSpan(23, 59, 0)
            };

            var result2 = manager.GetNextExecution(config2, 1);
            Assert.True(result2.IsSuccess);
            var expectedEndYear = new DateTimeOffset(2025, 12, 31, 23, 59, 0, TimeSpan.Zero);
            Assert.Equal(expectedEndYear, result2.Value.ExecutionTime);
        }

        [Fact]
        public void DailyOnce_InvalidRecurrence_ReturnsFailure()
        {
            var config = new ScheduleEntity
            {
                Enabled = true,
                ScheduleType = ScheduleType.DailyOnce,
                StartDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero),
                CurrentDate = new DateTimeOffset(2025, 10, 26, 0, 0, 0, TimeSpan.Zero),
                Recurrence = 0, 
                DailyFrequencyOnce = true,
                ExecutionTimeOfOneDay = new TimeSpan(10, 0, 0)
            };

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(config, 5);

            Assert.True(result.IsFailure);
            Assert.Contains("DailyOnce tasks must have a recurrence greater than 0.", result.Error);
        }

    }
}
