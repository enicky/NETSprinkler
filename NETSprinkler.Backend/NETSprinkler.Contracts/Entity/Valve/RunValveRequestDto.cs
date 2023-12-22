using System;
namespace NETSprinkler.Contracts.Entity.Valve
{
	public class RunValveRequestDto
	{
		public int ValveId { get; set; }
		public int Seconds { get; set; }
	}
}

