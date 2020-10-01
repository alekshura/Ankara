namespace Compentio.Ankara.Api.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class HealthChecksCollectionExtensions
    {
        public static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddSqlServer(
                          connectionString: configuration.GetConnectionString("Ankara"),
                          healthQuery: "SELECT 1;",
                          name: "AnkaraDatabase-check",
                          failureStatus: HealthStatus.Degraded,
                          tags: new string[] { "ankaradb", "sqlserver" });

        }
    }
}
