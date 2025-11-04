using MyScheduler.Entities;
using MyScheduler.Helpers;
using MyScheduler.ScheduleCalculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public static class RecurringWeeklyOnceValidator
    {
        public static void Validate(ScheduleEntity scheduleConfig, StringBuilder errors)
        {
            var recurringWeeklyOnceCalculator = new RecurringWeeklyOnceCalculator();

            DailyFrequencyCommonRules.CheckDailyCommonRules(scheduleConfig, errors);

            if (scheduleConfig.DaysOfWeek.Count < 1)
                errors.AppendLine("DaysOfWeek must be selected for RecurringWeeklyOnce.");

            if(scheduleConfig.DailyOnceExecutionTime < TimeSpan.Zero)
                errors.AppendLine("DailyOnceExecutionTime is wrong");

            if (scheduleConfig.WeeklyRecurrence < 1)
                errors.AppendLine("WeeklyRecurrence must be at least 1.");

            if (WeeklyScheduleHelper.SameWeekDayChecker(scheduleConfig.DaysOfWeek!))
                errors.AppendLine("Check days of the week they cant be repeated");


        }
    }
}
