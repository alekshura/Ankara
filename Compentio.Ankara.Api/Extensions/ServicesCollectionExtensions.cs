namespace Compentio.Ankara.Api.Extensions
{
    using Compentio.Ankara.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServicesCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            //Register here your services
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<ICurrentUserContext, CurrentUserContext>();            
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IMaintenanceService, MaintenanceService>();
            services.AddTransient<ISessionService, SessionService>();
        }
    }
}
