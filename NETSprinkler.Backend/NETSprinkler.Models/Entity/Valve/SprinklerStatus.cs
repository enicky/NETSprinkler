namespace NETSprinkler.Models.Entity.Valve
{
    public class SprinklerStatus
	{
		public SprinklerStatus()
		{
		}

        public int SprinklerId { get; set; }
        public SprinklerState Status { get; set; }
    }

    public enum SprinklerState
    {
        Open,
        Closed
    }
}

