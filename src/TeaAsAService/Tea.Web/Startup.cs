using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tea.Web.Configuration;
using Microsoft.Extensions.Hosting;
using Tea.Core.Impl.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Tea.Core.Data;
using Tea.Core.Impl;
using Microsoft.AspNetCore.Authentication;
using AspNetCoreRateLimit;
using NSwag.AspNetCore;
using Tea.Core;
using Tea.Core.Impl.Services;
using Tea.Web.Middleware;

namespace Tea.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            
            services.AddDbContext<TeaContext>(options =>
                    options
                        .UseLazyLoadingProxies()
                        .UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddAuthentication(BasicAuthenticationHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthenticationHandler.SchemeName, null);

            services
                .AddOptions()
                .AddScoped<IDataStore, DataStore>()                
                .AddSingleton<IRoundService, RoundService>()
                .AddSingleton<IPasswordHasher,PasswordHasher>()
                .AddApiVersioningConfig()
                .AddRateLimiting(Configuration)
                .AddSwagger("v1")
                .AddApplicationHealthChecks(Configuration)
                .AddHttpContextAccessor();           

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseIpRateLimiting();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseOpenApi(cfg =>
            {
                cfg.DocumentName = "v1";
                cfg.Path = "/swagger/v1/swagger.json";
            });

            app.UseSwaggerUi3(config =>
            {
                config.SwaggerRoutes.Clear();

                config.SwaggerRoutes.Add(new SwaggerUi3Route("v1", $"/swagger/v1/swagger.json"));

                config.Path = "";
                config.DocumentTitle = "TeaAsAService API Documentation";
                config.DocExpansion = "list";
                config.EnableTryItOut = true;
                config.WithCredentials = true;
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc");
            });
        }
    }
}
