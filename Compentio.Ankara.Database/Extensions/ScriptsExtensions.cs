namespace Compentio.Ankara.Database.Extensions
{
    using FluentMigrator.Builders.Execute;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;

    public static class ScriptsExtensions
    {
        public static void AppScript(this IExecuteExpressionRoot executeExpressionRoot, string path)
        {
            var parameters = new Dictionary<string, string>
            {
                {"ScriptParameter", Program.Configuration.GetValue<string>("ScriptParameter") }
            };

            executeExpressionRoot.Script(path, parameters);
        }      
    }
}
