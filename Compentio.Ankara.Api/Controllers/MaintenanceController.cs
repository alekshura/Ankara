namespace Compentio.Ankara.Api.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Compentio.Ankara.Api.Filters;
    using Compentio.Ankara.Models.Maintenance;
    using Compentio.Ankara.Models.Users;
    using Compentio.Ankara.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    [ApiController]
    [Authorize]
    [Route("api/maintenance")]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly HealthCheckService _healthCheckService;

        public MaintenanceController(IMaintenanceService maintenanceService, HealthCheckService healthCheckService)
        {
            _maintenanceService = maintenanceService;
            _healthCheckService = healthCheckService;
        }
        /// <summary>
        /// Method adds logs to the logs table. It is used by Frontend application.
        /// </summary>
        /// <returns></returns>
        [HttpPost("")]
        [ProducesResponseType(typeof(IEnumerable<IActionResult>), 200)]
        public async Task<IActionResult> AddLogBuffer(IEnumerable<LogEntry> logBuffer)
        {
            await _maintenanceService.AddLogBuffer(logBuffer)
                .ConfigureAwait(false);

            return Ok();
        }
        /// <summary>
        /// Method returns application versions: Database actual version and assembly version.
        /// </summary>
        /// <returns></returns>
        [HttpGet("version")]
        [ProducesResponseType(typeof(VersionInfo), 200)]
        public async Task<IActionResult> GetVersion()
        {
            var version = await _maintenanceService.GetVersion()
                .ConfigureAwait(false);

            return Ok(version);
        }
        /// <summary>
        /// Methods shows API health condition.
        /// </summary>
        /// <returns></returns>
        [HttpGet("healthcheck")]
        [ProducesResponseType(typeof(IActionResult), 200)]
        [RoleAuthorization(Roles.TechAdmin)]
        public async Task<IActionResult> Get()
        {
            var report = await _healthCheckService.CheckHealthAsync().ConfigureAwait(false);
            return report.Status == HealthStatus.Healthy ? Ok(report) : StatusCode((int)HttpStatusCode.ServiceUnavailable, report);
        }
    }
}
