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
    using Microsoft.Extensions.CommandLineUtils;
    using FluentMigrator.Runner.Logging;

    class Program
    {
        public static IConfigurationRoot Configuration { private set; get; }
        public static bool PreviewOnly { private set; get; } = false;
        private static bool PreviewUnapplied { get; set; } = false;
        private static string PreviewScriptName { get; set; } = null;

        static void Main(string[] args)
        {
            CommandLineApplication commandLineApplication = new CommandLineApplication(throwOnUnexpectedArg: false);
            var previewOption = commandLineApplication.Option(
              "-$|-s |--script <fineName>", "Generate Sql database scripts but do not execute it on the database.", CommandOptionType.SingleValue);
            var unappliedOption = commandLineApplication.Option(
              "-$|-u |--unnaplied", "Generate Sql scripts for unapplied changes in database without executing. ", CommandOptionType.NoValue);

            commandLineApplication.HelpOption("-? | -h | --help");
            commandLineApplication.OnExecute(() =>
            {
                if (previewOption.HasValue())
                {
                    PreviewOnly = true;
                    PreviewScriptName = previewOption.Value();
                }

                if (unappliedOption.HasValue())
                {
                    PreviewOnly = true;
                    PreviewUnapplied = true;
                    PreviewScriptName = previewOption.HasValue() ? previewOption.Value() : $"UnappliedMigrations_{ DateTime.UtcNow:yyyyMMddHHmm}.sql";
                }

                Run();
                return 0;
            });

            commandLineApplication.Execute(args);
        }

        private static void Run()
        {
            var logger = LogManager.GetCurrentClassLogger();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Configuration = configuration;

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
                    .ConfigureGlobalProcessorOptions(o => o.PreviewOnly = PreviewOnly)
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(InitialMigration).Assembly).For.Migrations())
                        .AddLogging(loggingBuilder => {
                            loggingBuilder.ClearProviders();
                            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                            loggingBuilder.AddNLog("NLog.config");
                            loggingBuilder.AddFluentMigratorConsole();
                            if (PreviewOnly)
                            {
                                loggingBuilder.Services.AddSingleton<ILoggerProvider, LogFileFluentMigratorLoggerProvider>();
                                loggingBuilder.Services.Configure<LogFileFluentMigratorLoggerOptions>(
                                    opt =>
                                    {
                                        opt.OutputFileName = PreviewScriptName;
                                        opt.OutputGoBetweenStatements = true;
                                        opt.ShowSql = true;
                                    });
                            };
                        })

                .BuildServiceProvider(false);
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            if (PreviewOnly && !PreviewUnapplied)
            {
                foreach (var item in runner.MigrationLoader.LoadMigrations())
                {
                    runner.Up(item.Value.Migration);
                }

                return;
            }

            runner.MigrateUp();
        }
    }
}
