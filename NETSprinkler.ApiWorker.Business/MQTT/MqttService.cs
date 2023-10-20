using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using NETSprinkler.ApiWorker.Business.Services.Sprinkler;
using NETSprinkler.Contracts.Entity.Mqtt;
using NETSprinkler.Models.Entity.Valve;
using Newtonsoft.Json;

namespace NETSprinkler.ApiWorker.Business.MQTT
{
    public class MqttService : IMqttService
    {
        private readonly IManagedMqttClient _managedMqttClient;
        private readonly MqttClientOptions _options;
        private readonly ILogger<MqttService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        
        private static string MqttSprinklerStatusTopic = "sprinkler/valve/status";
        private const string MqttSprinklerCommandStart = "sprinkler/valve/cmd/start";
        private const string MqttSprinklerCommandStop = "sprinkler/valve/cmd/stop";



        public MqttService(MqttClientOptions options, ILogger<MqttService> logger, IServiceScopeFactory scopeFactory)
		{
            _options = options;
            _logger = logger;
            this._scopeFactory = scopeFactory;
            var mqttFactory = new MqttFactory();
			_managedMqttClient = mqttFactory.CreateManagedMqttClient();


            subscribeToCommandTopics().ConfigureAwait(false).GetAwaiter().GetResult() ;
        }

        private async Task subscribeToCommandTopics()
        {
            var topicFilter = new MqttTopicFilterBuilder()
                .WithTopic(MqttSprinklerCommandStart)
                .Build();
            var topicFilterStopCommand = new MqttTopicFilterBuilder().WithTopic(MqttSprinklerCommandStop).Build();
            _managedMqttClient.ApplicationMessageReceivedAsync += async (MqttApplicationMessageReceivedEventArgs arg) =>
            {
                var topic = arg.ApplicationMessage.Topic;

                switch (topic)
                {
                    case MqttSprinklerCommandStart:
                        _logger.LogInformation($"[Mqtt] Starting sprinkler ...");
                        await ProcessStartSprinklerCommand(arg.ApplicationMessage.ConvertPayloadToString());
                        break;
                    case MqttSprinklerCommandStop:
                        _logger.LogInformation("[Mqtt:HandleMessage] Stopping Sprinkler");
                        await ProcessStopSprinklerCommand(arg.ApplicationMessage.ConvertPayloadToString());
                        break;
                        
                };
                _logger.LogInformation($"Received message on topic {topic} -> {arg.ApplicationMessage.ConvertPayloadToString()}");        
                
            };
            await _managedMqttClient.SubscribeAsync(new List<MqttTopicFilter> { topicFilter, topicFilterStopCommand});

        }

        private async Task ProcessStopSprinklerCommand(string content)
        {
            var request = JsonConvert.DeserializeObject<MqttStopSprinklerRequest>(content);
            _logger.LogDebug($"[MqttService:ProcessStopSprinklerCommand] Stopping sprinkler with id {request.ValveId}");
            using var scope = _scopeFactory.CreateAsyncScope();
            var _sprinklerService = scope.ServiceProvider.GetRequiredService<ISprinklerService>();
            await _sprinklerService.StopAsync(request.ValveId);
            _logger.LogDebug($"[MqttService:ProcessStopSprinklerCommand] Sprinkler stopped");
        }

        private async Task ProcessStartSprinklerCommand(string content)
        {
            
            var request = JsonConvert.DeserializeObject<MqttStartSprinklerRequest>(content);
            _logger.LogDebug($"[MqttService:ProcessStartSprinklerCommand] Starting sprinkler with id {request.ValveId}");
            using var scope = _scopeFactory.CreateAsyncScope();
            var _sprinklerService =  scope.ServiceProvider.GetRequiredService<ISprinklerService>();

            await _sprinklerService.StartAsync(request.ValveId);
            _logger.LogDebug("[MqttService:ProcessStartSprinklerCommand] Sprinkler Started");
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

