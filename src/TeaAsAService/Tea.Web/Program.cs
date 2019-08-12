using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Tea.Web
{
    public class Program
    {
        public static string ConfiguredEnvironment { get; private set; }

        public static string DotNetEnvironment { get; private set; }

        public static IConfigurationRoot Configuration { get; private set; }

        public static bool IsLocal =>
            string.Equals(ConfiguredEnvironment, "local", StringComparison.OrdinalIgnoreCase);

        public static void Main(string[] args)
        {
            DotNetEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{DotNetEnvironment}.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            ConfiguredEnvironment = string.IsNullOrEmpty(Configuration["Environment"]) ? "local" : Configuration["Environment"];

            Log.Logger = CreateLogger();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static ILogger CreateLogger()
        {
            var loggerConfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()                
                .Enrich.WithProperty("Application", "Tea.Web")
                .Enrich.WithProperty("env", ConfiguredEnvironment)
                .WriteTo.Console()
                
                .MinimumLevel.Information();

            if (IsLocal)
            {
                return loggerConfig
                    .WriteTo.File("./Logs/Tea.Web-{Date}.log", rollingInterval: RollingInterval.Day)                    
                    .CreateLogger();
            }

            return loggerConfig.CreateLogger();

        }
    }
}
