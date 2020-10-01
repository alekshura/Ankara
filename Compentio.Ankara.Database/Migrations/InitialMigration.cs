namespace Compentio.Ankara.Database.Migrations
{
    using Compentio.Ankara.Database.Extensions;
    using FluentMigrator;
    using NLog;
    using System;
    using static Compentio.Ankara.Database.Extensions.TableExtensions;

    [Migration(2020_01_01_1000)]
    public class InitialMigration : Migration
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override void Up()
        {
            logger.Info("Start initial migration");

            if (!Schema.AppSchemaExists())
            {
                Create.AppSchema();
            }

            if (!Schema.AppSchema().AppTableExists(TableNames.Cache))
            {
                Create.AppTable(TableNames.Cache)
                    .WithColumn("Id").AsString(449).NotNullable().PrimaryKey()
                    .WithColumn("Value").AsMaxVarBinary().NotNullable()
                    .WithColumn("ExpiresAtTime").AsDateTimeOffset(7).NotNullable()
                    .WithColumn("SlidingExpirationInSeconds").AsInt64().Nullable()
                    .WithColumn("AbsoluteExpiration").AsDateTimeOffset(7).Nullable();
            }

            if (!Schema.AppSchema().AppTableExists(TableNames.Logs))
            {
                Create.AppTable(TableNames.Logs)
                    .WithIdentityColumn()
                    .WithColumn("Severity").AsString()
                    .WithColumn("Message").AsMaxString()
                    .WithColumn("Timestamp").AsDateTime().Indexed()
                    .WithColumn("Logger").AsString()
                    .WithColumn("ExceptionType").AsString().Nullable()
                    .WithColumn("ExceptionMessage").AsMaxString().Nullable()
                    .WithColumn("UserLogin").AsString().Nullable()
                    .WithColumn("SourceName").AsString();
            }

            if (!Schema.AppSchema().AppTableExists(TableNames.Users))
            {
                Create.AppTable(TableNames.Users)
                   .WithIdentityColumn()
                   .WithColumn("UserId").AsInt64().Indexed()
                   .WithColumn("Roles").AsMaxString().WithDefaultValue("")
                   .WithPeriodColumns()
                   .WithActiveColumn()
                   .WithAuditColumns();
            }

            if (!Schema.AppSchema().AppTableExists(TableNames.UsersSession))
            {
                Create.AppTable(TableNames.UsersSession)
                  .WithColumn("SessionKey").AsString(449).NotNullable().PrimaryKey()
                  .WithColumn("SessionId").AsString().NotNullable()
                  .WithColumn("Login").AsString().NotNullable()
                  .WithColumn("UserId").AsInt64().NotNullable().Indexed()
                  .WithColumn("TimeStamp").AsDateTimeOffset(7).NotNullable().WithDefaultValue(DateTimeOffset.UtcNow);
            }


            Execute.AppScript("./Scripts/Example.sql");

            logger.Info("Initial migration completed");
        }

        public override void Down()
        {
            logger.Warn("Initial migration rollback.");

            Delete.AppSchema();

            Delete.Table(TableNames.Logs);
            Delete.Table(TableNames.Users);
        }
    }
}
