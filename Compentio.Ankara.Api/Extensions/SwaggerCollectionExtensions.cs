namespace Compentio.Ankara.Api.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using System.IO;

    public static class SwaggerCollectionExtensions
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "AnkaraApi.xml");
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ankara API", Version = "v1" });
                c.IncludeXmlComments(filePath);
            });
        }
    }
}
