namespace Compentio.Ankara.Repositories.ConnectionFactory
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Data.Common;

    public class DbProviderConnectionFactory : IAdoConnectionFactory
    {
        private readonly DbProviderFactory _adoFactory;
        private readonly string _connectionString;
        
        public DbProviderConnectionFactory(DbProviderFactory adoFactory, IConfiguration configuration)
        {
            _adoFactory = adoFactory ?? throw new ArgumentNullException(nameof(adoFactory));
            if (string.IsNullOrEmpty(configuration.GetConnectionString("Ankara")))
            {
                throw new ArgumentException("ConnectionString Cannot be null or empty!");
            }
            _connectionString = configuration.GetConnectionString("Ankara");
        }
        public DbConnection CreateConnection()
        {
            var connection = _adoFactory.CreateConnection();
            connection.ConnectionString = _connectionString;
            return connection;
        }
    }
}
