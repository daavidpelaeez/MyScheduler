using Xunit;
using MyScheduler.Infraestructure.Localizer;

namespace SchedulerTests.Helpers
{
    public class LocalizerTests
    {
        [Fact]
        public void GetString_ReturnsKey_WhenKeyNotFound()
        {
            var localizer = new Localizer();
            var result = localizer.GetString("NonExistentKey", "en-US");
            Assert.Equal("NonExistentKey", result);
        }
    }
}
