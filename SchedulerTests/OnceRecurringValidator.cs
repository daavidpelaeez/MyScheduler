using MyScheduler.Entities;
using MyScheduler.Enums;
using MyScheduler.Services;
using MyScheduler.Validators;

namespace MyScheduler
{
    public class OnceRecurringValidator
    {
        [Fact]
        public void EventSuccessWithoutEndDate_OnceTask()
        {
            var schedule = new ScheduleEntity();

            schedule.Type = Enums.Type.Once;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            schedule.EventDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);

            var result = Validator.ValidateTask(schedule,10);

            Assert.True(result.IsSuccess);
            Assert.Equal(schedule, result.Value);
        }

        [Fact]
        public void EventDateNull_ShouldFail()
        {
            var schedule = new ScheduleEntity();

            schedule.Type = Enums.Type.Once;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);

            var result = Validator.ValidateTask(schedule,10);

            Assert.True(result.IsFailure);
            Assert.Contains("EventDate is required for Once tasks.", result.Error);
        }

        [Fact]
        public void RecurrenceLessThanOne_ShouldFail()
        {
            var schedule = new ScheduleEntity();

            schedule.Type = Enums.Type.Recurring;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            schedule.Recurrence = 0;

            var result = Validator.ValidateTask(schedule, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("Recurring tasks must have a recurrence greater than 0.", result.Error);
        }

        [Fact]
        public void RecurrenceLessThan0_ShouldFail()
        {
            var schedule = new ScheduleEntity();

            schedule.Type = Enums.Type.Recurring;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            schedule.Recurrence = -1;

            var result = Validator.ValidateTask(schedule    , 10);

            Assert.True(result.IsFailure);
            Assert.Contains("Recurring tasks must have a recurrence greater than 0.", result.Error);
        }

        [Fact]
        public void EventDateBeforeTheStartDate_ShouldFail()
        {
            var schedule = new ScheduleEntity();

            schedule.Type = Enums.Type.Recurring;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            schedule.EventDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            schedule.Recurrence = 2;

            var result = Validator.ValidateTask(schedule, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("The event date cannot be before the start date.", result.Error);
        }

        [Fact]
        public void EvenDateLaterThanEndDate_ShouldFail()
        {
            var schedule = new ScheduleEntity();

            schedule.Type = Enums.Type.Recurring;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.EventDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            schedule.Recurrence = 2;

            var result = Validator.ValidateTask(schedule, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("The event date cannot be after the end date.", result.Error);
        }

        [Fact]
        public void StartDateAfterEndDate_ShouldFail()
        {
            var schedule = new ScheduleEntity();

            schedule.Type = Enums.Type.Recurring;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            schedule.EventDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            schedule.Recurrence = 2;

            var result = Validator.ValidateTask(schedule, 10);
            Assert.True(result.IsFailure, result.Error);
            Assert.Contains("The start date cannot be after the end date.", result.Error);
        }

        [Fact]
        public void EventDateInThePast_ShouldFail()
        {
            var schedule = new ScheduleEntity();

            schedule.Type = Enums.Type.Recurring;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero);
            schedule.EventDate = new DateTimeOffset(2025, 10, 7, 0, 0, 0, TimeSpan.Zero);
            schedule.Recurrence = 2;

            var result = Validator.ValidateTask(schedule, 10);
            Assert.True(result.IsFailure);
            Assert.Contains("The event date must be in the future.", result.Error);
        }

        [Fact]
        public void OnceTask_WithEndDateAfterEventDate_ShouldPass()
        {
            var schedule = new ScheduleEntity();
            schedule.Type = Enums.Type.Once;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            schedule.EventDate = new DateTimeOffset(2025, 10, 10, 14, 30, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero);

            var result = Validator.ValidateTask(schedule, 10);

            Assert.True(result.IsSuccess);
            Assert.Equal(schedule, result.Value);
        }

        [Fact]
        public void OnceTask_EventDateAfterEndDate_ShouldFail()
        {
            var schedule = new ScheduleEntity();
            schedule.Type = Enums.Type.Once;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            schedule.EventDate = new DateTimeOffset(2025, 10, 12, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero);

            var result = Validator.ValidateTask(schedule, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("after the end date", result.Error.ToLower());
        }

        [Fact]
        public void RecurringTask_EndDateBeforeCurrentDate_ShouldFail()
        {
            var schedule = new ScheduleEntity();
            schedule.Type = Enums.Type.Recurring;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            schedule.Recurrence = 1;

            var result = Validator.ValidateTask(schedule, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("end date", result.Error.ToLower());
        }

        [Fact]
        public void RecurringTask_WithoutEventDate_WithoutEndDate_RecurrencePositive_ShouldPass()
        {
            var schedule = new ScheduleEntity();
            schedule.Type = Enums.Type.Recurring;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            schedule.Recurrence = 3;

            var result = Validator.ValidateTask(schedule, 10);

            Assert.True(result.IsSuccess);
            Assert.Equal(schedule, result.Value);
        }

        [Fact]
        public void RecurringTask_StartDateAfterEndDate_ShouldFail()
        {
            var schedule = new ScheduleEntity();
            schedule.Type = Enums.Type.Recurring;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 4, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            schedule.Recurrence = 2;

            var result = Validator.ValidateTask(schedule, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("start date cannot be after the end date", result.Error.ToLower());
        }

        [Fact]
        public void OnceTask_WithRecurrenceGreaterThanZero_ShouldFail()
        {
            var schedule = new ScheduleEntity();
            schedule.Type = Enums.Type.Once;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            schedule.EventDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            schedule.Recurrence = 1;

            var result = Validator.ValidateTask(schedule, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("once", result.Error.ToLower());
        }

        [Fact]
        public void EventDateInPast_ShouldFail()
        {
            var schedule = new ScheduleEntity();
            schedule.Type = Enums.Type.Recurring;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            schedule.EventDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            schedule.Recurrence = 1;

            var result = Validator.ValidateTask(schedule, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("The event date must be in the future.", result.Error);
        }

        [Fact]
        public void RecurringTask_EndDateEqualsStartDate_ShouldPass()
        {
            var schedule = new ScheduleEntity();

            schedule.Type = Enums.Type.Recurring;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            schedule.Recurrence = 1;

            var result = Validator.ValidateTask(schedule, 10);

            Assert.True(result.IsSuccess);
            Assert.Equal(schedule, result.Value);
        }

        [Fact]
        public void RecurringTask_WithoutEventDate_ShouldPass()
        {
            var schedule = new ScheduleEntity();

            schedule.Type = Enums.Type.Recurring;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            schedule.Recurrence = 2;

            var result = Validator.ValidateTask(schedule, 10);

            Assert.True(result.IsSuccess);
            Assert.Equal(schedule, result.Value);
        }

        [Fact]
        public void EventDateEqualsCurrentDate_ShouldPass()
        {
            var schedule = new ScheduleEntity();

            schedule.Type = Enums.Type.Once;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 10, 10, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 10, 9, 0, 0, TimeSpan.Zero);
            schedule.EventDate = new DateTimeOffset(2025, 10, 10, 10, 0, 0, TimeSpan.Zero);

            var result = Validator.ValidateTask(schedule, 10);

            Assert.True(result.IsSuccess);
            Assert.Equal(schedule, result.Value);
        }

        [Fact]
        public void EndDateEqualsCurrentDate_ShouldPass()
        {
            var schedule = new ScheduleEntity();

            schedule.Type = Enums.Type.Recurring;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 5, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            schedule.Recurrence = 1;

            var result = Validator.ValidateTask(schedule, 10);

            Assert.True(result.IsSuccess);
            Assert.Equal(schedule, result.Value);
        }

        [Fact]
        public void MultipleErrors_ShouldReturnAllErrorMessages()
        {
            var schedule = new ScheduleEntity();

            schedule.Type = Enums.Type.Once;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 11, 0, 0, 0, TimeSpan.Zero);
            schedule.EventDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            schedule.Recurrence = 1;

            var result = Validator.ValidateTask(schedule, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("The event date must be in the future.", result.Error);
            Assert.Contains("The event date cannot be before the start date.", result.Error);
            Assert.Contains("Once tasks cannot have a recurrence", result.Error);
        }


        [Fact]
        public void EventDateEqualsEndDate_ShouldPass()
        {
            var schedule = new ScheduleEntity();

            schedule.Type = Enums.Type.Recurring;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.EventDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            schedule.EndDate = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);
            schedule.Recurrence = 1;

            var result = Validator.ValidateTask(schedule, 10);

            Assert.True(result.IsSuccess);
            Assert.Equal(schedule, result.Value);
        }

        [Fact]
        public void Recurrence_MoreThanLimit_ShouldFail()
        {
            var schedule = new ScheduleEntity();

            schedule.Type = Enums.Type.Recurring;
            schedule.CurrentDate = new DateTimeOffset(2025, 10, 8, 0, 0, 0, TimeSpan.Zero);
            schedule.StartDate = new DateTimeOffset(2025, 10, 9, 0, 0, 0, TimeSpan.Zero);
            schedule.Recurrence = 1200;

            var result = Validator.ValidateTask(schedule, 10);

            Assert.True(result.IsFailure);
            Assert.Contains("Recurrence cannot be more than 1000.", result.Error);
        }

        


    }
}
