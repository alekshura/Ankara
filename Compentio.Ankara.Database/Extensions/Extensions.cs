namespace Compentio.Ankara.Database.Extensions
{
    using FluentMigrator.Builders;
    using FluentMigrator.Builders.Create.Table;
    using System;

    public static class Extentions
    {
        /// <summary>
        /// Uee this extensions method to define audit colums in tables. This add columns with the same names for audit info in different tables. 
        /// </summary>
        /// <param name="tableWithColumnSyntax"></param>
        /// <returns></returns>
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
        /// <summary>
        /// This column we used for json data. Relational DataBase is denormalised.
        /// </summary>
        /// <param name="columnTypeSyntax"></param>
        /// <returns></returns>
        public static ICreateTableColumnOptionOrWithColumnSyntax AsMaxString(this IColumnTypeSyntax<ICreateTableColumnOptionOrWithColumnSyntax> columnTypeSyntax)
        {
            return columnTypeSyntax.AsCustom("nvarchar(max)");
        }
    }
}
