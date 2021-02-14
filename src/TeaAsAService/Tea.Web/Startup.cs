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
using Tea.Web.Helpers;
using AspNetCoreRateLimit;
using Tea.Core;
using Tea.Core.Impl.Services;

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
            
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services
                .AddOptions()
                .AddScoped<IDataStore, DataStore>()                
                .AddSingleton<IRoundService, RoundService>()
                .AddApiVersioningConfig()
                .AddRateLimiting(Configuration)
                .AddSwagger()
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
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./v1/swagger.json", "TeaAsAService");
            });
        }
    }
}
