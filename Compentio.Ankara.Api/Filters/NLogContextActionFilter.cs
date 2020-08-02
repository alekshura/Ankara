namespace Compentio.Ankara.Api.Filters
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Collections.Generic;

    public class NLogContextActionFilter : IActionFilter
    {
        private const string LogContextKey = "ankara:log_context";

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var logContext = GetLogContext(context);
            context.HttpContext.Items[LogContextKey] = logContext;
            SetLogContext(logContext);
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Items.TryGetValue(LogContextKey, out var itemValue))
            {
                var logContext = (IDictionary<string, object>)itemValue;
                ResetLogContext(logContext);
            }
        }

        private static IDictionary<string, object> GetLogContext(ActionExecutingContext actionContext)
        {
            return new Dictionary<string, object>
            {
                { "UserLogin", actionContext.HttpContext.User.Identity.Name }
            };
        }

        private static void SetLogContext(IDictionary<string, object> logContext)
        {
            foreach (var item in logContext)
            {
                NLog.MappedDiagnosticsLogicalContext.Set(item.Key, item.Value);
            }
        }

        private static void ResetLogContext(IDictionary<string, object> logContext)
        {
            foreach (var item in logContext)
            {
                NLog.MappedDiagnosticsLogicalContext.Remove(item.Key);
            }
        }
    }
}
