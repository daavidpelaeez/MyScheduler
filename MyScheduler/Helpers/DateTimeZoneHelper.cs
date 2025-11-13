using System;
using System.Linq;

namespace MyScheduler.Helpers
{
    public static class DateTimeZoneHelper
    {
        public static DateTimeOffset ToTimeZoneOffset(DateTime day, TimeSpan time)
        {
            var timeZoneID = TimeZoneInfo.Local.Id;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneID);

            var localDateTime = day.Date + time;

            if (timeZone.IsAmbiguousTime(localDateTime))
            {
                var offsets = timeZone.GetAmbiguousTimeOffsets(localDateTime);
                return new DateTimeOffset(localDateTime, offsets.Max());
            }
            else
            {
                var offset = timeZone.GetUtcOffset(localDateTime);
                return new DateTimeOffset(localDateTime, offset);
            }

        }
        
    }
}
