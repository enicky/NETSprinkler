using System;
namespace NETSprinkler.ApiWorker.Business.MQTT
{
	public class MqttClientServiceProvider
	{
        public readonly IMqttService MqttService;

        public MqttClientServiceProvider(IMqttService mqttService)
		{
            MqttService = mqttService;
        }
	}
}

