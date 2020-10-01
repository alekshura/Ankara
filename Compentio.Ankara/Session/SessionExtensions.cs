namespace Compentio.Ankara.Session
{
    using Compentio.Ankara.Models.Users;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    public static class ISessionExtensions
    {
        private const string IsSessionActiveKey = "IsSessionActiveKey";
        private const string UserKey = "UserKey";

        public static bool IsSessionActive(this ISession session)
        {
            if (!session.IsAvailable)
            {
                session.LoadAsync().ConfigureAwait(false);
            }

            var value = session.GetString(IsSessionActiveKey);

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return true;
        }
        private static async Task Set<T>(this ISession session, string key, T value)
        {
            if (!session.IsAvailable)
                await session.LoadAsync().ConfigureAwait(false);

            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        private static async Task<T> Get<T>(this ISession session, string key)
        {
            if (!session.IsAvailable)
            {
                await session.LoadAsync().ConfigureAwait(false);
            }

            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
        public static User GetCurrentUser(this ISession session)
        {
            return Get<User>(session, UserKey).Result;
        }
        public static async Task SetCurrentUser(this ISession session, User user)
        {           
            await Set(session, UserKey, user).ConfigureAwait(false);
            await session.Set(IsSessionActiveKey, true);
        }
        public static string GetSessionKey(this ISession session)
        {
            var distributedSession = session as DistributedSession;
            return distributedSession.SessionKey;
        }
        public static void ClearSession(this ISession session)
        {
            var distributedSession = session as DistributedSession;
            distributedSession.ClearSession();
        }
        
    }
}
