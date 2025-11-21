
using MyScheduler.Application.Services;
using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Enums;

using MyScheduler.Helpers;
using MyScheduler.Infraestructure.Localizer;


namespace MyScheduler.Weekly
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
            string expectedDescription = "Occurs every 4 week(s) on monday, tuesday and sunday at 13:30, starting 16/10/2025";

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
            var result = manager.GetOutput(taskConfig, 10);

            string expectedDescription = "Occurs every 4 week(s) on monday, tuesday and sunday at 13:30, starting 16/10/2025";
            DateTimeOffset expectedExecutionTime = new DateTimeOffset(2025, 10, 19, 13, 30, 0, TimeSpan.FromHours(2));

            Assert.Equal(expectedDescription, result.Value.Description);
            Assert.Equal(expectedExecutionTime, result.Value.ExecutionTime);
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
            var result = manager.GetOutput(taskConfig, 10);

            string expectedDescription = "Occurs every 1 week(s) on monday every 2 hours between 13:30 and 19:30, starting 17/10/2025";
            DateTimeOffset expectedExecutionTime = new DateTimeOffset(2025, 10, 20, 13, 30, 0, TimeSpan.FromHours(2));

            Assert.Equal(expectedDescription, result.Value.Description);
            Assert.Equal(expectedExecutionTime, result.Value.ExecutionTime);
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
            var result = manager.GetOutput(taskConfig, 10);

            Assert.True(result.IsFailure, result.Error);
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
            var result = onceCalc.GetOutput(taskConfig, 10);

            Assert.True(result.IsFailure, result.Error);
            Assert.Contains("No next execution", result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenGetWeeklyDescription2Days()
        {
            var listOfDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetWeeklyDayList(listOfDays, localizer, "en-US");
            Assert.Equal("monday and tuesday", result);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenGetWeeklyDescription0Days()
        {
            var listOfDays = new List<DayOfWeek>();
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetWeeklyDayList(listOfDays, localizer, "en-US");
            Assert.Equal("no days specified", result);
        }
    }
}
