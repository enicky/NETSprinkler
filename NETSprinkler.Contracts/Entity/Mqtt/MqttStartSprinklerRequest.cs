using System;
namespace NETSprinkler.Contracts.Entity.Mqtt
{
	public class MqttStartSprinklerRequest
    {
        public int SprinklerId { get; set; } = 0;
        public ValveStatus Status { get; set; }
    }

    public enum ValveStatus
    {
        Open,
        Closed
    }
}

