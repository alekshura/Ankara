namespace Compentio.Ankara.Services
{
    using Compentio.Ankara.Models.Users;
    using Compentio.Ankara.Repositories;
    using System.Threading.Tasks;

    public interface ISessionService
    {
        Task StartSession(User user);
        Task EndSession(string userLogin);
        Task SetUserContext(User user);
        Task<bool> HasActiveSession(string userLogin);
        Task<bool> ValidateSession(string sessionKey);
    }

    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly ICurrentUserContext _userContext;

        public SessionService(ISessionRepository sessionRepository, ICurrentUserContext userContext)
        {
            _sessionRepository = sessionRepository;
            _userContext = userContext;
        }

        public async Task<bool> HasActiveSession(string userLogin)
        {
            return await _sessionRepository.HasActiveSession(userLogin).ConfigureAwait(false);
        }
        public async Task<bool> ValidateSession(string sessionKey)
        {
            var activeSession = await _sessionRepository.GetActiveSession(sessionKey).ConfigureAwait(false);

            if (activeSession == null)
            {               
                return false;
            }

            if (activeSession.SessionKey != sessionKey || activeSession.SessionId != _userContext.SessionId)
            {                
                return false;
            }
            return true;
        }
        public async Task StartSession(User user)
        {
            await _userContext.SetCurrentUser(user).ConfigureAwait(false);
            await _sessionRepository.UpdateUserSession().ConfigureAwait(false);
        }
        public async Task SetUserContext(User user)
        {
            await _userContext.SetCurrentUser(user).ConfigureAwait(false);
        }
        public async Task EndSession(string userLogin)
        {
            await _sessionRepository.ClearUserSession(userLogin).ConfigureAwait(false);
            await _userContext.Clear().ConfigureAwait(false);
        }
    }
}
