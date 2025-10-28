using MyScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Validators
{
    public class DailyEveryValidator
    {
        public static void Validate(ScheduleEntity scheduleConfig, StringBuilder errors)
        {
            DailyFrequencyCommonRules.CheckDailyCommonRules(scheduleConfig, errors);

        }
    }
}
