using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Helpers;
using MyScheduler.ScheduleCalculators;
using MyScheduler.Services;




namespace MyScheduler
{
    public class WeeklyCalculatorTests
    {
        //Once
        //
        [Fact]
        public void WeeklyOnce_ShouldPass_WhenCheckingCorrectDescription()
        {
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Tuesday);
            listOfDays.Add(DayOfWeek.Sunday);

            var taskManager = new ScheduleManager();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyOnce;
            taskConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 4;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero);

            var result = DescriptionGenerator.GetDescription(taskConfig);
            String expectedDescription = $"Occurs every 4 weeks on monday, tuesday and sunday " +
            "at 13:30:00, starting 16/10/2025";

            Assert.Equal(expectedDescription, result);
        }

        [Fact]
        public void WeeklyOnce_ShouldPass_WhenCheckingCorrectOutput()
        {
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Tuesday);
            listOfDays.Add(DayOfWeek.Sunday);

            var taskManager = new ScheduleManager();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyOnce;
            taskConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 4;
            taskConfig.DailyFrequencyOnce = true;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025,10,31,0,0,0, 0, TimeSpan.Zero);
            taskConfig.Enabled = true;

            var result = taskManager.GetNextExecution(taskConfig,10);

            String expectedDescription = $"Occurs every 4 weeks on monday, tuesday and sunday " +
            "at 13:30:00, starting 16/10/2025";

            DateTimeOffset expectedExecutionTime = new DateTimeOffset(2025, 10, 19,13,30,0,TimeSpan.Zero);

            Assert.Equal(expectedDescription, result.Value.Description);
            Assert.Equal(expectedExecutionTime, result.Value.ExecutionTime);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenCheckingCorrectOuput()
        {
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);

            var taskManager = new ScheduleManager();
            var weManager = new WeeklyEveryCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyEvery;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 1;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 21, 0, 0, 0, TimeSpan.Zero);
            taskConfig.DailyFrequencyEvery = true;
            taskConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            taskConfig.DailyEndTime = new TimeSpan(19, 30, 0);
            taskConfig.TimeUnit = TimeUnit.Hours;
            taskConfig.TimeUnitNumberOf = 2;
            taskConfig.Enabled = true;

            var result = taskManager.GetNextExecution(taskConfig, 10);

            String expectedDescription = $"Occurs every 1 weeks on monday every 2 hours between 13:30:00 and 19:30:00 starting on 17/10/2025";
           

            DateTimeOffset expectedExecutionTime = new DateTimeOffset(2025, 10, 20, 13, 30, 0, TimeSpan.Zero);

            Assert.Equal(expectedDescription,result.Value.Description);
            Assert.Equal(expectedExecutionTime, result.Value.ExecutionTime);
        }

        [Fact]
        public void WeeklyOnce_ShouldPass_WhenUsingOccurrences()
        {
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Tuesday);
            listOfDays.Add(DayOfWeek.Sunday);

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyOnce;
            taskConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 3;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
           
            WeeklyOnceCalculator weeklyOnceCalculator = new WeeklyOnceCalculator();

            var result = weeklyOnceCalculator.CalculateWeeklyOnceConfig(taskConfig, 5);

            var expectedExecutions = new List<DateTimeOffset>();

            expectedExecutions.Add(new DateTimeOffset(2025, 10, 19, 13, 30, 0, TimeSpan.Zero));
            expectedExecutions.Add(new DateTimeOffset(2025, 11, 3, 13, 30, 0, TimeSpan.Zero));
            expectedExecutions.Add(new DateTimeOffset(2025, 11, 4, 13, 30, 0, TimeSpan.Zero));
            expectedExecutions.Add(new DateTimeOffset(2025, 11, 9, 13, 30, 0, TimeSpan.Zero));
            expectedExecutions.Add(new DateTimeOffset(2025, 11, 24, 13, 30, 0, TimeSpan.Zero));

            Assert.Equal(expectedExecutions, result);
            
        }

        //WeeklyEvery
        [Fact]
        public void WeeklyEvery_ShouldPass_WhenCalculateConfig()
        {
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Tuesday);
            listOfDays.Add(DayOfWeek.Sunday);

            var taskManager = new ScheduleManager();
            var weManager = new WeeklyEveryCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyEvery;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 4;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 19, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 19, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, 0, TimeSpan.Zero);
            taskConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            taskConfig.DailyEndTime = new TimeSpan(19, 30, 0);
            taskConfig.TimeUnit = TimeUnit.Hours;
            taskConfig.TimeUnitNumberOf = 2;

            var result = weManager.CalculateWeeklyRecurringConfig(taskConfig,10);

            List<DateTimeOffset> expected = new List<DateTimeOffset>();

            expected.Add(new DateTimeOffset(2025, 10, 19, 13, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 19, 15, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 19, 17, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 19, 19, 30, 0, TimeSpan.Zero));

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenJumpIs45MinutesStartingAtHalfHour()
        {
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Tuesday);
            listOfDays.Add(DayOfWeek.Sunday);

            var taskManager = new ScheduleManager();
            var weManager = new WeeklyEveryCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyOnce;
            taskConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 4;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, 0, TimeSpan.Zero);
            taskConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            taskConfig.DailyEndTime = new TimeSpan(15, 30, 0);
            taskConfig.TimeUnit = TimeUnit.Minutes;
            taskConfig.TimeUnitNumberOf = 45;

            var result = weManager.CalculateWeeklyRecurringConfig(taskConfig, 10);

            List<DateTimeOffset> expected = new List<DateTimeOffset>();

            expected.Add(new DateTimeOffset(2025, 10, 19, 13, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 19, 14, 15, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 19, 15, 0, 0, TimeSpan.Zero));
            

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenJumpIs27Seconds()
        {
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Tuesday);
            listOfDays.Add(DayOfWeek.Sunday);

            var taskManager = new ScheduleManager();
            var weManager = new WeeklyEveryCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyEvery;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 4;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 16, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, 0, TimeSpan.Zero);
            taskConfig.DailyStartTime = new TimeSpan(13, 59, 0);
            taskConfig.DailyEndTime = new TimeSpan(14, 0, 0);
            taskConfig.TimeUnit = TimeUnit.Seconds;
            taskConfig.TimeUnitNumberOf = 27;

            var result = weManager.CalculateWeeklyRecurringConfig(taskConfig, 10);

            List<DateTimeOffset> expected = new List<DateTimeOffset>();

            expected.Add(new DateTimeOffset(2025, 10, 19, 13, 59, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 19, 13, 59, 27, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 19, 13, 59, 54, TimeSpan.Zero));

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyEvery_ShouldSkipTodayAndTakeNextDay()
        {
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);

            var taskManager = new ScheduleManager();
            var weManager = new WeeklyEveryCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyEvery;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 2;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 20, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, 0, TimeSpan.Zero);
            taskConfig.DailyStartTime = new TimeSpan(16, 0, 0);
            taskConfig.DailyEndTime = new TimeSpan(20, 0, 0);
            taskConfig.TimeUnit = TimeUnit.Hours;
            taskConfig.TimeUnitNumberOf = 2;

            var result = weManager.CalculateWeeklyRecurringConfig(taskConfig, 10);

            List<DateTimeOffset> expected = new List<DateTimeOffset>();

            expected.Add(new DateTimeOffset(2025, 10, 20, 16, 0, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 20, 18, 0, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 20, 20, 0, 0, TimeSpan.Zero));

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenCalculateUsingOccurrences()
        {
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Tuesday);
            listOfDays.Add(DayOfWeek.Sunday);

            var taskManager = new ScheduleManager();
            var weManager = new WeeklyEveryCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyEvery;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 4;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 19, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 19, 0, 0, 0, TimeSpan.Zero);
            taskConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            taskConfig.DailyEndTime = new TimeSpan(19, 30, 0);
            taskConfig.TimeUnit = TimeUnit.Hours;
            taskConfig.TimeUnitNumberOf = 2;

            var result = weManager.CalculateWeeklyRecurringConfig(taskConfig, 2);

            List<DateTimeOffset> expected = new List<DateTimeOffset>();

            expected.Add(new DateTimeOffset(2025, 10, 19, 13, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 19, 15, 30, 0, TimeSpan.Zero));


            Assert.Equal(expected, result);
        }

        //Common
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
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Tuesday);
            listOfDays.Add(DayOfWeek.Sunday);

            var taskManager = new ScheduleManager();
            var weManager = new WeeklyEveryCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyEvery;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 4;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 19, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 13, 0, 0, 0, TimeSpan.Zero);
            taskConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            taskConfig.DailyEndTime = new TimeSpan(19, 30, 0);
            taskConfig.TimeUnit = TimeUnit.Hours;
            taskConfig.TimeUnitNumberOf = 2;

            var result = weManager.CalculateWeeklyRecurringConfig(taskConfig, 2);

            List<DateTimeOffset> expected = new List<DateTimeOffset>();

            expected.Add(new DateTimeOffset(2025, 10, 19, 13, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 19, 15, 30, 0, TimeSpan.Zero));


            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyEvery_ShouldFail_WhenCurrentDateIsAfterEndDate()
        {
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Tuesday);
            listOfDays.Add(DayOfWeek.Sunday);

            var taskManager = new ScheduleManager();
            var weManager = new WeeklyEveryCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyEvery;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 4;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 19, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 20, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 19, 0, 0, 0, TimeSpan.Zero);
            taskConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            taskConfig.DailyEndTime = new TimeSpan(19, 30, 0);
            taskConfig.TimeUnit = TimeUnit.Hours;
            taskConfig.TimeUnitNumberOf = 2;

            var result = taskManager.GetNextExecution(taskConfig, 10);


            Assert.True(result.IsFailure,result.Error);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenDatesAtTheEndOfYearWithOccurrences()
        {
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Tuesday);
            listOfDays.Add(DayOfWeek.Sunday);

            var taskManager = new ScheduleManager();
            var weManager = new WeeklyEveryCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyEvery;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 4;
            taskConfig.StartDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            taskConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            taskConfig.DailyEndTime = new TimeSpan(19, 30, 0);
            taskConfig.TimeUnit = TimeUnit.Hours;
            taskConfig.TimeUnitNumberOf = 2;

            var result = weManager.CalculateWeeklyRecurringConfig(taskConfig, 5);

            List<DateTimeOffset> expected = new List<DateTimeOffset>();

            expected.Add(new DateTimeOffset(2026, 1, 4, 13, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2026, 1, 4, 15, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2026, 1, 4, 17, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2026, 1, 4, 19, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2026, 1, 26, 13, 30, 0, TimeSpan.Zero));


            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenDatesAtTheEndOfYearWithEndDate()
        {
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Tuesday);
            listOfDays.Add(DayOfWeek.Sunday);

            var taskManager = new ScheduleManager();
            var weManager = new WeeklyEveryCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyEvery;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 4;
            taskConfig.StartDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2026, 1, 5, 0, 0, 0, TimeSpan.Zero);
            taskConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            taskConfig.DailyEndTime = new TimeSpan(19, 30, 0);
            taskConfig.TimeUnit = TimeUnit.Hours;
            taskConfig.TimeUnitNumberOf = 2;

            var result = weManager.CalculateWeeklyRecurringConfig(taskConfig, 500);

            List<DateTimeOffset> expected = new List<DateTimeOffset>();

            expected.Add(new DateTimeOffset(2026, 1, 4, 13, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2026, 1, 4, 15, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2026, 1, 4, 17, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2026, 1, 4, 19, 30, 0, TimeSpan.Zero));

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenExecutionAtTheEndDate()
        {
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Tuesday);
            listOfDays.Add(DayOfWeek.Sunday);

            var taskManager = new ScheduleManager();
            var weManager = new WeeklyEveryCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyEvery;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 4;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 27, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 27, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 28, 0, 0, 0, TimeSpan.Zero);
            taskConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            taskConfig.DailyEndTime = new TimeSpan(19, 30, 0);
            taskConfig.TimeUnit = TimeUnit.Hours;
            taskConfig.TimeUnitNumberOf = 2;

            var result = weManager.CalculateWeeklyRecurringConfig(taskConfig, 500);

            List<DateTimeOffset> expected = new List<DateTimeOffset>();

            expected.Add(new DateTimeOffset(2025, 10, 27, 13, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 27, 15, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 27, 17, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 27, 19, 30, 0, TimeSpan.Zero));

            // Los que interesan que coja

            expected.Add(new DateTimeOffset(2025, 10, 28, 13, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 28, 15, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 28, 17, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 28, 19, 30, 0, TimeSpan.Zero));

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyOnce_ShouldPass_WhenExecutionAtTheEndDate()
        {
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Tuesday);
            listOfDays.Add(DayOfWeek.Sunday);

            var taskManager = new ScheduleManager();
            var weManager = new WeeklyOnceCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyOnce;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 2;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 27, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 27, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 28, 0, 0, 0, TimeSpan.Zero);
            taskConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);

            var result = weManager.CalculateWeeklyOnceConfig(taskConfig, 500);

            List<DateTimeOffset> expected = new List<DateTimeOffset>();

            expected.Add(new DateTimeOffset(2025, 10, 27, 13, 30, 0, TimeSpan.Zero));

            // Lo que interesa que coja

            expected.Add(new DateTimeOffset(2025, 10, 28, 13, 30, 0, TimeSpan.Zero));
            
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyOnce_ShouldPass_JumpWhenDayItsNotSelected()
        {
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);

            var taskManager = new ScheduleManager();
            var weManager = new WeeklyOnceCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyOnce;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 1;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 21, 0, 0, 0, TimeSpan.Zero);
            taskConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);

            var result = weManager.CalculateWeeklyOnceConfig(taskConfig, 500);

            List<DateTimeOffset> expected = new List<DateTimeOffset>();

            expected.Add(new DateTimeOffset(2025, 10, 20, 13, 30, 0, TimeSpan.Zero));

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_JumpWhenDayItsNotSelected()
        {
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);

            var taskManager = new ScheduleManager();
            var weManager = new WeeklyEveryCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyEvery;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 1;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 21, 0, 0, 0, TimeSpan.Zero);
            taskConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            taskConfig.DailyEndTime = new TimeSpan(19, 30, 0);
            taskConfig.TimeUnit = TimeUnit.Hours;
            taskConfig.TimeUnitNumberOf = 2;

            var result = weManager.CalculateWeeklyRecurringConfig(taskConfig, 500);

            List<DateTimeOffset> expected = new List<DateTimeOffset>();

            expected.Add(new DateTimeOffset(2025, 10, 20, 13, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 20, 15, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 20, 17, 30, 0, TimeSpan.Zero));
            expected.Add(new DateTimeOffset(2025, 10, 20, 19, 30, 0, TimeSpan.Zero));

            Assert.Equal(expected, result);
        }

        [Fact]
        public void WeeklyOnce_ShouldPass_WhenNoDatesAvailable()
        {
            var listOfDays = new List<DayOfWeek>();

            var taskManager = new ScheduleManager();
            var weManager = new WeeklyOnceCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyOnce;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 1;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero);

            var result = weManager.GetNextExecutionWeeklyOnce(taskConfig,10);

            
            Assert.Contains("No next execution found",result.Error);
            Assert.True(result.IsFailure);

        }

        [Fact]
        public void WeeklyOnce_ShouldPass_WhenUsingMaxOccurrences()
        {
            var listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Tuesday);

            var taskManager = new ScheduleManager();
            var weManager = new WeeklyOnceCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyOnce;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 2;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);

            List<DateTimeOffset> expectedDates = new List<DateTimeOffset>();
            expectedDates.Add(new DateTimeOffset(2025, 10, 27, 13, 30, 0, 0, TimeSpan.Zero));
            expectedDates.Add(new DateTimeOffset(2025, 10, 28, 13, 30, 0, 0, TimeSpan.Zero));
            expectedDates.Add(new DateTimeOffset(2025, 11, 10, 13, 30, 0, 0, TimeSpan.Zero));

            var result = weManager.CalculateWeeklyOnceConfig(taskConfig, 3);

            Assert.Equal(result,expectedDates);

        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenGetWeeklyDescription2Days()
        {
            var listOfDays = new List<DayOfWeek>();

            listOfDays.Add(DayOfWeek.Monday);
            listOfDays.Add(DayOfWeek.Tuesday);

            var taskManager = new ScheduleManager();
            var weManager = new WeeklyEveryCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyEvery;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 1;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 21, 0, 0, 0, TimeSpan.Zero);
            taskConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            taskConfig.DailyEndTime = new TimeSpan(19, 30, 0);
            taskConfig.TimeUnit = TimeUnit.Hours;
            taskConfig.TimeUnitNumberOf = 2;

            var result = DescriptionGenerator.GetWeeklyDescription(taskConfig.DaysOfWeek);

           
            Assert.Equal("monday and tuesday",result);
        }

        [Fact]
        public void WeeklyEvery_ShouldPass_WhenGetWeeklyDescription0Days()
        {
            var listOfDays = new List<DayOfWeek>();


            var taskManager = new ScheduleManager();
            var weManager = new WeeklyEveryCalculator();

            var taskConfig = new ScheduleEntity();
            taskConfig.ScheduleType = Enums.ScheduleType.WeeklyEvery;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.WeeklyRecurrence = 1;
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 21, 0, 0, 0, TimeSpan.Zero);
            taskConfig.DailyStartTime = new TimeSpan(13, 30, 0);
            taskConfig.DailyEndTime = new TimeSpan(19, 30, 0);
            taskConfig.TimeUnit = TimeUnit.Hours;
            taskConfig.TimeUnitNumberOf = 2;

            var result = DescriptionGenerator.GetWeeklyDescription(taskConfig.DaysOfWeek);


            Assert.Equal("No days specified", result);
        }


        // 
    }
}
