namespace Compentio.Ankara.Services
{
    using Compentio.Ankara.Models.Maintenance;
    using Compentio.Ankara.Repositories;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMaintenanceService
    {
        Task AddLogBuffer(IEnumerable<LogEntry> logBuffer, string userLogin);
    }
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IMaintenanceRepository _maintenanceRepository;

        public MaintenanceService(IMaintenanceRepository maintenanceRepository)
        {
            _maintenanceRepository = maintenanceRepository;
        }

        public async Task AddLogBuffer(IEnumerable<LogEntry> logBuffer, string userLogin)
        {
            await _maintenanceRepository.AddLogBuffer(logBuffer, userLogin).ConfigureAwait(false);
        }
    }
}
