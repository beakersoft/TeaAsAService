using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Tea.Web.Configuration
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddDynamoDBClient(this IServiceCollection services, IConfiguration config)
        {
            var dynamoDbConfig = config.GetSection("DynamoDb");
            var runLocalDynamoDb = dynamoDbConfig.GetValue<bool>("LocalMode");

            if (runLocalDynamoDb)
            {
                services.AddSingleton<IAmazonDynamoDB>(sp =>
                {
                    var clientConfig = new AmazonDynamoDBConfig
                    {
                        ServiceURL = dynamoDbConfig.GetValue<string>("LocalServiceUrl")
                    };
                    return new AmazonDynamoDBClient(clientConfig);
                });
            }
            else
            {
                services.AddAWSService<IAmazonDynamoDB>();
            }

            return services;
        }

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


    }
}
