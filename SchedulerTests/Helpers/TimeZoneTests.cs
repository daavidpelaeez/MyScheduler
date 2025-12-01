using System;
using System.Collections.Generic;
using System.Linq;

using MyScheduler.Domain.Services;


namespace MyScheduler.Helpers
{
    public class TimeZoneTests
    {
        [Theory]
        [InlineData("W. Europe Standard Time", "W. Europe Standard Time")]
        [InlineData("Romance Standard Time", "Romance Standard Time")]
        [InlineData("Central European Standard Time", "Central European Standard Time")]
        [InlineData("GMT Standard Time", "GMT Standard Time")]
        [InlineData("Unknown", "Central European Standard Time")]
        public void GetTimeZoneID_ReturnsExpected(string input, string expected)
        {
            var result = TimeZoneService.GetTimeZoneID(input);
            Assert.Equal(expected, result);
        }
    }
}
