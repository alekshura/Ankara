namespace Compentio.Ankara.Api.Extensions
{
    using Microsoft.AspNetCore.Server.IISIntegration;
    using Microsoft.Extensions.DependencyInjection;

    public static class AuthenticationServiceCollectionExtensions
    {
        public static void AddWindowsAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = IISDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = IISDefaults.AuthenticationScheme;
            });

            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.AddPolicy("DefaultPolicy", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                });
            });
        }
    }
}
