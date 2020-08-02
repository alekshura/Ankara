namespace Compentio.Ankara.Api.Extensions
{
    using Compentio.Ankara.Api.Middleware;
    using Microsoft.AspNetCore.Builder;

    public static class MiddlewareExtensions
    {
        // Extension method used to add the middleware to the HTTP request pipeline.
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
