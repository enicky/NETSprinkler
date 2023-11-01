using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet.Client;
using NETSprinkler.ApiWorker.Business.MQTT;

namespace NETSprinkler.ApiWorker.Business.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddMqttClientHostedService(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddMqttClientServiceWithConfig(optionsBuilder =>
			{
				//var clientSettinigs = AppSettingsProvider.ClientSettings;
				//var brokerHostSettings = AppSettingsProvider.BrokerHostSettings;
				var server = configuration.GetSection("Mqtt")["Server"];
                var userName = configuration.GetSection("Mqtt")["UserName"];
				var password = configuration.GetSection("Mqtt")["Password"];
				var clientId = configuration.GetSection("Mqtt")["ClientId"];
                optionsBuilder
					.WithTcpServer(server)
					.WithCredentials(userName, password)
					.WithClientId(clientId)
					.WithKeepAlivePeriod(TimeSpan.FromSeconds(3));
			
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

