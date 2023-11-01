using Azure.Core;
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
        private const string MqttSprinklerStateTopic = "sprinkler/valve/1/status";
        private const string MqttSprinklerCommandStart = "sprinkler/valve/cmd/start";
        private const string MqttSprinklerCommandStop = "sprinkler/valve/cmd/stop";

        private const string MqttSprinklerGeneralCommands = "sprinkler/valve/commands";
        private const string MqttSprinklerValve1CommandTopic = "sprinkler/valve/1/command";
        private const string MqttSprinklerAvailabilityTopic = "sprinkler/valve/available";



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
            _logger.LogInformation($"[MqttService:subscribteToCommandTopics] Subscribing to start and stop");
            var topicFilter = new MqttTopicFilterBuilder()
                .WithTopic(MqttSprinklerCommandStart)
                .Build();
            var topicFilterStopCommand = new MqttTopicFilterBuilder().WithTopic(MqttSprinklerCommandStop).Build();
            var topicFilterGeneralCommands = new MqttTopicFilterBuilder().WithTopic(MqttSprinklerGeneralCommands).Build();
            var topicFilterValve1Commands = new MqttTopicFilterBuilder().WithTopic(MqttSprinklerValve1CommandTopic).Build();

            _managedMqttClient.ApplicationMessageReceivedAsync += async (MqttApplicationMessageReceivedEventArgs arg) =>
            {
                var topic = arg.ApplicationMessage.Topic;

                switch (topic)
                {
                    case MqttSprinklerCommandStart:
                        _logger.LogInformation($"[Mqtt:HandleMessage] Starting sprinkler ...");
                        await ProcessStartSprinklerCommand(arg.ApplicationMessage.ConvertPayloadToString());
                        break;
                    case MqttSprinklerCommandStop:
                        _logger.LogInformation("[Mqtt:HandleMessage] Stopping Sprinkler");
                        await ProcessStopSprinklerCommand(arg.ApplicationMessage.ConvertPayloadToString());
                        break;
                    case MqttSprinklerGeneralCommands:
                        _logger.LogInformation($"[Mqtt:HandleMessage] received General Command");
                        await ProcessGeneralCommand(arg.ApplicationMessage.ConvertPayloadToString());
                        break;
                    case MqttSprinklerValve1CommandTopic:
                        _logger.LogInformation("[Mqtt:HandleMessage] Received command for valve 1");
                        await ProcessValve1Command(arg.ApplicationMessage.ConvertPayloadToString()).ConfigureAwait(false);
                        break;
                        
                };
                _logger.LogInformation($"Received message on topic {topic} -> {arg.ApplicationMessage.ConvertPayloadToString()}");        
                
            };
            await _managedMqttClient.SubscribeAsync(new List<MqttTopicFilter> { topicFilter, topicFilterStopCommand, topicFilterGeneralCommands,
                topicFilterValve1Commands});

        }

        private async Task ProcessValve1Command(string v)
        {
            _logger.LogInformation($"[Mqtt:ProcessValve1Command] processing command for valve 1 : {v}");
            using var scope = _scopeFactory.CreateAsyncScope();
            var _sprinklerService = scope.ServiceProvider.GetRequiredService<ISprinklerService>();
            SprinklerState status = SprinklerState.Open;
            if (v.ToLower() == "off") { 
                await _sprinklerService.StopAsync(1).ConfigureAwait(false);
                status = SprinklerState.Closed;
            }
            else if (v.ToLower() == "on")
                await _sprinklerService.StartAsync(1).ConfigureAwait(false);

            await SendStatus(new SprinklerStatus { SprinklerId = 1, Status = status }).ConfigureAwait(false);
        }

        private Task ProcessGeneralCommand(string v)
        {
            switch (v)
            {
                case "restart":
                    _logger.LogInformation($"[Mqtt:ProcessGeneralCommand] Restarting software ... ");
                    break;
            }
            return Task.CompletedTask;
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
            await SendStateToStateTopic(sprinklerStatus.Status).ConfigureAwait(false);
            
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(MqttSprinklerStatusTopic)
                .WithRetainFlag(true)
                .WithPayload(JsonConvert.SerializeObject(sprinklerStatus))
                .Build();
            var x = new ManagedMqttApplicationMessageBuilder().WithApplicationMessage(message).Build();
            await _managedMqttClient.EnqueueAsync(x);
        }

        private async Task SendStateToStateTopic(SprinklerState status)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(MqttSprinklerStateTopic)
                .WithRetainFlag(true)
                .WithPayload(status == SprinklerState.Open ? "ON" : "OFF")
                .Build();
            await _managedMqttClient.EnqueueAsync(message).ConfigureAwait(false);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(_options)
                .Build();
            await _managedMqttClient.StartAsync(managedMqttClientOptions).ConfigureAwait(false);
            await SendAvailabilityStatus(true).ConfigureAwait(false);
            await SendStatus(new SprinklerStatus { SprinklerId = 1, Status = SprinklerState.Closed }).ConfigureAwait(false);
        }

        private async Task SendAvailabilityStatus(bool status)
        {
            var msg = new MqttApplicationMessageBuilder()
                .WithTopic(MqttSprinklerAvailabilityTopic)
                .WithRetainFlag(false)
                .WithPayload(status.ToString())
                .Build();
            await _managedMqttClient.EnqueueAsync(new ManagedMqttApplicationMessageBuilder().WithApplicationMessage(msg).Build());
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await SendAvailabilityStatus(false).ConfigureAwait(false);
            await _managedMqttClient.StopAsync(true);

        }
    }
}

