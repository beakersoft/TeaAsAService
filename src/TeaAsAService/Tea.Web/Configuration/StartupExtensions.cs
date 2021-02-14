using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Tea.Core.Impl.HealthChecks;

namespace Tea.Web.Configuration
{
    public static class StartupExtensions
    {        
        public static IServiceCollection AddApiVersioningConfig(this IServiceCollection services)
        {
            services.AddApiVersioning(o => {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TeaAsAService", Version = "v1" });
            });

            return services;
        }

        public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            return services;
        }

        public static IServiceCollection AddApplicationHealthChecks(this IServiceCollection services, IConfiguration Configuration)
        {
            services
                .AddHealthChecks()
        // Add a health check for a SQL Server database
        .AddCheck(
            "TeasAsAServiceDB-check",
            new DatabaseHealthCheck(Configuration.GetConnectionString("DefaultConnection")),
            HealthStatus.Unhealthy,
            new string[] { "teaAsAServiceDb" });

            return services;
        }
    }
}
