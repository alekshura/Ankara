namespace Compentio.Ankara.Models.Users
{
    using System.Collections.Generic;

    public class AuthorizationResult
    {
        public AuthorizationResult() { }

        public AuthorizationResult(bool isAuthorized)
        {
            IsAuthorized = isAuthorized;
        }
        public AuthorizationResult(IEnumerable<User> user, bool isAuthorized)
        {
            User = user;
            IsAuthorized = isAuthorized;
        }
        public bool IsAuthorized { get; set; } = false;
        public IEnumerable<User> User { get; set; }
    }
}
