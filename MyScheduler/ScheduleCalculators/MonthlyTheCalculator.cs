using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyScheduler.ScheduleCalculators
{
    public class MonthlyTheCalculator
    {
        public Result<ScheduleOutput> GetOutput(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var dates = CalculateExecutions(scheduleConfig, numOccurrences);
            return (dates.Count > 0) ? Result<ScheduleOutput>.Success(OutputHelper.OutputBuilder(dates.First(), DescriptionGenerator.GetDescription(scheduleConfig))) :
                Result<ScheduleOutput>.Failure("No next execution found");
        }

        public List<DateTimeOffset> CalculateExecutions(ScheduleEntity scheduleConfig, int? numOccurrences)
        {
            var resultList = new List<DateTimeOffset>();
            var order = scheduleConfig.MonthlyTheOrder!.Value;
            var dayOfWeek = scheduleConfig.MonthlyTheDayOfWeek!.Value;
            int monthRecurrence = scheduleConfig.MonthlyTheRecurrence!.Value;
            DateTimeOffset endDate = scheduleConfig.EndDate ?? DateTimeOffset.MaxValue;
            for (var currentMonth = scheduleConfig.StartDate;
                 currentMonth <= endDate && (numOccurrences == null || resultList.Count < numOccurrences);
                 currentMonth = currentMonth.AddMonths(monthRecurrence))
            {
                var targetDate = GetTargetDateInMonth(currentMonth, order, dayOfWeek);
                if (targetDate.HasValue && targetDate.Value >= scheduleConfig.StartDate)
                    resultList.Add(targetDate.Value);
            }
            return resultList;
        }

        private DateTimeOffset? GetTargetDateInMonth(DateTimeOffset baseDate, MonthlyTheOrder order, MonthlyDayOfWeek dayOfWeek)
        {
            var matchingDays = GetMatchingDaysInMonth(baseDate,dayOfWeek);
            return SelectDayByOrder(matchingDays, order);
        }

        private List<DateTimeOffset> GetMatchingDaysInMonth(DateTimeOffset baseDate, MonthlyDayOfWeek dayOfWeek)
        {
            var matchingDays = new List<DateTimeOffset>();
            int daysInMonth = DateTime.DaysInMonth(baseDate.Year, baseDate.Month);
            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTimeOffset(baseDate.Year, baseDate.Month, day, baseDate.Hour, baseDate.Minute, baseDate.Second, baseDate.Offset);
                if (IsMatchingDay(date, dayOfWeek))
                    matchingDays.Add(date);
            }
            return matchingDays;
        }

        private DateTimeOffset? SelectDayByOrder(List<DateTimeOffset> matchingDays, MonthlyTheOrder order)
        {
            if (matchingDays.Count < 1)
                return null;
            switch (order)
            {
                case MonthlyTheOrder.First:
                    return matchingDays.Count > 0 ? matchingDays[0] : (DateTimeOffset?)null;
                case MonthlyTheOrder.Second:
                    return matchingDays.Count > 1 ? matchingDays[1] : (DateTimeOffset?)null;
                case MonthlyTheOrder.Third:
                    return matchingDays.Count > 2 ? matchingDays[2] : (DateTimeOffset?)null;
                case MonthlyTheOrder.Fourth:
                    return matchingDays.Count > 3 ? matchingDays[3] : (DateTimeOffset?)null;
                case MonthlyTheOrder.Last:
                    return matchingDays[matchingDays.Count - 1];
                default:
                    return null;
            }
        }

        private bool IsMatchingDay(DateTimeOffset date, MonthlyDayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case MonthlyDayOfWeek.Day:
                    return true;
                case MonthlyDayOfWeek.Weekday:
                    return date.DayOfWeek >= DayOfWeek.Monday && date.DayOfWeek <= DayOfWeek.Friday;
                case MonthlyDayOfWeek.WeekendDay:
                    return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
                case MonthlyDayOfWeek.Monday: return date.DayOfWeek == DayOfWeek.Monday;
                case MonthlyDayOfWeek.Tuesday: return date.DayOfWeek == DayOfWeek.Tuesday;
                case MonthlyDayOfWeek.Wednesday: return date.DayOfWeek == DayOfWeek.Wednesday;
                case MonthlyDayOfWeek.Thursday: return date.DayOfWeek == DayOfWeek.Thursday;
                case MonthlyDayOfWeek.Friday: return date.DayOfWeek == DayOfWeek.Friday;
                case MonthlyDayOfWeek.Saturday: return date.DayOfWeek == DayOfWeek.Saturday;
                case MonthlyDayOfWeek.Sunday: return date.DayOfWeek == DayOfWeek.Sunday;
                default:
                    return false;
            }
        }
    }
}
