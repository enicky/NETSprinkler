using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
using NETSprinkler.ApiWorker.Business.Settings;
using NETSprinkler.Common.DbContext;
using NETSprinkler.Contracts.Settings;

namespace NETSprinkler.ApiWorker.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController: Controller
	{
        private readonly ILogger<SettingsController> logger;

        private readonly ISettingsManager _settingsManager;
        private readonly IUnitOfWork _unitOfWork;

        public SettingsController(ILogger<SettingsController> logger, ISettingsManager settingsManager,
            IUnitOfWork unitOfWork)
		{
            this.logger = logger;
            _settingsManager = settingsManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("mac")]
        public IActionResult GetMacAddress(CancellationToken token)
        {

            string firstMacAddress = _settingsManager.GetMacAddressString();
            return Json(new { mac = firstMacAddress });
        }

        [HttpGet("all")]
        public IActionResult GetAllSettings(CancellationToken token)
        {
            var allSettings = new AllSettings
            {
                FirmwareVersion = "1.0.0",
                FirmwareMinorVersion = "0",
                LastRebootCause = "None",
                LastRebootCauseName = "None",
                MacAddress = _settingsManager.GetMacAddressString(),
                UpTime = _settingsManager.GetUptimeValue(),
                Valves = _settingsManager.GetAllValvesAsync(),
                LastRebootTime = _settingsManager.LastRebootTime()
            };

            return Json(allSettings);
        }

        [HttpGet("disable")]
        public async Task<IActionResult> DisableController(CancellationToken cancellationToken)
        {
            logger.LogInformation($"[SettingsController:DisableController] Start stopping and disabling all valves");
            var disableControllerResult = await _settingsManager.DisableController(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Ok(new {sucess = disableControllerResult});
        }

        [HttpGet("enable")]
        public async Task<IActionResult> EnableController(CancellationToken cancellationToken)
        {
            logger.LogInformation($"[SettingsController:EnableController] Start enabling all valves");
            var enableControllerResult = await _settingsManager.EnableController(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Ok(new {success = enableControllerResult});
        }
    }
}

