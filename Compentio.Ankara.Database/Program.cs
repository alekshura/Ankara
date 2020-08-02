namespace Compentio.Ankara.Database
{
    using FluentMigrator.Runner;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;
    using NLog;
    using NLog.Extensions.Logging;
    using Compentio.Ankara.Database.Migrations;

    class Program
    {

        static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            try
            {
                logger.Info($"Start dabase migration. ConnectionString: {configuration.GetConnectionString("Ankara")}");

                var serviceProvider = CreateServices(configuration);
                using var scope = serviceProvider.CreateScope();
                UpdateDatabase(scope.ServiceProvider);

                logger.Info($"Migration sucessfull.");
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Migration stopped because of exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        private static IServiceProvider CreateServices(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Ankara");

            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(InitialMigration).Assembly).For.Migrations())
                        .AddLogging(loggingBuilder => {
                            loggingBuilder.ClearProviders();
                            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                            loggingBuilder.AddNLog("NLog.config");
                        })
                        .BuildServiceProvider(false);
        }


        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}
