using NETSprinkler.Models.Entity.Schedule;

namespace NETSprinkler.Common.Services;

public class CronSchduleService: ICronScheduleService
{
    public Task<string> CreateCronString(Schedule registeredSchedule)
    {
        //"*/5 * * * *"
        var str = $"0 {registeredSchedule.StartMinute} {registeredSchedule.StartHour} * * {string.Join(',', registeredSchedule.DaysToRun)}";
        return Task.FromResult(str);
    }
}