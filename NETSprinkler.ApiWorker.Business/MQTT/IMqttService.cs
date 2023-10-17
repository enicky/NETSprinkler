using System;
using Microsoft.Extensions.Hosting;
using NETSprinkler.Models.Entity.Valve;

namespace NETSprinkler.ApiWorker.Business.MQTT
{
	public interface IMqttService : IHostedService
	{
        Task SendStatus(SprinklerStatus sprinklerStatus);
        Task StartMqttClient(CancellationToken token = default);

    }
}

