using NETSprinkler.Models.Entity.Schedule;

namespace NETSprinkler.Common.Services;

public class CronScheduleService : ICronScheduleService
{
    public Task<string> CreateCronString(Schedule registeredSchedule, bool isEndCron = false)
    {
        //"*/5 * * * *"
        string cronString = string.Empty;
        if(!isEndCron)
            cronString = $"0 {registeredSchedule.StartMinute} {registeredSchedule.StartHour} * * {string.Join(',', registeredSchedule.DaysToRun.Select(q => (int)q))}";
        else
        {
            cronString =
                $"0 {registeredSchedule.EndMinute} {registeredSchedule.EndHour} * * {string.Join(',', registeredSchedule.DaysToRun.Select(q => (int)q))}";
        }
        return Task.FromResult(cronString);
    }

    public async Task<(string, string)> GenerateCronStrings(Schedule registeredSchedule)
    {
        var startCronString = await CreateCronString(registeredSchedule);
        var endCronString = await CreateCronString(registeredSchedule, true);
        return (startCronString, endCronString);
    }
}