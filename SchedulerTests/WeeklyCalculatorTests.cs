//using MyScheduler.Entities;
//using MyScheduler.Enums;
//using MyScheduler.Services;
//using System.Threading.Tasks;


//namespace MyScheduler
//{
//    public class WeeklyCalculatorTests
//    {

//        [Fact]
//        public void checkWeeklyOnceOneDescription()
//        {
//            var listOfDays = new List<DayOfWeek>();

//            listOfDays.Add(DayOfWeek.Monday);
//            listOfDays.Add(DayOfWeek.Tuesday);
//            listOfDays.Add(DayOfWeek.Sunday);

//            var taskManager = new TaskManager();

//            var taskEntity = new TaskEntity();
//            taskEntity.TypeTask = TypeTask.WeeklyOnce;
//            taskEntity.CurrentDate = DateTime.Now;
//            taskEntity.DaysOfWeek = listOfDays;
//            String descripcion = taskManager.GetWeeklyDescription(listOfDays);
//            String expectedDescription = "monday, tuesday and sunday";

//            Assert.Equal(expectedDescription, descripcion);


//        }

//        [Fact]
//        public void checkWeeklyOnceFullDescription()
//        {
//            var listOfDays = new List<DayOfWeek>();

//            listOfDays.Add(DayOfWeek.Monday);
//            listOfDays.Add(DayOfWeek.Tuesday);
//            listOfDays.Add(DayOfWeek.Sunday);
            
//            var taskManager = new TaskManager();

//            var taskEntity = new TaskEntity();
//            taskEntity.TypeTask = TypeTask.WeeklyOnce;
//            taskEntity.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);
//            taskEntity.DaysOfWeek = listOfDays;
//            taskEntity.WeeklyRecurrence = 4;
//            taskEntity.StartDate = DateTime.Now;
//            String descripcion = taskManager.GetDescription(taskEntity);
//            String expectedDescription = $"Occurs every 2 weeks on monday, tuesday and sunday at 13:30:00" +
//                    $" starting on 15/10/2025 ";

//            Assert.Equal(expectedDescription,descripcion);

//        }


//        [Fact]
//        public void checkGetRecurrenceDaysWeeklyOnce()
//        {
//            var listOfDays = new List<DayOfWeek>();
//            var listOfDaysToHardCheck = new List<DateTimeOffset>();

//            listOfDays.Add(DayOfWeek.Monday);
//            listOfDays.Add(DayOfWeek.Tuesday);
//            listOfDays.Add(DayOfWeek.Sunday);

//            listOfDaysToHardCheck.Add(new DateTimeOffset(2025, 10, 13, 13, 30, 0, TimeSpan.Zero));
//            listOfDaysToHardCheck.Add(new DateTimeOffset(2025, 10, 14, 13, 30, 0, TimeSpan.Zero));
//            listOfDaysToHardCheck.Add(new DateTimeOffset(2025, 10, 19, 13, 30, 0, TimeSpan.Zero));
//            listOfDaysToHardCheck.Add(new DateTimeOffset(2025, 11, 03, 13, 30, 0, TimeSpan.Zero));

//            var taskManager = new TaskManager();

//            var taskEntity = new TaskEntity();
//            taskEntity.CurrentDate = new DateTimeOffset(2025, 10, 13, 0, 0, 0, TimeSpan.Zero);
//            taskEntity.TypeTask = TypeTask.WeeklyOnce;
//            taskEntity.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);
//            taskEntity.DaysOfWeek = listOfDays;
//            taskEntity.WeeklyRecurrence = 3;
//            taskEntity.StartDate = new DateTimeOffset(2025, 10, 13, 0, 0, 0, TimeSpan.Zero);
//            taskEntity.EndDate = null;
            
//            var listOfDaysObtained = taskManager.CalculateWeeklyOnceConfig(taskEntity,4);

//            Assert.Equal(listOfDaysObtained, listOfDaysToHardCheck);

//        }

//        [Fact]
//        public void checkGetNextExecutionWeekly()
//        {
//            var listOfDays = new List<DayOfWeek>();
//            var listOfDaysToHardCheck = new List<DateTimeOffset>();

//            listOfDaysToHardCheck.Add(new DateTimeOffset(2025, 10, 14, 13, 30, 0, TimeSpan.Zero));
//            listOfDaysToHardCheck.Add(new DateTimeOffset(2025, 10, 19, 13, 30, 0, TimeSpan.Zero));
//            listOfDaysToHardCheck.Add(new DateTimeOffset(2025, 10, 20, 13, 30, 0, TimeSpan.Zero));
//            listOfDaysToHardCheck.Add(new DateTimeOffset(2025, 10, 21, 13, 30, 0, TimeSpan.Zero));

//            listOfDays.Add(DayOfWeek.Monday);
//            listOfDays.Add(DayOfWeek.Tuesday);
//            listOfDays.Add(DayOfWeek.Sunday);

//            var taskManager = new TaskManager();

//            var taskEntity = new TaskEntity();
//            taskEntity.CurrentDate = new DateTimeOffset(2025, 10, 13, 0, 0, 0, TimeSpan.Zero);
//            taskEntity.TypeTask = TypeTask.WeeklyOnce;
//            taskEntity.ExecutionTimeOfOneDay = new TimeSpan(13, 30, 0);
//            taskEntity.DaysOfWeek = listOfDays;
//            taskEntity.WeeklyRecurrence = 2;
//            taskEntity.StartDate = new DateTimeOffset(2025, 10,13, 0, 0, 0, TimeSpan.Zero);
//            taskEntity.EndDate = new DateTimeOffset(2025, 10, 25, 0, 0, 0, TimeSpan.Zero);

//            var listOfDaysObtained = taskManager.CalculateWeeklyOnceConfig(taskEntity,10);

//            //var result = taskManager.GetNextExecutionWeeklyOnce(taskEntity);

//            //Assert.Equal(listOfDaysObtained, listOfDaysToHardCheck);
//            //Assert.True(result.IsSuccess);
//            //Assert.Equal(result.Value.ExecutionTime, listOfDaysToHardCheck[0]);

//        }





//    }
//}
