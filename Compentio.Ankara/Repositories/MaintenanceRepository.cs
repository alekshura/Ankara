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
        Task AddLogBuffer(IEnumerable<LogEntry> logBuffer, string userLogin);

    }
    public class MaintenanceRepository : IMaintenanceRepository
    {
        private readonly IAdoConnectionFactory _connectionFactory;

        public MaintenanceRepository(IAdoConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddLogBuffer(IEnumerable<LogEntry> logBuffer, string userLogin)
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
                    Logger = !string.IsNullOrWhiteSpace(logEntry.Logger) ? logEntry.Logger : "Compentio.Ankara.Angular",
                    logEntry.ExceptionMessage,
                    logEntry.Timestamp,
                    logEntry.ExceptionType,
                    SourceName = "Compentio.Ankara.Gui",
                    userLogin
                });
            }

            scope.Complete();
        }
    }
}
