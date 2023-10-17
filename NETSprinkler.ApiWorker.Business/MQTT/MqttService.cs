using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using NETSprinkler.Models.Entity.Valve;
using Newtonsoft.Json;

namespace NETSprinkler.ApiWorker.Business.MQTT
{
    public class MqttService : IMqttService
    {
        private readonly IManagedMqttClient _managedMqttClient;
        private readonly MqttClientOptions _options;
        private readonly ILogger<MqttService> _logger;

        private static string MqttSprinklerStatusTopic = "sprinkler/valve/status";
        private static string MqttSprinklerCommandStart = "sprinkler/valve/cmd/start";

        

        public MqttService(MqttClientOptions options, ILogger<MqttService> logger)
		{
            _options = options;
            _logger = logger;
            var mqttFactory = new MqttFactory();
			_managedMqttClient = mqttFactory.CreateManagedMqttClient();


            subscribeToCommandTopics().ConfigureAwait(false).GetAwaiter().GetResult() ;
        }

        private async Task subscribeToCommandTopics()
        {
            var topicFilter = new MqttTopicFilterBuilder()
                .WithTopic(MqttSprinklerCommandStart)
                .Build();
            _managedMqttClient.ApplicationMessageReceivedAsync +=  (MqttApplicationMessageReceivedEventArgs arg) =>
            {
                var topic = arg.ApplicationMessage.Topic;
                _logger.LogInformation($"Received message on topic {topic} -> {arg.ApplicationMessage.ConvertPayloadToString()}");        
                return Task.CompletedTask;
            };
            await _managedMqttClient.SubscribeAsync(new List<MqttTopicFilter> { topicFilter});

        }

        public async Task SendStatus(SprinklerStatus sprinklerStatus)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(MqttSprinklerStatusTopic)
                .WithPayload(JsonConvert.SerializeObject(sprinklerStatus))
                .Build();
            await _managedMqttClient.EnqueueAsync(message);
        }

        public async Task StartMqttClient(CancellationToken token = default)
		{
           /* var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(_options.Server)
                .WithCredentials(mqttOptions.UserName, mqttOptions.Password)
                .WithClientId("sprinkler")
                .Build();*/
            var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(_options)
                .Build();


            
		}

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(_options)
                .Build();
            await _managedMqttClient.StartAsync(managedMqttClientOptions);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _managedMqttClient.StopAsync(true);

        }
    }
}

