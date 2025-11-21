using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Enums;
using MyScheduler.Infraestructure.Localizer;
using MyScheduler.Helpers;
using MyScheduler.Application.Services;

namespace MyScheduler.Descriptions
{
    public class DescriptionTests
    {
        [Fact]
        public void GetOutput_ShouldPass_WhenMonthlyDay_AllIsGood()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Enabled = true,
                Occurs = Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                MonthlyDayNumber = 3,
                MonthlyDayRecurrence = 2,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.Zero,
                StartDate = new DateTimeOffset(2025, 6, 11, 0, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, 0, 0, TimeSpan.Zero)
            };
            var manager = new ScheduleManager();
            var result = manager.GetOutput(schedule, 10);
            var expectedDate = new DateTimeOffset(2025, 8, 3, 0, 0, 0, TimeSpan.FromHours(2));
            var expectedDescription = $"Occurs day {schedule.MonthlyDayNumber} every {schedule.MonthlyDayRecurrence} month(s) at 00:00, starting 11/06/2025";
            Assert.Equal(expectedDate, result.Value.ExecutionTime);
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Theory]
        [InlineData("en-US", "Occurs every 1 day(s) at 08:00, starting 01/01/2025")]
        [InlineData("en-UK", "Occurs every 1 day(s) at 08:00, starting 01/01/2025")]
        [InlineData("es", "Ocurre cada 1 día(s) a las 08:00, comienza el 01/01/2025")]
        public void GetOutput_ShouldPass_WhenDailyOnce_AllLanguages(string language, string expectedDescription)
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Enabled = true,
                Occurs = Occurs.Daily,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = new TimeSpan(8, 0, 0),
                Recurrence = 1,
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, 0, 0, TimeSpan.Zero),
                Language = language
            };
            var manager = new ScheduleManager();
            var result = manager.GetOutput(schedule, 1);
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Theory]
        [InlineData("en-US", "Occurs every 1 week(s) on monday and wednesday every 2 hours between 08:00 and 10:00, starting 01/01/2025")]
        [InlineData("en-UK", "Occurs every 1 week(s) on monday and wednesday every 2 hours between 08:00 and 10:00, starting 01/01/2025")]
        [InlineData("es", "Ocurre cada 1 semana(s) los lunes y miércoles cada 2 horas entre las 08:00 y las 10:00, comienza el 01/01/2025")]
        public void GetOutput_ShouldPass_WhenWeeklyRange_AllLanguages(string language, string expectedDescription)
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Enabled = true,
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                WeeklyRecurrence = 1,
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday },
                TimeUnitNumberOf = 2,
                TimeUnit = TimeUnit.Hours,
                DailyStartTime = new TimeSpan(8, 0, 0),
                DailyEndTime = new TimeSpan(10, 0, 0),
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, 0, 0, TimeSpan.Zero),
                Language = language
            };
            var manager = new ScheduleManager();
            var result = manager.GetOutput(schedule, 1);
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Theory]
        [InlineData("en-US", "Occurs the first Monday of every 1 month(s) every 2 hours between 08:00 and 10:00, starting 01/01/2025")]
        [InlineData("en-UK", "Occurs the first Monday of every 1 month(s) every 2 hours between 08:00 and 10:00, starting 01/01/2025")]
        [InlineData("es", "Ocurre el primer lunes de cada 1 mes(es) cada 2 horas entre las 08:00 y las 10:00, comienza el 01/01/2025")]
        public void GetOutput_ShouldPass_WhenMonthlyTheRange_AllLanguages(string language, string expectedDescription)
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Enabled = true,
                Occurs = Occurs.Monthly,
                MonthlyFrequencyTheCheckbox = true,
                MonthlyTheOrder = MonthlyTheOrder.First,
                MonthlyTheDayOfWeek = MonthlyDayOfWeek.Monday,
                MonthlyTheRecurrence = 1,
                DailyFrequencyRangeCheckbox = true,
                TimeUnitNumberOf = 2,
                TimeUnit = TimeUnit.Hours,
                DailyStartTime = new TimeSpan(8, 0, 0),
                DailyEndTime = new TimeSpan(10, 0, 0),
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, 0, 0, TimeSpan.Zero),
                Language = language
            };
            var manager = new ScheduleManager();
            var result = manager.GetOutput(schedule, 1);
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Theory]
        [InlineData("en-US", "Occurs day 5 every 1 month(s) at 08:00, starting 01/01/2025")]
        [InlineData("en-UK", "Occurs day 5 every 1 month(s) at 08:00, starting 01/01/2025")]
        [InlineData("es", "Ocurre el día 5 cada 1 mes(es) a las 08:00, comienza el 01/01/2025")]
        public void GetOutput_ShouldPass_WhenMonthlyDayOnce_AllLanguages(string language, string expectedDescription)
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Enabled = true,
                Occurs = Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                MonthlyDayNumber = 5,
                MonthlyDayRecurrence = 1,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = new TimeSpan(8, 0, 0),
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, 0, 0, TimeSpan.Zero),
                Language = language
            };
            var manager = new ScheduleManager();
            var result = manager.GetOutput(schedule, 1);
            Assert.Equal(expectedDescription, result.Value.Description);
        }


        [Theory]
        [InlineData("en-US", "Occurs day 5 every 1 month(s) at 08:00, starting 01/01/2025")]
        [InlineData("en-UK", "Occurs day 5 every 1 month(s) at 08:00, starting 01/01/2025")]
        [InlineData("es", "Ocurre el día 5 cada 1 mes(es) a las 08:00, comienza el 01/01/2025")]
        public void GetOutput_ShouldPass_WhenMonthlyDayOnce_AllLanguage(string language, string expectedDescription)
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Enabled = true,
                Occurs = Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                MonthlyDayNumber = 5,
                MonthlyDayRecurrence = 1,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = new TimeSpan(8, 0, 0),
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, 0, 0, TimeSpan.Zero),
                Language = language
            };
            var manager = new ScheduleManager();
            var result = manager.GetOutput(schedule, 1);
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Theory]
        [InlineData("en-US", "Occurs day 10 every 2 month(s) every 3 hours between 09:00 and 15:00, starting 01/01/2025")]
        [InlineData("en-UK", "Occurs day 10 every 2 month(s) every 3 hours between 09:00 and 15:00, starting 01/01/2025")]
        [InlineData("es", "Ocurre el día 10 cada 2 mes(es) cada 3 horas entre las 09:00 y las 15:00, comienza el 01/01/2025")]
        public void GetOutput_ShouldPass_WhenMonthlyDayRange_AllLanguages(string language, string expectedDescription)
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Enabled = true,
                Occurs = Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                MonthlyDayNumber = 10,
                MonthlyDayRecurrence = 2,
                DailyFrequencyRangeCheckbox = true,
                DailyStartTime = new TimeSpan(9, 0, 0),
                DailyEndTime = new TimeSpan(15, 0, 0),
                TimeUnitNumberOf = 3,
                TimeUnit = TimeUnit.Hours,
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, 0, 0, TimeSpan.Zero),
                Language = language
            };
            var manager = new ScheduleManager();
            var result = manager.GetOutput(schedule, 1);
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Theory]
        [InlineData("en-US", "Occurs every 1 week(s) on monday every 15 minutes between 10:00 and 12:00, starting 01/01/2025")]
        [InlineData("en-UK", "Occurs every 1 week(s) on monday every 15 minutes between 10:00 and 12:00, starting 01/01/2025")]
        [InlineData("es", "Ocurre cada 1 semana(s) los lunes cada 15 minutos entre las 10:00 y las 12:00, comienza el 01/01/2025")]
        public void GetOutput_ShouldPass_WhenWeeklyRangeSingleDay_AllLanguages(string language, string expectedDescription)
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Enabled = true,
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                WeeklyRecurrence = 1,
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday },
                TimeUnitNumberOf = 15,
                TimeUnit = TimeUnit.Minutes,
                DailyStartTime = new TimeSpan(10, 0, 0),
                DailyEndTime = new TimeSpan(12, 0, 0),
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, 0, 0, TimeSpan.Zero),
                Language = language
            };
            var manager = new ScheduleManager();
            var result = manager.GetOutput(schedule, 1);
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Theory]
        [InlineData("en-US", "Occurs every 1 day(s) every 10 minutes between 07:00 and 08:00, starting 01/01/2025")]
        [InlineData("en-UK", "Occurs every 1 day(s) every 10 minutes between 07:00 and 08:00, starting 01/01/2025")]
        [InlineData("es", "Ocurre cada 1 día(s) cada 10 minutos entre las 07:00 y las 08:00, comienza el 01/01/2025")]
        public void GetOutput_ShouldPass_WhenDailyRange_AllLanguages(string language, string expectedDescription)
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Enabled = true,
                Occurs = Occurs.Daily,
                DailyFrequencyRangeCheckbox = true,
                Recurrence = 1,
                TimeUnitNumberOf = 10,
                TimeUnit = TimeUnit.Minutes,
                DailyStartTime = new TimeSpan(7, 0, 0),
                DailyEndTime = new TimeSpan(8, 0, 0),
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, 0, 0, TimeSpan.Zero),
                Language = language
            };
            var manager = new ScheduleManager();
            var result = manager.GetOutput(schedule, 1);
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Theory]
        [InlineData("en-US", "Occurs the last Friday of every 3 month(s) at 20:00, starting 01/01/2025")]
        [InlineData("en-UK", "Occurs the last Friday of every 3 month(s) at 20:00, starting 01/01/2025")]
        [InlineData("es", "Ocurre el último viernes de cada 3 mes(es) a las 20:00, comienza el 01/01/2025")]
        public void GetOutput_ShouldPass_WhenMonthlyTheOnce_AllLanguages(string language, string expectedDescription)
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Enabled = true,
                Occurs = Occurs.Monthly,
                MonthlyFrequencyTheCheckbox = true,
                MonthlyTheOrder = MonthlyTheOrder.Last,
                MonthlyTheDayOfWeek = MonthlyDayOfWeek.Friday,
                MonthlyTheRecurrence = 3,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = new TimeSpan(20, 0, 0),
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, 0, 0, TimeSpan.Zero),
                Language = language
            };
            var manager = new ScheduleManager();
            var result = manager.GetOutput(schedule, 1);
            Assert.Equal(expectedDescription, result.Value.Description);
        }

        [Fact]
        public void Localizer_ShouldPass_WhenReturnCorrectDescription()
        {
            var localizer = new Localizer();
            var scheduleConfig = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Daily,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = new TimeSpan(8, 0, 0),
                Recurrence = 1,
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var result = DescriptionGenerator.GetDescription(scheduleConfig);
            Assert.Equal("Occurs every 1 day(s) at 08:00, starting 01/01/2025", result);
        }

        [Theory]
        [InlineData("en-US", "Monday")]
        [InlineData("en-UK", "Monday")]
        [InlineData("es", "lunes")]
        public void Localizer_ShouldPass_WhenReturnCorrectDayTranslation(string language, string expected)
        {
            var localizer = new Localizer();
            var result = localizer.GetString("Monday", language);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("en-US", "no days specified")]
        [InlineData("en-UK", "no days specified")]
        [InlineData("es", "no se especificaron días")]
        public void Localizer_ShouldPass_WhenReturnCorrectNoDaysSpecified(string language, string expected)
        {
            var localizer = new Localizer();
            var result = localizer.GetString("NoDaysSpecified", language);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetDescription_ShouldPass_WhenOnce_ReturnsExpected()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Once,
                OnceTypeDateExecution = new DateTimeOffset(2025, 1, 1, 8, 0, 0, TimeSpan.Zero),
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var result = DescriptionGenerator.GetDescription(schedule);
            Assert.Contains("Occurs once. Schedule on", result);
        }

        [Fact]
        public void GetDescription_ShouldPass_WhenRecurring_ReturnsExpected()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = ScheduleType.Recurring,
                Occurs = Occurs.Daily,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = new TimeSpan(8, 0, 0),
                Recurrence = 1,
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var result = DescriptionGenerator.GetDescription(schedule);
            Assert.Contains("Occurs every 1 day(s) at", result);
        }

        [Fact]
        public void GetDescription_ShouldPass_WhenDefault_ReturnsNotAvailable()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = (ScheduleType)99,
                Language = "en-US"
            };
            var result = DescriptionGenerator.GetDescription(schedule);
            Assert.Equal("Description not available for this task type.", result);
        }

        [Fact]
        public void GetRecurringDescription_ShouldPass_WhenDailyOnce()
        {
            var schedule = new ScheduleEntity
            {
                Occurs = Occurs.Daily,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = new TimeSpan(8, 0, 0),
                Recurrence = 1,
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetRecurringDescription(schedule, localizer, "en-US");
            Assert.Contains("Occurs every 1 day(s) at", result);
        }

        [Fact]
        public void GetRecurringDescription_ShouldPass_WhenDailyRange()
        {
            var schedule = new ScheduleEntity
            {
                Occurs = Occurs.Daily,
                DailyFrequencyRangeCheckbox = true,
                Recurrence = 1,
                TimeUnitNumberOf = 2,
                TimeUnit = TimeUnit.Hours,
                DailyStartTime = new TimeSpan(8, 0, 0),
                DailyEndTime = new TimeSpan(10, 0, 0),
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetRecurringDescription(schedule, localizer, "en-US");
            Assert.Contains("Occurs every 1 day(s) every 2 hours", result);
        }

        [Fact]
        public void GetRecurringDescription_ShouldPass_WhenWeeklyOnce()
        {
            var schedule = new ScheduleEntity
            {
                Occurs = Occurs.Weekly,
                DailyFrequencyOnceCheckbox = true,
                WeeklyRecurrence = 1,
                DailyOnceExecutionTime = new TimeSpan(8, 0, 0),
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday },
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetRecurringDescription(schedule, localizer, "en-US");
            Assert.Contains("Occurs every 1 week(s) on", result);
        }

        [Fact]
        public void GetRecurringDescription_ShouldPass_WhenWeeklyRange()
        {
            var schedule = new ScheduleEntity
            {
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                WeeklyRecurrence = 1,
                TimeUnitNumberOf = 2,
                TimeUnit = TimeUnit.Hours,
                DailyStartTime = new TimeSpan(8, 0, 0),
                DailyEndTime = new TimeSpan(10, 0, 0),
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday },
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetRecurringDescription(schedule, localizer, "en-US");
            Assert.Contains("Occurs every 1 week(s) on", result);
            Assert.Contains("every 2 hours", result);
        }

        [Fact]
        public void GetRecurringDescription_ShouldPass_WhenMonthlyDay()
        {
            var schedule = new ScheduleEntity
            {
                Occurs = Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                MonthlyDayNumber = 10,
                MonthlyDayRecurrence = 2,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = new TimeSpan(8, 0, 0),
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetRecurringDescription(schedule, localizer, "en-US");
            Assert.Contains("Occurs day 10 every 2 month(s)", result);
            Assert.Contains("at 08:00", result);
        }

        [Fact]
        public void GetRecurringDescription_ShouldPass_WhenMonthlyThe()
        {
            var schedule = new ScheduleEntity
            {
                Occurs = Occurs.Monthly,
                MonthlyFrequencyTheCheckbox = true,
                MonthlyTheOrder = MonthlyTheOrder.First,
                MonthlyTheDayOfWeek = MonthlyDayOfWeek.Monday,
                MonthlyTheRecurrence = 1,
                DailyFrequencyRangeCheckbox = true,
                TimeUnitNumberOf = 2,
                TimeUnit = TimeUnit.Hours,
                DailyStartTime = new TimeSpan(8, 0, 0),
                DailyEndTime = new TimeSpan(10, 0, 0),
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetRecurringDescription(schedule, localizer, "en-US");
            Assert.Contains("Occurs the first Monday of every 1 month(s)", result);
            Assert.Contains("every 2 hours", result);
        }

        [Fact]
        public void GetRecurringDescription_ShouldPass_WhenDefault()
        {
            var schedule = new ScheduleEntity
            {
                Occurs = (Occurs)99,
                Language = "en-US"
            };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetRecurringDescription(schedule, localizer, "en-US");
            Assert.Equal("Recurring schedule description not available.", result);
        }

        [Fact]
        public void GetRecurringDescription_ShouldPass_WhenDaily_NoCheckboxes()
        {
            var schedule = new ScheduleEntity
            {
                Occurs = Occurs.Daily,
                DailyFrequencyOnceCheckbox = false,
                DailyFrequencyRangeCheckbox = false,
                Recurrence = 1,
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetRecurringDescription(schedule, localizer, "en-US");
            Assert.Equal("Recurring schedule description not available.", result);
        }

        [Fact]
        public void GetRecurringDescription_ShouldPass_WhenWeekly_NoCheckboxes()
        {
            var schedule = new ScheduleEntity
            {
                Occurs = Occurs.Weekly,
                DailyFrequencyOnceCheckbox = false,
                DailyFrequencyRangeCheckbox = false,
                WeeklyRecurrence = 1,
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday },
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetRecurringDescription(schedule, localizer, "en-US");
            Assert.Equal("Recurring schedule description not available.", result);
        }

        [Fact]
        public void GetRecurringDescription_ShouldPass_WhenMonthly_NoCheckboxes()
        {
            var schedule = new ScheduleEntity
            {
                Occurs = Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = false,
                MonthlyFrequencyTheCheckbox = false,
                DailyFrequencyOnceCheckbox = false,
                DailyFrequencyRangeCheckbox = false,
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetRecurringDescription(schedule, localizer, "en-US");
            Assert.Contains(", starting", result); 
        }

        [Fact]
        public void GetRecurringDescription_ShouldPass_WhenMonthly_OnlyDayCheckbox()
        {
            var schedule = new ScheduleEntity
            {
                Occurs = Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                MonthlyFrequencyTheCheckbox = false,
                MonthlyDayNumber = 5,
                MonthlyDayRecurrence = 1,
                DailyFrequencyOnceCheckbox = false,
                DailyFrequencyRangeCheckbox = false,
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetRecurringDescription(schedule, localizer, "en-US");
            Assert.Contains("Occurs day 5 every 1 month(s)", result);
            Assert.Contains(", starting", result);
        }

        [Fact]
        public void GetRecurringDescription_ShouldPass_WhenMonthly_OnlyTheCheckbox()
        {
            var schedule = new ScheduleEntity
            {
                Occurs = Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = false,
                MonthlyFrequencyTheCheckbox = true,
                MonthlyTheOrder = MonthlyTheOrder.Last,
                MonthlyTheDayOfWeek = MonthlyDayOfWeek.Friday,
                MonthlyTheRecurrence = 3,
                DailyFrequencyOnceCheckbox = false,
                DailyFrequencyRangeCheckbox = false,
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetRecurringDescription(schedule, localizer, "en-US");
            Assert.Contains("Occurs the last Friday of every 3 month(s)", result);
            Assert.Contains(", starting", result);
        }

        [Fact]
        public void GetRecurringDescription_ShouldPass_WhenWeekly_NullDaysOfWeek()
        {
            var schedule = new ScheduleEntity
            {
                Occurs = Occurs.Weekly,
                DailyFrequencyOnceCheckbox = true,
                WeeklyRecurrence = 1,
                DailyOnceExecutionTime = new TimeSpan(8, 0, 0),
                DaysOfWeek = null,
                StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetRecurringDescription(schedule, localizer, "en-US");
            Assert.Contains("no days specified", result);
        }

        [Fact]
        public void GetWeeklyDayList_ShouldPass_WhenNullOrEmpty_ReturnsNoDaysSpecified()
        {
            var localizer = new Localizer();
            Assert.Equal("no days specified", DescriptionGenerator.GetWeeklyDayList(null, localizer, "en-US"));
            Assert.Equal("no days specified", DescriptionGenerator.GetWeeklyDayList(new List<DayOfWeek>(), localizer, "en-US"));
        }

        [Fact]
        public void GetWeeklyDayList_ShouldPass_WhenOneDay_ReturnsDay()
        {
            var localizer = new Localizer();
            var days = new List<DayOfWeek> { DayOfWeek.Monday };
            Assert.Equal("monday", DescriptionGenerator.GetWeeklyDayList(days, localizer, "en-US"));
        }

        [Fact]
        public void GetWeeklyDayList_ShouldPass_WhenTwoDays_ReturnsDayAndDay()
        {
            var localizer = new Localizer();
            var days = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday };
            Assert.Equal("monday and tuesday", DescriptionGenerator.GetWeeklyDayList(days, localizer, "en-US"));
        }

        [Fact]
        public void GetWeeklyDayList_ShouldPass_WhenMoreThanTwoDays_ReturnsDaysAndDay()
        {
            var localizer = new Localizer();
            var days = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday };
            Assert.Equal("monday, tuesday and wednesday", DescriptionGenerator.GetWeeklyDayList(days, localizer, "en-US"));
        }


        [Fact]
        public void GetRecurringDescription_ShouldPass_WhenWeeklyRange_AllValuesPresent()
        {
            var schedule = new ScheduleEntity
            {
                Occurs = Occurs.Weekly,
                DailyFrequencyRangeCheckbox = true,
                WeeklyRecurrence = 2,
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday },
                TimeUnitNumberOf = 5,
                TimeUnit = TimeUnit.Minutes,
                DailyStartTime = new TimeSpan(9, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0),
                StartDate = new DateTimeOffset(2025, 6, 6, 0, 0, 0, TimeSpan.Zero),
                Language = "en-US"
            };
            var localizer = new Localizer();
            var result = DescriptionGenerator.GetRecurringDescription(schedule, localizer, "en-US");
            Assert.Contains("Occurs every 2 week(s) on monday and wednesday every 5 minutes between 09:00 and 17:00, starting 06/06/2025", result);
        }


    }
}
