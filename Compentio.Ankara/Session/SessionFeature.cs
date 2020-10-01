namespace Compentio.Ankara.Session
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;

    public class SessionFeature : ISessionFeature
    {
        public ISession Session { get; set; }
    }
}