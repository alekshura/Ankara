namespace Compentio.Ankara.Session
{
    using System;
    using Microsoft.AspNetCore.Http;

    public interface ISessionStore
    {
        ISession Create(string sessionKey, TimeSpan idleTimeout, TimeSpan ioTimeout, Func<bool> tryEstablishSession, bool isNewSessionKey);
    }
}