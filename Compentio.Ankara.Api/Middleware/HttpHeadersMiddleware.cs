namespace Compentio.Ankara.Api.Middleware
{
    using Microsoft.AspNetCore.Builder;

    public static class HttpHeadersMiddleware
    {
        public static void UseHttpHeaders(this IApplicationBuilder builder)
        {
            builder.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "deny");
                context.Response.Headers.Add("Content-Security-Policy", 
                    $"default-src 'self'; script-src 'self' { context.Request.Host.Value }; style-src 'self' { context.Request.Host.Value }; frame-ancestors 'none'");
                context.Response.Headers.Add("Content-Security-Policy-Report-Only",
                    $"default-src 'self'; script-src 'self' { context.Request.Host.Value }; style-src 'self' { context.Request.Host.Value }; frame-ancestors 'none'; report-uri /report-csp");
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=63072000; includeSubDomains; preload");
                context.Response.Headers.Add("Referrer-Policy", "same-origin");
                context.Response.Headers.Add("Feature-Policy", $"sync-xhr 'self' { context.Request.Host.Value }");

                context.Response.Headers.Add("Cache-control", "no-store");
                context.Response.Headers.Add("Pragma", "no-cache");

                await next().ConfigureAwait(false);
            });
        }
    }
}
