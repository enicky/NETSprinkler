using System;
namespace NETSprinkler.Contracts.Entity.Valve
{
	public class EnableValveRequestDto
	{
		public int ValveId { get; set; }
		public bool EnableValve { get; set; }

	}
}

