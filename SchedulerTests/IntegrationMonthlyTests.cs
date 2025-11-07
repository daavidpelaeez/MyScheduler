using MyScheduler.Entities;
using MyScheduler.Helpers;
using MyScheduler.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScheduler
{
    public class IntegrationMonthlyTests
    {
        
        [Fact]
        public void MonthlyDay_ShouldPass_WhenAllIsGood()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 3;
            schedule.MonthlyDayRecurrence = 2;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025,6,11,0,0,0,0,TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero);
            
            ScheduleManager manager = new ScheduleManager();

            var result = manager.GetNextExecution(schedule, 10);

            var expectedDate = new DateTimeOffset(2025,8,3,0,0,0,TimeSpan.FromHours(2));
            var expectedDescription = $"Occurs day {schedule.MonthlyDayNumber} every {schedule.MonthlyDayRecurrence} month(s) at {schedule.DailyOnceExecutionTime}, starting 11/06/2025";

            Assert.Equal(expectedDate,result.Value.ExecutionTime); 
            Assert.Equal(expectedDescription,result.Value.Description);
        }

        [Fact]
        public void MonthlyDay_ShouldPass_WithDayNumberOne()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 1;
            schedule.MonthlyDayRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025,1,15,0,0,0,0,TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025,6,30,0,0,0,0,0, TimeSpan.Zero);
            
            ScheduleManager manager = new ScheduleManager();

            var result = manager.GetNextExecution(schedule, 10);

            var expectedDate = new DateTimeOffset(2025,2,1,0,0,0,TimeSpan.FromHours(1));
            var expectedDescription = $"Occurs day {schedule.MonthlyDayNumber} every {schedule.MonthlyDayRecurrence} month(s) at {schedule.DailyOnceExecutionTime}, starting 15/01/2025";

            Assert.Equal(expectedDate,result.Value.ExecutionTime); 
            Assert.Equal(expectedDescription,result.Value.Description);
        }

        [Fact]
        public void MonthlyDay_ShouldPass_WithDayNumberThirtyOne()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 31;
            schedule.MonthlyDayRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero);
            
            ScheduleManager manager = new ScheduleManager();

            var result = manager.GetNextExecution(schedule, 10);

            var expectedDate = new DateTimeOffset(2025,1,31,0,0,0,TimeSpan.FromHours(1));
            var expectedDescription = $"Occurs day {schedule.MonthlyDayNumber} every {schedule.MonthlyDayRecurrence} month(s) at {schedule.DailyOnceExecutionTime}, starting 01/01/2025";

            Assert.Equal(expectedDate,result.Value.ExecutionTime); 
            Assert.Equal(expectedDescription,result.Value.Description);
        }


        [Fact]
        public void MonthlyDay_ShouldPass_WithThreeMonthRecurrence()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 15;
            schedule.MonthlyDayRecurrence = 3;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero);
            
            ScheduleManager manager = new ScheduleManager();

            var result = manager.GetNextExecution(schedule, 10);

            var expectedDate = new DateTimeOffset(2025,1,15,0,0,0,TimeSpan.FromHours(1));
            var expectedDescription = $"Occurs day {schedule.MonthlyDayNumber} every {schedule.MonthlyDayRecurrence} month(s) at {schedule.DailyOnceExecutionTime}, starting 01/01/2025";

            Assert.Equal(expectedDate,result.Value.ExecutionTime); 
            Assert.Equal(expectedDescription,result.Value.Description);
        }


      [Fact]
        public void MonthlyThe_ShouldPass_FirstMonday()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.First;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Monday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero);
            
            ScheduleManager manager = new ScheduleManager();

            var result = manager.GetNextExecution(schedule, 10);

            var expectedDate = new DateTimeOffset(2025,1,6,0,0,0,TimeSpan.FromHours(1));
            var expectedDescription = $"Occurs the {schedule.MonthlyTheOrder} {schedule.MonthlyTheDayOfWeek} of every {schedule.MonthlyTheRecurrence} month(s) at {schedule.DailyOnceExecutionTime}, starting 01/01/2025";

            Assert.Equal(expectedDate,result.Value.ExecutionTime); 
            Assert.Equal(expectedDescription,result.Value.Description);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_SecondTuesday()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Second;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Tuesday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero);
            
            ScheduleManager manager = new ScheduleManager();

            var result = manager.GetNextExecution(schedule, 10);

            var expectedDate = new DateTimeOffset(2025,1,14,0,0,0,TimeSpan.FromHours(1));
            var expectedDescription = $"Occurs the {schedule.MonthlyTheOrder} {schedule.MonthlyTheDayOfWeek} of every {schedule.MonthlyTheRecurrence} month(s) at {schedule.DailyOnceExecutionTime}, starting 01/01/2025";

            Assert.Equal(expectedDate,result.Value.ExecutionTime); 
            Assert.Equal(expectedDescription,result.Value.Description);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_ThirdWednesday()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Third;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Wednesday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero);
            
            ScheduleManager manager = new ScheduleManager();

            var result = manager.GetNextExecution(schedule, 10);

            var expectedDate = new DateTimeOffset(2025,1,15,0,0,0,TimeSpan.FromHours(1));
            var expectedDescription = $"Occurs the {schedule.MonthlyTheOrder} {schedule.MonthlyTheDayOfWeek} of every {schedule.MonthlyTheRecurrence} month(s) at {schedule.DailyOnceExecutionTime}, starting 01/01/2025";

            Assert.Equal(expectedDate,result.Value.ExecutionTime); 
            Assert.Equal(expectedDescription,result.Value.Description);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_FourthThursday()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Fourth;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Thursday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero);
            
            ScheduleManager manager = new ScheduleManager();

            var result = manager.GetNextExecution(schedule, 10);

            var expectedDate = new DateTimeOffset(2025,1,23,0,0,0,TimeSpan.FromHours(1));
            var expectedDescription = $"Occurs the {schedule.MonthlyTheOrder} {schedule.MonthlyTheDayOfWeek} of every {schedule.MonthlyTheRecurrence} month(s) at {schedule.DailyOnceExecutionTime}, starting 01/01/2025";

            Assert.Equal(expectedDate,result.Value.ExecutionTime); 
            Assert.Equal(expectedDescription,result.Value.Description);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_LastFriday()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Last;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Friday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero);
            
            ScheduleManager manager = new ScheduleManager();

            var result = manager.GetNextExecution(schedule, 10);

            var expectedDate = new DateTimeOffset(2025,1,31,0,0,0,TimeSpan.FromHours(1));
            var expectedDescription = $"Occurs the {schedule.MonthlyTheOrder} {schedule.MonthlyTheDayOfWeek} of every {schedule.MonthlyTheRecurrence} month(s) at {schedule.DailyOnceExecutionTime}, starting 01/01/2025";

            Assert.Equal(expectedDate,result.Value.ExecutionTime); 
            Assert.Equal(expectedDescription,result.Value.Description);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_FirstSaturday()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.First;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Saturday;
            schedule.MonthlyTheRecurrence = 2;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero);
            
            ScheduleManager manager = new ScheduleManager();

            var result = manager.GetNextExecution(schedule, 10);

            var expectedDate = new DateTimeOffset(2025,1,4,0,0,0,TimeSpan.FromHours(1));
            var expectedDescription = $"Occurs the {schedule.MonthlyTheOrder} {schedule.MonthlyTheDayOfWeek} of every {schedule.MonthlyTheRecurrence} month(s) at {schedule.DailyOnceExecutionTime}, starting 01/01/2025";

            Assert.Equal(expectedDate,result.Value.ExecutionTime); 
            Assert.Equal(expectedDescription,result.Value.Description);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_LastSunday()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Last;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Sunday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero);
            
            ScheduleManager manager = new ScheduleManager();

            var result = manager.GetNextExecution(schedule, 10);

            var expectedDate = new DateTimeOffset(2025,1,26,0,0,0,TimeSpan.FromHours(1));
            var expectedDescription = $"Occurs the {schedule.MonthlyTheOrder} {schedule.MonthlyTheDayOfWeek} of every {schedule.MonthlyTheRecurrence} month(s) at {schedule.DailyOnceExecutionTime}, starting 01/01/2025";

            Assert.Equal(expectedDate,result.Value.ExecutionTime); 
            Assert.Equal(expectedDescription,result.Value.Description);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_FirstDay()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.First;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Day;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025,1,5,0,0,0,0,TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero);
            
            ScheduleManager manager = new ScheduleManager();

            var result = manager.GetNextExecution(schedule, 10);

            var expectedDate = new DateTimeOffset(2025,2,1,0,0,0,TimeSpan.FromHours(1));
            var expectedDescription = $"Occurs the {schedule.MonthlyTheOrder} {schedule.MonthlyTheDayOfWeek} of every {schedule.MonthlyTheRecurrence} month(s) at {schedule.DailyOnceExecutionTime}, starting 05/01/2025";

            Assert.Equal(expectedDate,result.Value.ExecutionTime); 
            Assert.Equal(expectedDescription,result.Value.Description);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_FirstWeekday()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.First;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Weekday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero);
            
            ScheduleManager manager = new ScheduleManager();

            var result = manager.GetNextExecution(schedule, 10);

            var expectedDate = new DateTimeOffset(2025,1,1,0,0,0,TimeSpan.FromHours(1));
            var expectedDescription = $"Occurs the {schedule.MonthlyTheOrder} {schedule.MonthlyTheDayOfWeek} of every {schedule.MonthlyTheRecurrence} month(s) at {schedule.DailyOnceExecutionTime}, starting 01/01/2025";

            Assert.Equal(expectedDate,result.Value.ExecutionTime); 
            Assert.Equal(expectedDescription,result.Value.Description);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_LastWeekendDay()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Last;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.WeekendDay;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.Zero;
            schedule.StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero);
            
            ScheduleManager manager = new ScheduleManager();

            var result = manager.GetNextExecution(schedule, 10);

            var expectedDate = new DateTimeOffset(2025,1,26,0,0,0,TimeSpan.FromHours(1));
            var expectedDescription = $"Occurs the {schedule.MonthlyTheOrder} {schedule.MonthlyTheDayOfWeek} of every {schedule.MonthlyTheRecurrence} month(s) at {schedule.DailyOnceExecutionTime}, starting 01/01/2025";

            Assert.Equal(expectedDate,result.Value.ExecutionTime); 
            Assert.Equal(expectedDescription,result.Value.Description);
        }


        [Theory]
        [InlineData(Enums.MonthlyTheOrder.First, Enums.MonthlyDayOfWeek.Monday, 1, 8, 0, 0)]
        [InlineData(Enums.MonthlyTheOrder.Second, Enums.MonthlyDayOfWeek.Tuesday, 2, 9, 15, 0)]
        [InlineData(Enums.MonthlyTheOrder.Third, Enums.MonthlyDayOfWeek.Wednesday, 1, 10, 30, 0)]
        [InlineData(Enums.MonthlyTheOrder.Fourth, Enums.MonthlyDayOfWeek.Thursday, 2, 11, 45, 0)]
        [InlineData(Enums.MonthlyTheOrder.Last, Enums.MonthlyDayOfWeek.Friday, 1, 12, 0, 0)]
        [InlineData(Enums.MonthlyTheOrder.First, Enums.MonthlyDayOfWeek.Day, 1, 13, 0, 0)]
        [InlineData(Enums.MonthlyTheOrder.Second, Enums.MonthlyDayOfWeek.Weekday, 2, 14, 15, 0)]
        [InlineData(Enums.MonthlyTheOrder.Third, Enums.MonthlyDayOfWeek.WeekendDay, 1, 15, 30, 0)]
        [InlineData(Enums.MonthlyTheOrder.Fourth, Enums.MonthlyDayOfWeek.Saturday, 2, 16, 45, 0)]
        [InlineData(Enums.MonthlyTheOrder.Last, Enums.MonthlyDayOfWeek.Sunday, 1, 17, 0, 0)]
        public void MonthlyThe_ShouldPass_AllOrdersAndDays_DailyOnce(
            Enums.MonthlyTheOrder order, Enums.MonthlyDayOfWeek dayOfWeek, int recurrence, int hour, int min, int sec)
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = order;
            schedule.MonthlyTheDayOfWeek = dayOfWeek;
            schedule.MonthlyTheRecurrence = recurrence;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = new TimeSpan(hour, min, sec);
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Theory]
        [InlineData(Enums.MonthlyTheOrder.First, Enums.MonthlyDayOfWeek.Monday, 1, 8, 0, 0, 10, 0, 0, 1, Enums.TimeUnit.Hours)]
        [InlineData(Enums.MonthlyTheOrder.Second, Enums.MonthlyDayOfWeek.Tuesday, 2, 9, 0, 0, 9, 30, 0, 15, Enums.TimeUnit.Minutes)]
        [InlineData(Enums.MonthlyTheOrder.Third, Enums.MonthlyDayOfWeek.Wednesday, 1, 10, 0, 0, 10, 1, 0, 20, Enums.TimeUnit.Seconds)]
        [InlineData(Enums.MonthlyTheOrder.Fourth, Enums.MonthlyDayOfWeek.Thursday, 2, 11, 0, 0, 12, 0, 0, 1, Enums.TimeUnit.Hours)]
        [InlineData(Enums.MonthlyTheOrder.Last, Enums.MonthlyDayOfWeek.Friday, 1, 13, 0, 0, 13, 30, 0, 30, Enums.TimeUnit.Minutes)]
        [InlineData(Enums.MonthlyTheOrder.First, Enums.MonthlyDayOfWeek.Day, 1, 14, 0, 0, 14, 1, 0, 10, Enums.TimeUnit.Seconds)]
        [InlineData(Enums.MonthlyTheOrder.Second, Enums.MonthlyDayOfWeek.Weekday, 2, 15, 0, 0, 16, 0, 0, 2, Enums.TimeUnit.Hours)]
        [InlineData(Enums.MonthlyTheOrder.Third, Enums.MonthlyDayOfWeek.WeekendDay, 1, 17, 0, 0, 17, 30, 0, 15, Enums.TimeUnit.Minutes)]
        [InlineData(Enums.MonthlyTheOrder.Fourth, Enums.MonthlyDayOfWeek.Saturday, 2, 18, 0, 0, 18, 1, 0, 5, Enums.TimeUnit.Seconds)]
        [InlineData(Enums.MonthlyTheOrder.Last, Enums.MonthlyDayOfWeek.Sunday, 1, 19, 0, 0, 20, 0, 0, 1, Enums.TimeUnit.Hours)]
        public void MonthlyTheDailyRange_ShouldPass_AllOrdersAndDays_AllTimeUnits(
            Enums.MonthlyTheOrder order, Enums.MonthlyDayOfWeek dayOfWeek, int recurrence,
            int sh, int sm, int ss, int eh, int em, int es, int unit, Enums.TimeUnit timeUnit)
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = order;
            schedule.MonthlyTheDayOfWeek = dayOfWeek;
            schedule.MonthlyTheRecurrence = recurrence;
            schedule.DailyFrequencyRangeCheckbox = true;
            schedule.DailyStartTime = new TimeSpan(sh, sm, ss);
            schedule.DailyEndTime = new TimeSpan(eh, em, es);
            schedule.TimeUnitNumberOf = unit;
            schedule.TimeUnit = timeUnit;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(schedule.DailyStartTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyDay_ShouldPass_WithRecurrenceTwo_StartDateBeforeDayInMonth_ReturnsSameMonth()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 10;
            schedule.MonthlyDayRecurrence = 2;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.FromHours(7);
            schedule.StartDate = new DateTimeOffset(2025, 3, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);

            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.False(result.IsFailure);
            Assert.Equal(2025, result.Value.ExecutionTime.Year);
            Assert.Equal(3, result.Value.ExecutionTime.Month);
            Assert.Equal(10, result.Value.ExecutionTime.Day);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyDay_ShouldPass_WithRecurrenceTwo_StartDateAfterDayInMonth_ReturnsNextRecurrenceMonth()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 10;
            schedule.MonthlyDayRecurrence = 2;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.FromHours(7);
            schedule.StartDate = new DateTimeOffset(2025, 3, 15, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);

            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.False(result.IsFailure);
            Assert.Equal(2025, result.Value.ExecutionTime.Year);
            Assert.Equal(5, result.Value.ExecutionTime.Month);
            Assert.Equal(10, result.Value.ExecutionTime.Day);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyDayDailyRange_ShouldPass_WithRangeCrossingMidnight()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 5;
            schedule.MonthlyDayRecurrence = 1;
            schedule.DailyFrequencyRangeCheckbox = true;
            schedule.DailyStartTime = new TimeSpan(23, 0, 0);
            schedule.DailyEndTime = new TimeSpan(1, 0, 0);
            schedule.TimeUnitNumberOf = 30;
            schedule.TimeUnit = Enums.TimeUnit.Minutes;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);

            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.True(result.IsFailure);
            Assert.Contains("DailyStartTime cannot be after the DailyEndTime.", result.Error);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_LastDayOfMonth()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Last;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Day;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.FromHours(18);
            schedule.StartDate = new DateTimeOffset(2025, 2, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);

            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.False(result.IsFailure);
            Assert.Equal(2025, result.Value.ExecutionTime.Year);
            Assert.Equal(2, result.Value.ExecutionTime.Month);
            Assert.Equal(28, result.Value.ExecutionTime.Day);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_LastWeekdayOfMonth()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Last;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Weekday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.FromHours(19);
            schedule.StartDate = new DateTimeOffset(2025, 2, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);

            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.False(result.IsFailure);
            Assert.Equal(2025, result.Value.ExecutionTime.Year);
            Assert.Equal(2, result.Value.ExecutionTime.Month);
            Assert.Equal(28, result.Value.ExecutionTime.Day);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_FirstWeekendDayOfMonth()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.First;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.WeekendDay;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = TimeSpan.FromHours(8);
            schedule.StartDate = new DateTimeOffset(2025, 2, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);

            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.False(result.IsFailure);
            Assert.Equal(2025, result.Value.ExecutionTime.Year);
            Assert.Equal(2, result.Value.ExecutionTime.Month);
            Assert.Equal(1, result.Value.ExecutionTime.Day);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyTheDailyRange_ShouldPass_WithSecondsRange()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.First;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Monday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyRangeCheckbox = true;
            schedule.DailyStartTime = new TimeSpan(0, 0, 0);
            schedule.DailyEndTime = new TimeSpan(0, 1, 0);
            schedule.TimeUnitNumberOf = 10;
            schedule.TimeUnit = Enums.TimeUnit.Seconds;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);

            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.False(result.IsFailure);
            Assert.Equal(2025, result.Value.ExecutionTime.Year);
            Assert.Equal(1, result.Value.ExecutionTime.Month);
            Assert.Equal(6, result.Value.ExecutionTime.Day);
            Assert.Equal(schedule.DailyStartTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyDayDailyRange_ShouldPass_WithSecondsRange_MinimumInterval()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 2;
            schedule.MonthlyDayRecurrence = 1;
            schedule.DailyFrequencyRangeCheckbox = true;
            schedule.DailyStartTime = new TimeSpan(0, 0, 0);
            schedule.DailyEndTime = new TimeSpan(0, 0, 10);
            schedule.TimeUnitNumberOf = 1;
            schedule.TimeUnit = Enums.TimeUnit.Seconds;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);

            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.False(result.IsFailure);
            Assert.Equal(2025, result.Value.ExecutionTime.Year);
            Assert.Equal(1, result.Value.ExecutionTime.Month);
            Assert.Equal(2, result.Value.ExecutionTime.Day);
            Assert.Equal(schedule.DailyStartTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyDay_ShouldFail_WhenBothDayAndTheChecked()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = Enums.ScheduleType.Recurring,
                Enabled = true,
                Occurs = Enums.Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                MonthlyFrequencyTheCheckbox = true,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.Zero,
                StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero)
            };

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.True(result.IsFailure);
            Assert.Contains("You cannot set both Day and The options on monthly configuration. Please choose only one.", result.Error);
        }

        [Fact]
        public void MonthlyValidation_ShouldFail_WhenNeitherDayNorTheChecked()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = Enums.ScheduleType.Recurring,
                Enabled = true,
                Occurs = Enums.Occurs.Monthly,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.Zero,
                StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero)
            };

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.True(result.IsFailure);
            Assert.Contains("You need to check at least one option (Day or The) for monthly configuration.", result.Error);
        }

        [Fact]
        public void MonthlyDayValidation_ShouldFail_WhenDayNumberOutOfRange()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = Enums.ScheduleType.Recurring,
                Enabled = true,
                Occurs = Enums.Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                MonthlyDayNumber = 0, // invalid
                MonthlyDayRecurrence = 1,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.Zero,
                StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero)
            };

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.True(result.IsFailure);
            Assert.Contains("Monthly day number must be between 1 and 31.", result.Error);
        }

        [Fact]
        public void MonthlyDayValidation_ShouldFail_WhenDayRecurrenceIsZero()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = Enums.ScheduleType.Recurring,
                Enabled = true,
                Occurs = Enums.Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                MonthlyDayNumber = 5,
                MonthlyDayRecurrence = 0, // invalid
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.Zero,
                StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero)
            };

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.True(result.IsFailure);
            Assert.Contains("Monthly day recurrence must be greater than 0.", result.Error);
        }

        [Fact]
        public void MonthlyDayValidation_ShouldFail_WhenTheOrderIsSet()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = Enums.ScheduleType.Recurring,
                Enabled = true,
                Occurs = Enums.Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                MonthlyDayNumber = 10,
                MonthlyDayRecurrence = 1,
                MonthlyTheOrder = Enums.MonthlyTheOrder.First, 
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.Zero,
                StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero)
            };

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.True(result.IsFailure);
            Assert.Contains("You cannot set monthly order (The Order) when using Day configuration.", result.Error);
        }

        [Fact]
        public void MonthlyDayOnceValidation_ShouldFail_WhenMissingExecutionTime()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = Enums.ScheduleType.Recurring,
                Enabled = true,
                Occurs = Enums.Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                MonthlyDayNumber = 10,
                MonthlyDayRecurrence = 1,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = null, 
                StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero)
            };

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.True(result.IsFailure);
            Assert.Contains("DailyOnceExecutionTime is required for Monthly Day Once configuration.", result.Error);
        }

        [Fact]
        public void MonthlyDayRangeValidation_ShouldFail_WhenMissingRangeFields()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = Enums.ScheduleType.Recurring,
                Enabled = true,
                Occurs = Enums.Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                MonthlyDayNumber = 10,
                MonthlyDayRecurrence = 1,
                DailyFrequencyRangeCheckbox = true,
                DailyStartTime = null,
                DailyEndTime = null,
                TimeUnit = null,
                TimeUnitNumberOf = 0, 
                StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero)
            };

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.True(result.IsFailure);
            Assert.Contains("DailyStartTime is required for Monthly Day Range configuration.", result.Error);
            Assert.Contains("DailyEndTime is required for Monthly Day Range configuration.", result.Error);
            Assert.Contains("TimeUnit is required for Monthly Day Range configuration.", result.Error);
            Assert.Contains("TimeUnitNumberOf must be greater than 0 for Monthly Day Range configuration.", result.Error);
        }

        [Fact]
        public void MonthlyTheValidation_ShouldFail_WhenOrderAndDayOfWeekMissingAndRecurrenceZero()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = Enums.ScheduleType.Recurring,
                Enabled = true,
                Occurs = Enums.Occurs.Monthly,
                MonthlyFrequencyTheCheckbox = true,
                MonthlyTheOrder = null, 
                MonthlyTheDayOfWeek = null, 
                MonthlyTheRecurrence = 0, 
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = null,
                StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero)
            };

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.True(result.IsFailure);
            Assert.Contains("Monthly order (First, Second, Third, Fourth, Last) is required for The configuration.", result.Error);
            Assert.Contains("Monthly day of week is required for The configuration.", result.Error);
            Assert.Contains("Monthly The recurrence must be greater than 0.", result.Error);
        }

        [Fact]
        public void MonthlyTheOnceValidation_ShouldFail_WhenMissingExecutionTime()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = Enums.ScheduleType.Recurring,
                Enabled = true,
                Occurs = Enums.Occurs.Monthly,
                MonthlyFrequencyTheCheckbox = true,
                MonthlyTheOrder = Enums.MonthlyTheOrder.First,
                MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Monday,
                MonthlyTheRecurrence = 1,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = null, 
                StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero)
            };

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.True(result.IsFailure);
            Assert.Contains("DailyOnceExecutionTime is required for Monthly The Once configuration.", result.Error);
        }

        [Fact]
        public void MonthlyTheRangeValidation_ShouldFail_WhenMissingRangeFields()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = Enums.ScheduleType.Recurring,
                Enabled = true,
                Occurs = Enums.Occurs.Monthly,
                MonthlyFrequencyTheCheckbox = true,
                MonthlyTheOrder = Enums.MonthlyTheOrder.Last,
                MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Friday,
                MonthlyTheRecurrence = 1,
                DailyFrequencyRangeCheckbox = true,
                DailyStartTime = null,
                DailyEndTime = null,
                TimeUnit = null,
                TimeUnitNumberOf = 0, // invalid
                StartDate = new DateTimeOffset(2025,1,1,0,0,0,0,TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025,12,31,0,0,0,0,0, TimeSpan.Zero)
            };

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.True(result.IsFailure);
            Assert.Contains("DailyStartTime is required for Monthly The Range configuration.", result.Error);
            Assert.Contains("DailyEndTime is required for Monthly The Range configuration.", result.Error);
            Assert.Contains("TimeUnit is required for Monthly The Range configuration.", result.Error);
            Assert.Contains("TimeUnitNumberOf must be greater than 0 for Monthly The Range configuration.", result.Error);
        }

        [Fact]
        public void MonthlyDay_LeapYear_Feb29_ReturnsFeb29()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = Enums.ScheduleType.Recurring,
                Enabled = true,
                Occurs = Enums.Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                MonthlyDayNumber = 29,
                MonthlyDayRecurrence = 1,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.Zero,
                StartDate = new DateTimeOffset(2024, 2, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2024, 12, 31, 0, 0, 0, TimeSpan.Zero)
            };

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.False(result.IsFailure);
            Assert.Equal(2024, result.Value.ExecutionTime.Year);
            Assert.Equal(2, result.Value.ExecutionTime.Month);
            Assert.Equal(29, result.Value.ExecutionTime.Day);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyDay_NonLeapYear_Day29_AdjustsTo28()
        {
            var schedule = new ScheduleEntity
            {
                ScheduleType = Enums.ScheduleType.Recurring,
                Enabled = true,
                Occurs = Enums.Occurs.Monthly,
                MonthlyFrequencyDayCheckbox = true,
                MonthlyDayNumber = 29,
                MonthlyDayRecurrence = 1,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.Zero,
                StartDate = new DateTimeOffset(2025, 2, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero)
            };

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.False(result.IsFailure);
            Assert.Equal(2025, result.Value.ExecutionTime.Year);
            Assert.Equal(2, result.Value.ExecutionTime.Month);
            Assert.Equal(28, result.Value.ExecutionTime.Day); 
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyThe_ThirdWeekendDay_ReturnsCorrectDate()
        {
            
            var schedule = new ScheduleEntity
            {
                ScheduleType = Enums.ScheduleType.Recurring,
                Enabled = true,
                Occurs = Enums.Occurs.Monthly,
                MonthlyFrequencyTheCheckbox = true,
                MonthlyTheOrder = Enums.MonthlyTheOrder.Third,
                MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.WeekendDay,
                MonthlyTheRecurrence = 1,
                DailyFrequencyOnceCheckbox = true,
                DailyOnceExecutionTime = TimeSpan.Zero,
                StartDate = new DateTimeOffset(2025, 3, 1, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 3, 31, 0, 0, 0, TimeSpan.Zero)
            };

            var manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);

            Assert.False(result.IsFailure);
            Assert.Equal(2025, result.Value.ExecutionTime.Year);
            Assert.Equal(3, result.Value.ExecutionTime.Month);
            Assert.Equal(8, result.Value.ExecutionTime.Day);
        }

        [Fact]
        public void MonthlyDay_ShouldPass_WithDaySevenEveryFourMonths_AtEvening()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 7;
            schedule.MonthlyDayRecurrence = 4;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = new TimeSpan(18, 45, 0);
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2026, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(2025, result.Value.ExecutionTime.Year);
            Assert.Equal(1, result.Value.ExecutionTime.Month);
            Assert.Equal(7, result.Value.ExecutionTime.Day);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyDayDailyRange_ShouldPass_Day12Every3Months_Every2Hours()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 12;
            schedule.MonthlyDayRecurrence = 3;
            schedule.DailyFrequencyRangeCheckbox = true;
            schedule.DailyStartTime = new TimeSpan(8, 0, 0);
            schedule.DailyEndTime = new TimeSpan(16, 0, 0);
            schedule.TimeUnitNumberOf = 2;
            schedule.TimeUnit = Enums.TimeUnit.Hours;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2026, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(2025, result.Value.ExecutionTime.Year);
            Assert.Equal(1, result.Value.ExecutionTime.Month);
            Assert.Equal(12, result.Value.ExecutionTime.Day);
            Assert.Equal(schedule.DailyStartTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyDayDailyRange_ShouldPass_Day28Every1Month_Every5Minutes()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 28;
            schedule.MonthlyDayRecurrence = 1;
            schedule.DailyFrequencyRangeCheckbox = true;
            schedule.DailyStartTime = new TimeSpan(10, 0, 0);
            schedule.DailyEndTime = new TimeSpan(10, 30, 0);
            schedule.TimeUnitNumberOf = 5;
            schedule.TimeUnit = Enums.TimeUnit.Minutes;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2026, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(2025, result.Value.ExecutionTime.Year);
            Assert.Equal(1, result.Value.ExecutionTime.Month);
            Assert.Equal(28, result.Value.ExecutionTime.Day);
            Assert.Equal(schedule.DailyStartTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyDayDailyRange_ShouldPass_Day15Every2Months_Every20Seconds()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyDayCheckbox = true;
            schedule.MonthlyDayNumber = 15;
            schedule.MonthlyDayRecurrence = 2;
            schedule.DailyFrequencyRangeCheckbox = true;
            schedule.DailyStartTime = new TimeSpan(14, 0, 0);
            schedule.DailyEndTime = new TimeSpan(14, 1, 0);
            schedule.TimeUnitNumberOf = 20;
            schedule.TimeUnit = Enums.TimeUnit.Seconds;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2026, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(2025, result.Value.ExecutionTime.Year);
            Assert.Equal(1, result.Value.ExecutionTime.Month);
            Assert.Equal(15, result.Value.ExecutionTime.Day);
            Assert.Equal(schedule.DailyStartTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_FirstMonday_DailyOnce()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.First;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Monday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = new TimeSpan(8, 0, 0);
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_SecondTuesday_DailyOnce()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Second;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Tuesday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = new TimeSpan(9, 15, 0);
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_ThirdWednesday_DailyOnce()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Third;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Wednesday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = new TimeSpan(10, 30, 0);
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_FourthThursday_DailyOnce()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Fourth;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Thursday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = new TimeSpan(11, 45, 0);
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_LastFriday_DailyOnce()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Last;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Friday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = new TimeSpan(12, 0, 0);
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_FirstDay_DailyOnce()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.First;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Day;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = new TimeSpan(13, 0, 0);
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_SecondWeekday_DailyOnce()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Second;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Weekday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = new TimeSpan(14, 15, 0);
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_ThirdWeekendDay_DailyOnce()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Third;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.WeekendDay;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = new TimeSpan(15, 30, 0);
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_FourthSaturday_DailyOnce()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Fourth;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Saturday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = new TimeSpan(16, 45, 0);
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyThe_ShouldPass_LastSunday_DailyOnce()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Last;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Sunday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyOnceCheckbox = true;
            schedule.DailyOnceExecutionTime = new TimeSpan(17, 0, 0);
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(schedule.DailyOnceExecutionTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyTheDailyRange_ShouldPass_FirstMonday_EveryHour()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.First;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Monday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyRangeCheckbox = true;
            schedule.DailyStartTime = new TimeSpan(8, 0, 0);
            schedule.DailyEndTime = new TimeSpan(12, 0, 0);
            schedule.TimeUnitNumberOf = 1;
            schedule.TimeUnit = Enums.TimeUnit.Hours;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(schedule.DailyStartTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyTheDailyRange_ShouldPass_SecondTuesday_Every15Minutes()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Second;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Tuesday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyRangeCheckbox = true;
            schedule.DailyStartTime = new TimeSpan(9, 0, 0);
            schedule.DailyEndTime = new TimeSpan(9, 30, 0);
            schedule.TimeUnitNumberOf = 15;
            schedule.TimeUnit = Enums.TimeUnit.Minutes;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(schedule.DailyStartTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyTheDailyRange_ShouldPass_ThirdWednesday_Every20Seconds()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Third;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Wednesday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyRangeCheckbox = true;
            schedule.DailyStartTime = new TimeSpan(10, 0, 0);
            schedule.DailyEndTime = new TimeSpan(10, 1, 0);
            schedule.TimeUnitNumberOf = 20;
            schedule.TimeUnit = Enums.TimeUnit.Seconds;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(schedule.DailyStartTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyTheDailyRange_ShouldPass_FourthThursday_EveryHour()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Fourth;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Thursday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyRangeCheckbox = true;
            schedule.DailyStartTime = new TimeSpan(11, 0, 0);
            schedule.DailyEndTime = new TimeSpan(12, 0, 0);
            schedule.TimeUnitNumberOf = 1;
            schedule.TimeUnit = Enums.TimeUnit.Hours;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(schedule.DailyStartTime, result.Value.ExecutionTime.TimeOfDay);
        }

        [Fact]
        public void MonthlyTheDailyRange_ShouldPass_LastFriday_Every30Minutes()
        {
            ScheduleEntity schedule = new ScheduleEntity();
            schedule.ScheduleType = Enums.ScheduleType.Recurring;
            schedule.Enabled = true;
            schedule.Occurs = Enums.Occurs.Monthly;
            schedule.MonthlyFrequencyTheCheckbox = true;
            schedule.MonthlyTheOrder = Enums.MonthlyTheOrder.Last;
            schedule.MonthlyTheDayOfWeek = Enums.MonthlyDayOfWeek.Friday;
            schedule.MonthlyTheRecurrence = 1;
            schedule.DailyFrequencyRangeCheckbox = true;
            schedule.DailyStartTime = new TimeSpan(13, 0, 0);
            schedule.DailyEndTime = new TimeSpan(13, 30, 0);
            schedule.TimeUnitNumberOf = 30;
            schedule.TimeUnit = Enums.TimeUnit.Minutes;
            schedule.StartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
            ScheduleManager manager = new ScheduleManager();
            var result = manager.GetNextExecution(schedule, 1);
            Assert.False(result.IsFailure);
            Assert.Equal(schedule.DailyStartTime, result.Value.ExecutionTime.TimeOfDay);
        }
    }
}
