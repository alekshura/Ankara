namespace Compentio.Ankara.Services
{
    using Compentio.Ankara.Models.Maintenance;
    using Compentio.Ankara.Repositories;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    public interface IMaintenanceService
    {
        Task AddLogBuffer(IEnumerable<LogEntry> logBuffer);
        Task<VersionInfo> GetVersion();
    }
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IMaintenanceRepository _maintenanceRepository;

        public MaintenanceService(IMaintenanceRepository maintenanceRepository)
        {
            _maintenanceRepository = maintenanceRepository;
        }

        public async Task AddLogBuffer(IEnumerable<LogEntry> logBuffer)
        {
            await _maintenanceRepository.AddLogBuffer(logBuffer).ConfigureAwait(false);
        }
        public async Task<VersionInfo> GetVersion()
        {
            var versionInfo = new VersionInfo()
            {
                AssemblyVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion
            };

            var datadaseVersion = await _maintenanceRepository.GetDatabaseVersion().ConfigureAwait(false);
            versionInfo.DatabaseVersion = datadaseVersion;
            return versionInfo;
        }
    }
}
