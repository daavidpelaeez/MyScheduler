using System;
using System.Collections.Generic;
using MyScheduler.Domain.Enums;
using MyScheduler.Domain.Services.Calculators;
using Xunit;

namespace SchedulerTests.Monthly
{
    public class MonthlyTheCalculatorTests
    {
        private MonthlyTheCalculator calculator = new MonthlyTheCalculator();

        private List<DateTimeOffset> GetDates(int count)
        {
            var list = new List<DateTimeOffset>();
            for (int i = 0; i < count; i++)
                list.Add(new DateTimeOffset(2025, 1, i + 1, 0, 0, 0, TimeSpan.Zero));
            return list;
        }

        [Fact]
        public void SelectDayByOrder_ReturnsNull_WhenListIsEmpty()
        {
            var result = InvokeSelectDayByOrder(new List<DateTimeOffset>(), MonthlyTheOrder.First);
            Assert.Null(result);
        }

        [Fact]
        public void SelectDayByOrder_First_ReturnsFirst()
        {
            var result = InvokeSelectDayByOrder(GetDates(1), MonthlyTheOrder.First);
            Assert.Equal(GetDates(1)[0], result);
        }

        [Fact]
        public void SelectDayByOrder_Second_ReturnsSecondOrNull()
        {
            Assert.Null(InvokeSelectDayByOrder(GetDates(1), MonthlyTheOrder.Second));
            Assert.Equal(GetDates(2)[1], InvokeSelectDayByOrder(GetDates(2), MonthlyTheOrder.Second));
        }

        [Fact]
        public void SelectDayByOrder_Third_ReturnsThirdOrNull()
        {
            Assert.Null(InvokeSelectDayByOrder(GetDates(2), MonthlyTheOrder.Third));
            Assert.Equal(GetDates(3)[2], InvokeSelectDayByOrder(GetDates(3), MonthlyTheOrder.Third));
        }

        [Fact]
        public void SelectDayByOrder_Fourth_ReturnsFourthOrNull()
        {
            Assert.Null(InvokeSelectDayByOrder(GetDates(3), MonthlyTheOrder.Fourth));
            Assert.Equal(GetDates(4)[3], InvokeSelectDayByOrder(GetDates(4), MonthlyTheOrder.Fourth));
        }

        [Fact]
        public void SelectDayByOrder_Last_ReturnsLast()
        {
            Assert.Equal(GetDates(1)[0], InvokeSelectDayByOrder(GetDates(1), MonthlyTheOrder.Last));
            Assert.Equal(GetDates(4)[3], InvokeSelectDayByOrder(GetDates(4), MonthlyTheOrder.Last));
        }

        [Fact]
        public void SelectDayByOrder_Default_ReturnsNull()
        {
            // Simulate an invalid enum value
            var result = InvokeSelectDayByOrder(GetDates(2), (MonthlyTheOrder)999);
            Assert.Null(result);
        }

        private DateTimeOffset? InvokeSelectDayByOrder(List<DateTimeOffset> days, MonthlyTheOrder order)
        {
            var method = typeof(MonthlyTheCalculator).GetMethod("SelectDayByOrder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (DateTimeOffset?)method.Invoke(calculator, new object[] { days, order });
        }
    }
}
