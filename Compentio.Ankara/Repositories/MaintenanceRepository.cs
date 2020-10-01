namespace Compentio.Ankara.Repositories
{
    using Compentio.Ankara.Models.Maintenance;
    using Compentio.Ankara.Repositories.ConnectionFactory;
    using Dapper;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Transactions;

    public interface IMaintenanceRepository
    {
        Task AddLogBuffer(IEnumerable<LogEntry> logBuffer);
        Task<DatabaseVersionInfo> GetDatabaseVersion();
    }

    public class MaintenanceRepository : IMaintenanceRepository
    {
        private readonly IAdoConnectionFactory _connectionFactory;
        private readonly ICurrentUserContext _userContext;

        public MaintenanceRepository(IAdoConnectionFactory connectionFactory, ICurrentUserContext userContext)
        {
            _userContext = userContext;
            _connectionFactory = connectionFactory;
        }

        public async Task AddLogBuffer(IEnumerable<LogEntry> logBuffer)
        {
            using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using var connection = _connectionFactory.CreateConnection();

            foreach (var logEntry in logBuffer)
            {
                string query = @"INSERT INTO [app].[Logs]
                               ([Severity]
                               ,[Message]
                               ,[Logger]
                               ,[ExceptionType]
                               ,[Timestamp]
                               ,[ExceptionMessage]
                               ,[SourceName]
                               ,[UserLogin])
                         VALUES
                               (@Severity
                               ,@Message
                               ,@Logger
                               ,@ExceptionType
                               ,@Timestamp
                               ,@ExceptionMessage
                               ,@SourceName
                               ,@UserLogin)";
                await connection.ExecuteAsync(query, new 
                {
                    Severity = logEntry.Severity.ToUpper(),
                    logEntry.Message,
                    Logger = !string.IsNullOrWhiteSpace(logEntry.Logger) ? logEntry.Logger : "Angular.Gui",
                    logEntry.ExceptionMessage,
                    logEntry.Timestamp,
                    logEntry.ExceptionType,
                    SourceName = "Angular.Gui",
                    userLogin = _userContext.CurrentUser.Login
                });
            }

            scope.Complete();
        }
        public async Task<DatabaseVersionInfo> GetDatabaseVersion()
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = @"SELECT TOP(1) [Version]
                              ,[AppliedOn]
                              ,[Description]
                          FROM [dbo].[VersionInfo]
                          ORDER BY AppliedOn DESC";

            var result = await connection.QuerySingleAsync<DatabaseVersionInfo>(query).ConfigureAwait(false);
            return result;
        }
    }
}
