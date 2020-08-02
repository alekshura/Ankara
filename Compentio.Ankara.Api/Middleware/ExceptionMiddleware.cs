namespace Compentio.Ankara.Api.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using NLog;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly ConcurrentDictionary<Type, Func<Exception, HttpContext, Task>> Handlers = new ConcurrentDictionary<Type, Func<Exception, HttpContext, Task>>();

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    Logger.Warn("The response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                await Handle(ex, context).ConfigureAwait(false);
            }
        }

        // TODO Write aour custom exception and declare here
        //public static async Task Handle(Exception exception, HttpContext context)
        //{
        //    var result = new NonSuccessResult(NonSuccessResultCode.AddonWrongVersion, new string[0]);
        //    await Write(context, StatusCodes.Status400BadRequest, result).ConfigureAwait(false);
        //}

        public static async Task Handle(Exception exception, HttpContext context)
        {
            Logger.Error(exception, exception.Message);

            var handler = Handlers.GetOrAdd(exception.GetType(), k =>
            {
                var method = typeof(ExceptionMiddleware).GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(x =>
                    {
                        var parameters = x.GetParameters();
                        return parameters.Length == 2 && parameters[0].ParameterType == k &&
                               parameters[1].ParameterType == typeof(HttpContext);
                    });

                if (method == null)
                    return (e, c) => (Task)null;

                var exParam = Expression.Parameter(typeof(Exception), "ex");
                var contextParam = Expression.Parameter(typeof(HttpContext), "context");

                var lambda = Expression.Lambda<Func<Exception, HttpContext, Task>>(
                    Expression.Call(method, Expression.Convert(exParam, k), contextParam),
                    exParam, contextParam);

                return lambda.Compile();
            });

            var task = handler(exception, context);

            if (task == null)
                await Write(context, StatusCodes.Status500InternalServerError).ConfigureAwait(false);
            else
                await task.ConfigureAwait(false);
        }

        private static async Task Write(HttpContext context, int statusCode, object result = null)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            var content = result != null ? JsonConvert.SerializeObject(result) : String.Empty;
            await context.Response.WriteAsync(content).ConfigureAwait(false);
        }
    }
}
