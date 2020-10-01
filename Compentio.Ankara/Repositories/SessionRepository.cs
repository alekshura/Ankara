namespace Compentio.Ankara.Repositories
{
    using Compentio.Ankara.Models.Session;
    using Compentio.Ankara.Repositories.ConnectionFactory;
    using Dapper;
    using NLog;
    using System.Threading.Tasks;

    public interface ISessionRepository
    {
        Task UpdateUserSession();
        Task<bool> HasActiveSession(string userLogin);
        Task ClearUserSession(string userLogin);
        Task<UserSession> GetActiveSession(string sessionKey);
    }

    public class SessionRepository : ISessionRepository
    {
        private readonly ICurrentUserContext _userContext;
        private readonly IAdoConnectionFactory _connectionFactory;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public SessionRepository(IAdoConnectionFactory connectionFactory, ICurrentUserContext userContext)
        {
            _connectionFactory = connectionFactory;
            _userContext = userContext;
        }       
        public async Task UpdateUserSession()
        {
            using var connection = _connectionFactory.CreateConnection();

            string query = @"MERGE[app].[UsersSession] AS target
                             USING(SELECT @SessionId, @SessionKey, @Login, @UserId) AS source(SessionId, SessionKey, Login, UserId)
                             ON target.UserId = source.UserId
                             WHEN MATCHED THEN UPDATE SET SessionId = @SessionId, SessionKey = @SessionKey, Login = @Login, UserId = @UserId, TimeStamp = SYSDATETIMEOFFSET()
                             WHEN NOT MATCHED THEN
                             INSERT(SessionKey, SessionId, Login, UserId, TimeStamp)
                             VALUES(@SessionKey, @SessionId, @Login, @UserId, SYSDATETIMEOFFSET());";

            await connection.ExecuteAsync(query, new
            {
                _userContext.SessionId,
                _userContext.SessionKey,
                _userContext.CurrentUser.Login,
                UserId = _userContext.CurrentUser.Id
            }).ConfigureAwait(false);

            _logger.Info($"Update user session. SessionId: '{_userContext.SessionId}'. SessionKey: '{_userContext.SessionKey}'");
        }
        public async Task<bool> HasActiveSession(string userLogin)
        {
            using var connection = _connectionFactory.CreateConnection();
            string sessionsCache = @"SELECT COUNT(Id) FROM app.Cache WHERE Id IN (SELECT SessionKey FROM app.UsersSession WHERE Login = @Login) AND ExpiresAtTime > SYSDATETIMEOFFSET()";

            var activeSessions = await connection.QuerySingleAsync<int>(sessionsCache, new
            {
                Login = userLogin
            }).ConfigureAwait(false);

            if (activeSessions > 0)
            {                
                return true;
            }

            return false;
        }
        public async Task ClearUserSession(string userLogin)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = @"DELETE FROM app.UsersSession WHERE Login = @Login";

            var activeSessions = await connection.ExecuteAsync(query, new
            {
               Login = userLogin
            }).ConfigureAwait(false);
        }
        public async Task<UserSession> GetActiveSession(string sessionKey)
        {
            using var connection = _connectionFactory.CreateConnection();
            string sessionsCache = @"SELECT SessionKey, SessionId FROM app.UsersSession WHERE SessionKey = @SessionKey";

            return await connection.QuerySingleOrDefaultAsync<UserSession>(sessionsCache, new
            {
                SessionKey = sessionKey
            }).ConfigureAwait(false);
        }
    }
}
