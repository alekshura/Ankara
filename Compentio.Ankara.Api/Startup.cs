namespace Compentio.Ankara
{
    using System;
    using System.Security.Claims;
    using Compentio.Ankara.Api.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Server.IISIntegration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json.Converters;
    using Compentio.Ankara.Api.Providers;
    using Compentio.Ankara.Api.Filters;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = IISDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = IISDefaults.AuthenticationScheme;
            });

            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.AddPolicy("DefaultPolicy", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                });

                //authorizationOptions.AddPolicy("Administrator", policyBuilder =>
                //{
                //    policyBuilder.RequireAuthenticatedUser();
                //    policyBuilder.RequireClaim(ClaimTypes.Role, Roles.BusinessAdmin.ToString());
                //});
            });

            services.AddCors();
            
            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new ArrayBinderProvider());
                options.Filters.Add(new NLogContextActionFilter());
            }).AddNewtonsoftJson(opts => opts.SerializerSettings.Converters.Add(new StringEnumConverter()));

            services.AddRepositories(Configuration);
            
            services.AddServices();
            
            services.AddSwagger();    
        }       

         public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
                
            app.UseHttpsRedirection();

            app.UseRouting();

            // This CORS Policy for Angular SPA Hosted
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ankara API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseExceptionMiddleware();
        }
    }
}
