namespace Compentio.Ankara.Services
{
    using Compentio.Ankara.Models.Users;
    using Compentio.Ankara.Repositories;
    using System.Threading.Tasks;

    public interface IAuthorizationService
    {
        Task<User> Logon(string userName);
        Task Logout();

    }
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ICurrentUserContext _userContext;

        public AuthorizationService(IUsersRepository usersRepository, ICurrentUserContext userContext)
        {
            _usersRepository = usersRepository;
            _userContext = userContext;
        }
        public async Task<User> Logon(string userLogin)
        {
            var result = await _usersRepository.GetUser(userLogin).ConfigureAwait(false);

            return result.Match(
                Some: user =>
                {
                    _userContext.SetCurrentUser(user);
                    return user;
                },
                None: () => null);
        }
        public async Task Logout()
        {
            _userContext.Clear();
            await Task.CompletedTask;
        }
    }
}
