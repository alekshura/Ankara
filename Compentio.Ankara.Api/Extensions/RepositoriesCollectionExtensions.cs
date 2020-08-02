namespace Compentio.Ankara.Api.Extensions
{
    using Compentio.Ankara.Repositories;
    using Compentio.Ankara.Repositories.ConnectionFactory;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Data.Common;
    using System.Data.SqlClient;

    public static class RepositoriesCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAdoConnectionFactory, DbProviderConnectionFactory>(factory => {
                return CreateConnectionFactory(configuration);
            });

            // Register here your repositories
            services.AddSingleton<IUsersRepository, UsersRepository>();
        }

        private static DbProviderConnectionFactory CreateConnectionFactory(IConfiguration configuration)
        {
            var providerInvarianName = "System.Data.SqlClient";

            if (!DbProviderFactories.TryGetFactory(providerInvarianName, out var factory))
            {
                DbProviderFactories.RegisterFactory(providerInvarianName, SqlClientFactory.Instance);
                factory = DbProviderFactories.GetFactory(providerInvarianName);
            }
            
            return new DbProviderConnectionFactory(factory, configuration);
        }
    }
}
