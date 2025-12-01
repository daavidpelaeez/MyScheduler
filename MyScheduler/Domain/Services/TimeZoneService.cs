using MyScheduler.Domain.Entities;
using System;

namespace MyScheduler.Domain.Services
{
    public static class TimeZoneService
    {
        public static DateTimeOffset ToTimeZoneOffset(DateTime day, TimeSpan time, string timeZoneID)
        {
            var ID = GetTimeZoneID(timeZoneID);
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(ID);

            var localDateTime = day.Date + time;

            var offset = timeZone.GetUtcOffset(localDateTime);

            return new DateTimeOffset(localDateTime, offset);
        }

        public static string GetTimeZoneID(string timeZoneID)
        {
            switch (timeZoneID)
            {
                case "W. Europe Standard Time":
                    return "W. Europe Standard Time";  
                case "Romance Standard Time":
                    return "Romance Standard Time";   
                case "Central European Standard Time":
                    return "Central European Standard Time"; 
                case "GMT Standard Time":
                    return "GMT Standard Time"; 
                default:
                    return "Central European Standard Time"; 
            }
        }

    }
}
