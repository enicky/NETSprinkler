namespace NETSprinkler.Contracts.Scheduler
{
    public class SetNameRequestDto
	{
		public string Name { get; set; } = string.Empty;
		public int ScheduleId { get; set; }
	}
}

