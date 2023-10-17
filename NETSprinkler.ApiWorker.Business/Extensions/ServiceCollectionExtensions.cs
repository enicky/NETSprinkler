using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet.Client;
using NETSprinkler.ApiWorker.Business.MQTT;

namespace NETSprinkler.ApiWorker.Business.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddMqttClientHostedService(this IServiceCollection services)
		{
			services.AddMqttClientServiceWithConfig(optionsBuilder =>
			{
				//var clientSettinigs = AppSettingsProvider.ClientSettings;
				//var brokerHostSettings = AppSettingsProvider.BrokerHostSettings;
				optionsBuilder
			.WithTcpServer("192.168.1.154")
			.WithCredentials("enicky", "Aveve2008")
			.WithClientId("sprinkler");
			
                //optionsBuilder
                //.WithCredentials(clientSettinigs.UserName, clientSettinigs.Password)
                //.WithClientId(clientSettinigs.Id)
                //WithTcpServer(brokerHostSettings.Host, brokerHostSettings.Port);
            });
			return services;
        }

		private static IServiceCollection AddMqttClientServiceWithConfig(this IServiceCollection services, Action<MqttClientOptionsBuilder> configure)
		{
			services.AddSingleton<MqttClientOptions>(serviceProvider =>
			{
				var optionsBuilder = new MqttClientOptionsBuilder();
				configure(optionsBuilder);
				return optionsBuilder.Build();
			});
			services.AddSingleton<MqttService>();
			services.AddSingleton<IHostedService>(serviceProvider =>
			{
				return serviceProvider.GetRequiredService<MqttService>();

			});
			services.AddSingleton<MqttClientServiceProvider>(serviceProvider =>
			{
				var mqttClientService = serviceProvider.GetRequiredService<MqttService>();
				var mqttClientServiceProvider = new MqttClientServiceProvider(mqttClientService);
				return mqttClientServiceProvider;
			});
			return services;
		}

    }
}

