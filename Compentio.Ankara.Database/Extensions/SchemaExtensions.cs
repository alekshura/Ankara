namespace Compentio.Ankara.Database.Extensions
{
    using FluentMigrator;
    using FluentMigrator.Builders.Create;
    using FluentMigrator.Builders.Create.Schema;
    using FluentMigrator.Builders.Delete;
    using FluentMigrator.Builders.Schema;
    using FluentMigrator.Builders.Schema.Schema;
    using System;

    public static class SchemaExtensions
    {
        public static string Name => "app";

        public static bool AppSchemaExists(this ISchemaExpressionRoot schemaExpressionRoot)
        {
            return schemaExpressionRoot.Schema(Name).Exists() && !Program.PreviewOnly;
        }
        public static ISchemaSchemaSyntax AppSchema(this ISchemaExpressionRoot schemaExpressionRoot)
        {
            return schemaExpressionRoot.Schema(Name);
        }
        public static ICreateSchemaOptionsSyntax AppSchema(this ICreateExpressionRoot createExpressionRoot)
        {
            return createExpressionRoot.Schema(Name);
        }
        public static void AppSchema(this IDeleteExpressionRoot deleteExpressionRoot)
        {
            deleteExpressionRoot.Schema(Name);
        }
    }
}
