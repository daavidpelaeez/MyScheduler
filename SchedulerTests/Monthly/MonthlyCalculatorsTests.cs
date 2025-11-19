//using MyScheduler.Entities;
//using MyScheduler.Enums;
//using MyScheduler.ScheduleCalculators;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MyScheduler
//{
//    public class MonthlyCalculatorsTests
//    {

//        [Fact]
//        public void MonthlyDayCalculator()
//        {
//            ScheduleEntity schedule = new ScheduleEntity();

//            schedule.StartDate = new DateTimeOffset(2025,7,3,0,0,0,TimeSpan.Zero);
//            schedule.EndDate = new DateTimeOffset(2026,3,3,0,0,0,TimeSpan.Zero);
//            schedule.MonthlyDayNumber = 7;
//            schedule.MonthlyDayRecurrence = 2;

//            MonthlyDayCalculator mdc = new MonthlyDayCalculator();

//            List<DateTimeOffset> result = mdc.AddHourToList(schedule,10);

//            List<DateTimeOffset> expected = new List<DateTimeOffset> { new DateTimeOffset(2025,10,5,0,0,0,TimeSpan.Zero) };

//            Assert.Equal(expected, result);
//        }


//        [Fact]
//        public void MonthlyTheCalculator()
//        {
//            ScheduleEntity schedule = new ScheduleEntity();

//            schedule.StartDate = new DateTimeOffset(2025,7, 6, 0, 0, 0, TimeSpan.Zero);
//            schedule.EndDate = new DateTimeOffset(2026, 3, 3, 0, 0, 0, TimeSpan.Zero);
//            schedule.MonthlyTheDayOfWeek = MonthlyDayOfWeek.WeekendDay;
//            schedule.MonthlyTheOrder = MonthlyTheOrder.First;
//            schedule.MonthlyTheRecurrence = 1;

//            MonthlyTheCalculator mtc = new MonthlyTheCalculator();

//            List<DateTimeOffset> result = mtc.AddHourToList(schedule, 10);

//            List<DateTimeOffset> expected = new List<DateTimeOffset> { new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero) };

//            Assert.Equal(expected, result);


//        }

//        [Fact]
//        public void MonthlyDayDailyOnceCalculator()
//        {
//            ScheduleEntity schedule = new ScheduleEntity();

//            schedule.StartDate = new DateTimeOffset(2025, 7, 6, 0, 0, 0, TimeSpan.Zero);
//            schedule.EndDate = new DateTimeOffset(2026, 3, 3, 0, 0, 0, TimeSpan.Zero);
//            schedule.MonthlyDayNumber = 7;
//            schedule.MonthlyDayRecurrence = 2;
//            schedule.DailyOnceExecutionTime = new TimeSpan(15,0,0);

//            MonthlyDayDailyOnceCalculator mtc = new MonthlyDayDailyOnceCalculator();

//            List<DateTimeOffset> result = mtc.AddHourToList(schedule, 10);

//            List<DateTimeOffset> expected = new List<DateTimeOffset> { new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero) };

//            Assert.Equal(expected, result);


//        }

//        [Fact]
//        public void MonthlyDayDailyRangeCalculator()
//        {
//            ScheduleEntity schedule = new ScheduleEntity();

//            schedule.StartDate = new DateTimeOffset(2025, 7, 6, 0, 0, 0, TimeSpan.Zero);
//            schedule.EndDate = new DateTimeOffset(2026, 3, 3, 0, 0, 0, TimeSpan.Zero);
//            schedule.MonthlyDayNumber = 7;
//            schedule.MonthlyDayRecurrence = 2;
//            schedule.TimeUnit = TimeUnit.Hours;
//            schedule.TimeUnitNumberOf = 3;
//            schedule.DailyStartTime = new TimeSpan(15,0,0);
//            schedule.DailyEndTime = new TimeSpan(19, 0, 0);


//            MonthlyDayDailyRangeCalculator mtc = new MonthlyDayDailyRangeCalculator();

//            List<DateTimeOffset> result = mtc.CalculateExecutions(schedule, 10);

//            List<DateTimeOffset> expected = new List<DateTimeOffset> { new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero) };

//            Assert.Equal(expected, result);

//        }

//        [Fact]
//        public void MonthlyTheDailyOnceCalculatorTest()
//        {
//            ScheduleEntity schedule = new ScheduleEntity();

//            schedule.StartDate = new DateTimeOffset(2025, 7, 6, 0, 0, 0, TimeSpan.Zero);
//            schedule.EndDate = new DateTimeOffset(2026, 3, 3, 0, 0, 0, TimeSpan.Zero);
//            schedule.MonthlyTheDayOfWeek = MonthlyDayOfWeek.Monday;
//            schedule.MonthlyTheOrder = MonthlyTheOrder.Third;
//            schedule.MonthlyTheRecurrence = 1;
//            schedule.DailyOnceExecutionTime = new TimeSpan(15,0,0);

//            MonthlyTheDailyOnceCalculator mtc = new MonthlyTheDailyOnceCalculator();

//            List<DateTimeOffset> result = mtc.AddHourToList(schedule, 10);

//            List<DateTimeOffset> expected = new List<DateTimeOffset> { new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero) };

//            Assert.Equal(expected, result);


//        }


//        [Fact]
//        public void MonthlyTheDailyRangeCalculator()
//        {
//            ScheduleEntity schedule = new ScheduleEntity();

//            schedule.StartDate = new DateTimeOffset(2025, 7, 6, 0, 0, 0, TimeSpan.Zero);
//            schedule.EndDate = new DateTimeOffset(2026, 3, 3, 0, 0, 0, TimeSpan.Zero);
//            schedule.MonthlyTheDayOfWeek = MonthlyDayOfWeek.Friday;
//            schedule.MonthlyTheOrder= MonthlyTheOrder.First;
//            schedule.MonthlyTheRecurrence = 2;
//            schedule.TimeUnit = TimeUnit.Hours;
//            schedule.TimeUnitNumberOf = 5;
//            schedule.DailyStartTime = new TimeSpan(15, 0, 0);
//            schedule.DailyEndTime = new TimeSpan(19, 0, 0);

//            MonthlyTheDailyRangeCalculator mtc = new MonthlyTheDailyRangeCalculator();

//            List<DateTimeOffset> result = mtc.CalculateExecutions(schedule, 10);

//            List<DateTimeOffset> expected = new List<DateTimeOffset> { new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero) };

//            Assert.Equal(expected, result);

//        }



//    }
//}
