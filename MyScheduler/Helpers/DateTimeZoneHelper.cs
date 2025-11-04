using System;
using System.Linq;

namespace MyScheduler.Helpers
{
    public static class DateTimeZoneHelper
    {
        public static DateTimeOffset ToDateTimeOffset(DateTime day, TimeSpan time)
        {
            var tzId = TimeZoneInfo.Local.Id;
            var tz = TimeZoneInfo.FindSystemTimeZoneById(tzId);

            var localDateTime = day.Date + time;

            if (tz.IsAmbiguousTime(localDateTime))
            {
                var offsets = tz.GetAmbiguousTimeOffsets(localDateTime);
                return new DateTimeOffset(localDateTime, offsets.Max());
            }
            else
            {
                var offset = tz.GetUtcOffset(localDateTime);
                return new DateTimeOffset(localDateTime, offset);
            }

        }
        
    }
}
