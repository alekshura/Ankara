namespace Compentio.Ankara
{
    using Compentio.Ankara.Models.Users;
    using Compentio.Ankara.Session;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface ICurrentUserContext
    {
        User CurrentUser { get; }
        string SessionId { get; }
        string SessionKey { get; }
        Task SetCurrentUser(User user);
        Task Clear();
    }

    public class CurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string SessionId => _httpContextAccessor.HttpContext.Session.Id;

        public string SessionKey => _httpContextAccessor.HttpContext.Session.GetSessionKey();

        public User CurrentUser => _httpContextAccessor.HttpContext.Session.GetCurrentUser();

        public async Task SetCurrentUser(User user)
        {
            await _httpContextAccessor.HttpContext.Session.SetCurrentUser(user).ConfigureAwait(false);
        }
        public async Task Clear()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(SessionDefaults.CookieName, new CookieOptions()
            {
                Secure = true,
            });
            _httpContextAccessor.HttpContext.Session.Clear();
            await Task.CompletedTask;
        }
    }
}
