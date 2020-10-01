namespace Compentio.Ankara.Api.Extensions
{
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.IO;
    using Compentio.Ankara.Session;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class SessionServiceCollectionExtensions
    {
        public static void AddAppSession(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAppSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(configuration.GetValue<int>("SessionTimeoutInMinutes"));
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                options.Cookie.Name = SessionDefaults.CookieName;
            });

            var share = configuration.GetValue<string>("KeysSharePath");

            if (!string.IsNullOrWhiteSpace(share))
            {
                services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(share));

            }
        }

        private static IServiceCollection AddAppSession(this IServiceCollection services, Action<SessionOptions> configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            services.Configure(configure);
            services.TryAddTransient<ISessionStore, DistributedSessionStore>();
            return services;
        }
    }
}
