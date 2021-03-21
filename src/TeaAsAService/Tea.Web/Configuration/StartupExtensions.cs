using System;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NSwag;
using NSwag.Generation.Processors.Security;
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

        public static IServiceCollection AddSwagger(this IServiceCollection services, string version)
        {
            services.AddSwaggerDocument(config =>
            {
                var apiScheme = new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.Basic,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                };

                config.AddSecurity("Basic", Array.Empty<string>(), apiScheme);

                config.OperationProcessors
                        .Add(new AspNetCoreOperationSecurityScopeProcessor("Basic"));

                config.PostProcess = document =>
                {
                    document.Info.Version = version;
                    document.Info.Title = $"TeaAsAService API {version}";
                    document.Info.Description =
                        @"Methods are secured with basic authentication i.e. add an **Authorization** header with the value **Basic XXXX** where XXXX is the base64 encoded version of your username<colon>password

While there is only one version of the API at the moment, versioning is supported with the use of an **x-api-version** header which should contain the numeric version you want to target e.g. 1, 1.1 or 2 etc

Error responses are returned as **application/problem+json** responses as decribed here [https://tools.ietf.org/html/rfc7807#page-3](https://tools.ietf.org/html/rfc7807#page-3) and their appropriate HTTP status codes e.g. 400, 401, 404 etc";
                    
                    document.Info.Contact = new OpenApiContact
                    {
                        Name = "TeaAsAService",
                        Email = "beakersoft@gmail.com",
                        Url = "https://github.com/beakersoft/TeaAsAService"
                    };
                };
            });

            return services;
        }

        public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            return services;
        }

        public static IServiceCollection AddApplicationHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHealthChecks()
        // Add a health check for a My Sql database
        .AddCheck(
            "TeasAsAServiceDB-check",
            new DatabaseHealthCheck(configuration.GetConnectionString("DefaultConnection")),
            HealthStatus.Unhealthy,
            new[] { "teaAsAServiceDb" });

            return services;
        }
    }
}
