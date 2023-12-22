using Microsoft.Extensions.Logging;
using NETSprinkler.ApiWorker.Business.Services.Sprinkler;

namespace NETSprinkler.ApiWorker.Business.Jobs
{
	public class RunManuallySprinklerJob
	{
        private readonly ILogger<RunManuallySprinklerJob> _logger;
        private readonly ISprinklerService _sprinklerService;

        public RunManuallySprinklerJob(ILogger<RunManuallySprinklerJob> logger, ISprinklerService sprinklerService)
		{
            _logger = logger;
			_sprinklerService = sprinklerService;
		}

		public async Task Run(int valveId, int seconds)
		{
			_logger.LogDebug($"[RunManuallySprinklerJob:Run] Start running a job manually on valveId {valveId} for {seconds} seconds");
			await _sprinklerService.StartAsync(valveId);
			await Task.Delay(TimeSpan.FromSeconds(seconds));
			await _sprinklerService.StopAsync(valveId);
            _logger.LogDebug($"[RunManuallySprinklerJob:Run] Finished running manually job on {valveId}");
        }
    }
}

