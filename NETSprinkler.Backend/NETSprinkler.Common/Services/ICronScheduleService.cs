using NETSprinkler.Models.Entity.Schedule;

namespace NETSprinkler.Common.Services;

public interface ICronScheduleService
{
    Task<string> CreateCronString(Schedule registeredSchedule, bool isEndCron = false);
    Task<(string, string)> GenerateCronStrings(Schedule registeredSchedule);
}