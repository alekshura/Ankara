namespace Compentio.Ankara.Database.Extensions
{
    using FluentMigrator.Builders;
    using FluentMigrator.Builders.Alter;
    using FluentMigrator.Builders.Alter.Table;
    using FluentMigrator.Builders.Create;
    using FluentMigrator.Builders.Create.Table;
    using FluentMigrator.Builders.Insert;
    using FluentMigrator.Builders.Schema.Schema;
    using FluentMigrator.Builders.Update;
    using System;

    public static class TableExtensions
    {
        public static class TableNames
        {
            public static string Cache => "Cache";
            public static string Users => "Users";
            public static string UsersSession => "UsersSession";           
            public static string Logs => "Logs";           
        }

        public static ICreateTableWithColumnSyntax AppTable(this ICreateExpressionRoot createExpressionRoot, string tableName)
        {
            return createExpressionRoot.Table(tableName).InSchema(SchemaExtensions.Name);
        }
        public static IUpdateSetSyntax AppTable(this IUpdateExpressionRoot updateExpressionRoot, string tableName)
        {
            return updateExpressionRoot.Table(tableName).InSchema(SchemaExtensions.Name);
        }
        public static IInsertDataSyntax IntoAppTable(this IInsertExpressionRoot insertExpressionRoot, string tableName)
        {
            return insertExpressionRoot.IntoTable(tableName).InSchema(SchemaExtensions.Name);
        }

        public static IAlterTableAddColumnOrAlterColumnSyntax AppTable(this IAlterExpressionRoot alterExpressionRoot, string tableName)
        {
            return alterExpressionRoot.Table(tableName).InSchema(SchemaExtensions.Name);
        }

        public static bool AppTableExists(this ISchemaSchemaSyntax schemaTableSyntax, string tableName)
        {
            return schemaTableSyntax.Table(tableName).Exists() && !Program.PreviewOnly;
        }

        public static ICreateTableWithColumnSyntax WithAuditColumns(this ICreateTableWithColumnSyntax tableWithColumnSyntax)
        {
            return tableWithColumnSyntax.WithColumn("CreatedBy").AsString().WithDefaultValue("Migrator")
                                .WithColumn("CreationDate").AsDateTime().WithDefaultValue(DateTime.UtcNow)
                                .WithColumn("ModifiedBy").AsString().WithDefaultValue("Migrator")
                                .WithColumn("ModificationDate").AsDateTime().WithDefaultValue(DateTime.UtcNow);
        }

        public static ICreateTableWithColumnSyntax WithPeriodColumns(this ICreateTableWithColumnSyntax tableWithColumnSyntax)
        {
            return tableWithColumnSyntax.WithColumn("ValidFrom").AsDateTime().WithDefaultValue(DateTime.UtcNow.AddDays(1))
                                .WithColumn("ValidTo").AsDateTime().WithDefaultValue(DateTime.UtcNow.AddYears(10));
        }

        public static ICreateTableWithColumnSyntax WithActiveColumn(this ICreateTableWithColumnSyntax tableWithColumnSyntax)
        {
            return tableWithColumnSyntax.WithColumn("IsActive").AsBoolean().WithDefaultValue(true);
        }

        public static ICreateTableWithColumnSyntax WithIdentityColumn(this ICreateTableWithColumnSyntax tableWithColumnSyntax)
        {
            return tableWithColumnSyntax.WithColumn("Id").AsInt64().PrimaryKey().Identity();
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsMaxString(this IColumnTypeSyntax<ICreateTableColumnOptionOrWithColumnSyntax> columnTypeSyntax)
        {
            return columnTypeSyntax.AsCustom("nvarchar(max)");
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsMaxVarBinary(this IColumnTypeSyntax<ICreateTableColumnOptionOrWithColumnSyntax> columnTypeSyntax)
        {
            return columnTypeSyntax.AsCustom("varbinary(max)");
        }

        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AsMaxString(this IColumnTypeSyntax<IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax> columnTypeSyntax)
        {
            return columnTypeSyntax.AsCustom("nvarchar(max)");
        }

    }
}
