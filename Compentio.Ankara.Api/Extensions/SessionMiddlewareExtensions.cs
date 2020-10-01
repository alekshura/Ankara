namespace Compentio.Ankara.Api.Extensions
{
    using System;
    using Compentio.Ankara.Session;
    using Microsoft.AspNetCore.Builder;

    public static class SessionMiddlewareExtensions
    {
        public static IApplicationBuilder UseAppSession(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<SessionMiddleware>();
        }
    }
}