namespace Compentio.Ankara.Services
{
    using Compentio.Ankara.Models.Users;
    using Compentio.Ankara.Repositories;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAuthorizationService
    {
        Task<AuthorizationResult> Logon(string userLogin);
        Task Logout(string userLogin);

    }
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ISessionService _sessionService;

        public AuthorizationService(IUsersRepository usersRepository, ISessionService sessionService)
        {
            _usersRepository = usersRepository;
            _sessionService = sessionService;
        }

        public async Task<AuthorizationResult> Logon(string userLogin)
        {
            var result = await _usersRepository.Logon(userLogin).ConfigureAwait(false);

            if (result != null)
            {
                await _sessionService.StartSession(result.FirstOrDefault()).ConfigureAwait(false);
                return new AuthorizationResult(result, true);
            }

            return new AuthorizationResult(false);
        }

        public async Task Logout(string userLogin)
        {
            await _sessionService.EndSession(userLogin).ConfigureAwait(false);
        }
    }
}
