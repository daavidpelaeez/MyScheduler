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

            var offset = timeZone.GetUtcOffset(localDateTime);
            return new DateTimeOffset(localDateTime, offset);
        }
        
    }
}
