using MyScheduler.Entities;
using MyScheduler.Helpers;
using System.Text;

namespace MyScheduler.Validators
{
    public static class RecurringWeeklyRangeValidator
    {
        public static void Validate(ScheduleEntity scheduleConfig, StringBuilder errors)
        {
            DailyFrequencyCommonRules.CheckDailyCommonRules(scheduleConfig, errors);

            if (scheduleConfig.DaysOfWeek == null || scheduleConfig.DaysOfWeek.Count < 1)
            {
                errors.AppendLine("DaysOfWeek must be selected for RecurringWeeklyRange.");
                return;
            }

            if (!scheduleConfig.TimeUnitNumberOf.HasValue || scheduleConfig.TimeUnitNumberOf < 1)
                errors.AppendLine("TimeUnitNumberOf must be positive for Weekly Range configuration.");

            if (scheduleConfig.WeeklyRecurrence < 1)
                errors.AppendLine("WeeklyRecurrence must be at least 1.");

            if (scheduleConfig.TimeUnit == null)
                errors.AppendLine("TimeUnit is required for RecurringWeeklyRange.");

            if (scheduleConfig.TimeUnitNumberOf < 1)
                errors.AppendLine("TimeUnitNumberOf must be a positive number for RecurringWeeklyRange.");

            if (scheduleConfig.DailyStartTime == null)
                errors.AppendLine("DailyStartTime is required for RecurringWeeklyRange.");

            if (scheduleConfig.DailyEndTime == null)
                errors.AppendLine("DailyEndTime is required for RecurringWeeklyRange.");

            if (scheduleConfig.DailyStartTime > scheduleConfig.DailyEndTime)
                errors.AppendLine("DailyStartTime cannot be after the DailyEndTime.");

            if (scheduleConfig.DailyStartTime == scheduleConfig.DailyEndTime)
                errors.AppendLine("DailyStartTime cannot be the same as DailyEndTime.");

            if (WeeklyScheduleHelper.SameWeekDayChecker(scheduleConfig.DaysOfWeek))
                errors.AppendLine("Check days of the week they cant be repeated");


        }
    }
}
