using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Services;
using System.Threading.Tasks;


namespace MyScheduler
{
    public class WeeklyTests
    {
        [Fact]
        public void WeeklyOnce_DaysOfWeekEmpty_ShouldFail()
        {
            var taskConfig = new TaskEntity();
            taskConfig.TypeTask = TypeTask.WeeklyOnce;
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.ExecutionTimeOfOneDay = new TimeSpan(13,30,0);

            var taskManager = new TaskManager();
            var result = taskManager.GetNextExecution(taskConfig,10);
            Assert.True(result.IsFailure);
            Assert.Contains("DaysOfWeek must be selected for WeeklyOnce.", result.Error);
        }

        [Fact]
        public void WeeklyEvery_DaysOfWeekEmpty_ShouldFail()
        {
            var taskConfig = new TaskEntity();
            taskConfig.TypeTask = TypeTask.WeeklyEvery;
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);

            var taskManager = new TaskManager();
            var result = taskManager.GetNextExecution(taskConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("DaysOfWeek must be selected for WeeklyEvery.", result.Error);
        }

        [Fact]
        public void WeeklyOnce_WeeklyRecurrenceEmpty_ShouldFail()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var taskConfig = new TaskEntity();
            taskConfig.TypeTask = TypeTask.WeeklyOnce;
            taskConfig.DaysOfWeek= listOfDays;
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);

            var taskManager = new TaskManager();
            var result = taskManager.GetNextExecution(taskConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("WeeklyRecurrence must be at least 1.", result.Error);
        }

        [Fact]
        public void WeeklyEvery_WeeklyRecurrenceEmpty_ShouldFail()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var taskConfig = new TaskEntity();
            taskConfig.TypeTask = TypeTask.WeeklyEvery;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);

            var taskManager = new TaskManager();
            var result = taskManager.GetNextExecution(taskConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("WeeklyRecurrence must be at least 1.", result.Error);
        }

        [Fact]
        public void WeeklyOnce_ExecutionTimeOfOneDayNull_ShouldFail()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var taskConfig = new TaskEntity();
            taskConfig.TypeTask = TypeTask.WeeklyOnce;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.WeeklyRecurrence = 2;
            taskConfig.ExecutionTimeOfOneDay = null;

            var taskManager = new TaskManager();
            var result = taskManager.GetNextExecution(taskConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("ExecutionTimeOfOneDay is required for WeeklyOnce.", result.Error);
        }

        [Fact]
        public void WeeklyEvery_NotTimeUnitSelected_ShouldFail()
        {
            List<DayOfWeek> listOfDays = new List<DayOfWeek>();
            listOfDays.Add(DayOfWeek.Monday);

            var taskConfig = new TaskEntity();
            taskConfig.TypeTask = TypeTask.WeeklyEvery;
            taskConfig.DaysOfWeek = listOfDays;
            taskConfig.CurrentDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            taskConfig.StartDate = new DateTimeOffset(2025, 10, 15, 0, 0, 0, TimeSpan.Zero);
            taskConfig.EndDate = new DateTimeOffset(2025, 10, 17, 0, 0, 0, TimeSpan.Zero);
            taskConfig.WeeklyRecurrence = 2;
            

            var taskManager = new TaskManager();
            var result = taskManager.GetNextExecution(taskConfig, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("TimeUnit is required for WeeklyEvery.", result.Error);
        }



       






    }
}
