using Xunit;
using System.Text;
using MyScheduler.Domain.Entities;
using MyScheduler.Domain.Enums;
using MyScheduler.Domain.Validators;

namespace SchedulerTests.Helpers
{
    public class CommonRulesValidatorsTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("fr")] // idioma no soportado
        public void Validate_Language_Invalid_AddsError(string language)
        {
            var schedule = new ScheduleEntity {
                Language = language,
                TimeZoneID = "W. Europe Standard Time",
                Enabled = true,
                StartDate = DateTimeOffset.Now,
                CurrentDate = DateTimeOffset.Now
            };
            var errors = new StringBuilder();
            CommonRules.Validate(schedule, errors, 1);
            Assert.Contains("You need to choose an available Language", errors.ToString());
        }

        [Fact]
        public void Validate_TimeZoneID_Empty_AddsError()
        {
            var schedule = new ScheduleEntity {
                Language = "en-US",
                TimeZoneID = "",
                Enabled = true,
                StartDate = DateTimeOffset.Now,
                CurrentDate = DateTimeOffset.Now
            };
            var errors = new StringBuilder();
            CommonRules.Validate(schedule, errors, 1);
            Assert.Contains("You need to specify a time zone ID", errors.ToString());
        }



        
    }
}
