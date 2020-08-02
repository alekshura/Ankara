namespace Compentio.Ankara.Database.Migrations
{
    using Compentio.Ankara.Database.Extensions;
    using FluentMigrator;
    using NLog;

    [Migration(2020_01_01_1000)]
    public class InitialMigration : Migration
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override void Up()
        {
            logger.Info("Start initial migration");

            if (!Schema.Schema(Database.Schema.Name).Exists())
            {
                Create.Schema(Database.Schema.Name);
            }

            if (!Schema.Schema(Database.Schema.Name).Table(Database.Schema.Tables.Dictionaries).Exists())
            {
                Create.Table(Database.Schema.Tables.Dictionaries).InSchema(Database.Schema.Name)
                    .WithIdentityColumn()
                    .WithColumn("CategoryId").AsInt64()
                    .WithColumn("Value").AsString()
                    .WithColumn("DepartmentsJson").AsMaxString()
                    .WithActiveColumn()
                    .WithAuditColumns();
            }

            if (!Schema.Schema(Database.Schema.Name).Table(Database.Schema.Tables.Logs).Exists())
            {
                Create.Table(Database.Schema.Tables.Logs).InSchema(Database.Schema.Name)
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

            if (!Schema.Schema(Database.Schema.Name).Table(Database.Schema.Tables.Params).Exists())
            {
                Create.Table(Database.Schema.Tables.Params).InSchema(Database.Schema.Name)
                .WithIdentityColumn()
                .WithColumn("Name").AsString()
                .WithColumn("Value").AsString()
                .WithColumn("Description").AsString()
                .WithAuditColumns();
            }

            if (!Schema.Schema(Database.Schema.Name).Table(Database.Schema.Tables.Users).Exists())
            {
                Create.Table(Database.Schema.Tables.Users).InSchema(Database.Schema.Name)
               .WithIdentityColumn()
               .WithColumn("UserId").AsInt64().Indexed()
               .WithColumn("Roles").AsMaxString().WithDefaultValue("")
               .WithPeriodColumns()
               .WithActiveColumn()
               .WithAuditColumns();
            }           

            logger.Info("Initial migration completed");
        }

        public override void Down()
        {
            logger.Warn("Initial migration rollback.");

            Delete.Schema(Database.Schema.Name);
            Delete.Table(Database.Schema.Tables.Dictionaries);
            Delete.Table(Database.Schema.Tables.Logs);
            Delete.Table(Database.Schema.Tables.Params);
            Delete.Table(Database.Schema.Tables.Users);
        }
    }  
}
