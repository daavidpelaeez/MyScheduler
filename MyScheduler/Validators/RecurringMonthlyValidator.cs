using MyScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public class RecurringMonthlyValidator
    {
        public static void Validate(ScheduleEntity scheduleConfig, StringBuilder errors)
        {

            if (scheduleConfig.DailyFrequencyOnceCheckbox || scheduleConfig.DailyFrequencyRangeCheckbox)
            {
                DailyFrequencyCommonRules.CheckDailyCommonRules(scheduleConfig, errors);
            }

            ValidateMutualExclusivity(scheduleConfig, errors);

            if (scheduleConfig.MonthlyFrequencyDayCheckbox)
            {
                ValidateDayConfiguration(scheduleConfig, errors);
                

                if (scheduleConfig.DailyFrequencyOnceCheckbox)
                {
                    ValidateDayOnceConfiguration(scheduleConfig, errors);
                }
                else if (scheduleConfig.DailyFrequencyRangeCheckbox)
                {
                    ValidateDayRangeConfiguration(scheduleConfig, errors);
                }
            }

            if (scheduleConfig.MonthlyFrequencyTheCheckbox)
            {
                ValidateTheConfiguration(scheduleConfig, errors);
                

                if (scheduleConfig.DailyFrequencyOnceCheckbox)
                {
                    ValidateTheOnceConfiguration(scheduleConfig, errors);
                }
                else if (scheduleConfig.DailyFrequencyRangeCheckbox)
                {
                    ValidateTheRangeConfiguration(scheduleConfig, errors);
                }
            }
        }

        private static void ValidateMutualExclusivity(ScheduleEntity scheduleConfig, StringBuilder errors)
        {

            if (scheduleConfig.MonthlyFrequencyDayCheckbox && scheduleConfig.MonthlyFrequencyTheCheckbox)
            {
                errors.AppendLine("You cannot set both Day and The options on monthly configuration. Please choose only one.");
            }

            if (!scheduleConfig.MonthlyFrequencyDayCheckbox && !scheduleConfig.MonthlyFrequencyTheCheckbox)
            {
                errors.AppendLine("You need to check at least one option (Day or The) for monthly configuration.");
            }
        }

        private static void ValidateDayConfiguration(ScheduleEntity scheduleConfig, StringBuilder errors)
        {

            if (scheduleConfig.MonthlyDayNumber < 1 || scheduleConfig.MonthlyDayNumber > 31)
            {
                errors.AppendLine("Monthly day number must be between 1 and 31.");
            }

   
            if (scheduleConfig.MonthlyDayRecurrence < 1)
            {
                errors.AppendLine("Monthly day recurrence must be greater than 0.");
            }


            if (scheduleConfig.MonthlyTheOrder != null)
            {
                errors.AppendLine("You cannot set monthly order (The Order) when using Day configuration.");
            }

            if (scheduleConfig.MonthlyTheDayOfWeek != null)
            {
                errors.AppendLine("You cannot set monthly day of week when using Day configuration.");
            }

            if (scheduleConfig.MonthlyTheRecurrence > 0)
            {
                errors.AppendLine("You cannot set The recurrence when using Day configuration.");
            }
        }

        private static void ValidateDayOnceConfiguration(ScheduleEntity scheduleConfig, StringBuilder errors)
        {

            if (!scheduleConfig.DailyOnceExecutionTime.HasValue)
            {
                errors.AppendLine("DailyOnceExecutionTime is required for Monthly Day Once configuration.");
            }
        }

        private static void ValidateDayRangeConfiguration(ScheduleEntity scheduleConfig, StringBuilder errors)
        {

            if (!scheduleConfig.DailyStartTime.HasValue)
            {
                errors.AppendLine("DailyStartTime is required for Monthly Day Range configuration.");
            }

            if (!scheduleConfig.DailyEndTime.HasValue)
            {
                errors.AppendLine("DailyEndTime is required for Monthly Day Range configuration.");
            }

            if (!scheduleConfig.TimeUnit.HasValue)
            {
                errors.AppendLine("TimeUnit is required for Monthly Day Range configuration.");
            }

            if (scheduleConfig.TimeUnitNumberOf < 1)
            {
                errors.AppendLine("TimeUnitNumberOf must be greater than 0 for Monthly Day Range configuration.");
            }


            if (scheduleConfig.DailyStartTime.HasValue && scheduleConfig.DailyEndTime.HasValue)
            {
                if (scheduleConfig.DailyStartTime.Value > scheduleConfig.DailyEndTime.Value)
                {
                    errors.AppendLine("DailyStartTime cannot be after the DailyEndTime.");
                }

                if (scheduleConfig.DailyStartTime.Value == scheduleConfig.DailyEndTime.Value)
                {
                    errors.AppendLine("DailyStartTime cannot be the same as DailyEndTime.");
                }
            }
        }

        private static void ValidateTheConfiguration(ScheduleEntity scheduleConfig, StringBuilder errors)
        {

            if (!scheduleConfig.MonthlyTheOrder.HasValue)
            {
                errors.AppendLine("Monthly order (First, Second, Third, Fourth, Last) is required for The configuration.");


                if (!scheduleConfig.MonthlyTheDayOfWeek.HasValue)
                {
                    errors.AppendLine("Monthly day of week is required for The configuration.");
                }


                if (scheduleConfig.MonthlyTheRecurrence < 1)
                {
                    errors.AppendLine("Monthly The recurrence must be greater than 0.");
                }


                if (scheduleConfig.MonthlyDayNumber > 0)
                {
                    errors.AppendLine("You cannot set monthly day number when using The configuration.");
                }

                if (scheduleConfig.MonthlyDayRecurrence > 0)
                {
                    errors.AppendLine("You cannot set Day recurrence when using The configuration.");
                }
            }
        }

        private static void ValidateTheOnceConfiguration(ScheduleEntity scheduleConfig, StringBuilder errors)
        {

            if (!scheduleConfig.DailyOnceExecutionTime.HasValue)
            {
                errors.AppendLine("DailyOnceExecutionTime is required for Monthly The Once configuration.");
            }
        }

        private static void ValidateTheRangeConfiguration(ScheduleEntity scheduleConfig, StringBuilder errors)
        {

            if (!scheduleConfig.DailyStartTime.HasValue)
            {
                errors.AppendLine("DailyStartTime is required for Monthly The Range configuration.");
            }

            if (!scheduleConfig.DailyEndTime.HasValue)
            {
                errors.AppendLine("DailyEndTime is required for Monthly The Range configuration.");
            }

            if (!scheduleConfig.TimeUnit.HasValue)
            {
                errors.AppendLine("TimeUnit is required for Monthly The Range configuration.");
            }

            if (scheduleConfig.TimeUnitNumberOf < 1)
            {
                errors.AppendLine("TimeUnitNumberOf must be greater than 0 for Monthly The Range configuration.");
            }


        }
    }
}
