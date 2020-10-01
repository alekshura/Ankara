namespace Compentio.Ankara
{
    using System;
    using Compentio.Ankara.Api.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json.Converters;
    using Compentio.Ankara.Api.Providers;
    using Compentio.Ankara.Api.Filters;
    using Compentio.Ankara.Api.Extensions;
    using Compentio.Ankara.Api.Middleware;
    using AspNetCoreRateLimit;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWindowsAuthentication();

            services.AddCors();

            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new ArrayBinderProvider());
                options.Filters.Add(new NLogContextActionFilter());
            }).AddNewtonsoftJson(opts => opts.SerializerSettings.Converters.Add(new StringEnumConverter()));

            services.AddRepositories(Configuration);

            services.AddServices();

            services.AddThrottling(Configuration);

            services.AddHealthChecks(Configuration);

            services.AddSwagger();

            services.AddAppSession(Configuration);

            services.AddAntiforgery();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(policy =>
                policy.WithOrigins(configuration.GetSection("CorsOrigins").Value)
                    .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                    .AllowAnyHeader()
                    .AllowCredentials()
            );

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Retager API V1");
            });

            if (!env.IsDevelopment())
            {
                app.UseHttpHeaders();
            }

            app.UseAppSession();

            app.UseExceptionMiddleware();

            app.UseClientRateLimiting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
