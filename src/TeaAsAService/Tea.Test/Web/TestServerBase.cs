using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Tea.Web;
using Xunit;

namespace Tea.Test.Web
{

    [CollectionDefinition("FixtureCollection")]
    public class FixtureCollection : ICollectionFixture<TestServerBase>
    {
    }

    public class TestServerBase : IDisposable
    {
        public TestServer TestServer { get; }
        public HttpClient Client { get; }

        private readonly string HttpAuthUserName = @"7EmMT6n3f0i/YniN6osJXQ==";
        private readonly string HttpAuthPassword = @"TestPassword123*";
        public static string DotNetEnvironment = "Test";

        public TestServerBase()
        {
            var builder = new WebHostBuilder()
            .UseContentRoot(@"../../../../Tea.Web")             // refer to the API in the bin folder
            .ConfigureAppConfiguration((builderContext, config) =>
            {
                config.Sources.Clear();
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{DotNetEnvironment}.json", optional: false, reloadOnChange: false);
            })
            .UseEnvironment(DotNetEnvironment)
            .UseStartup<Startup>();             //point to the web startup class

            TestServer = new TestServer(builder);
            Client = SetupHttpHeaders(TestServer.CreateClient(), HttpAuthUserName, HttpAuthPassword);

            Console.WriteLine("Test Server Started.");
        }

        public HttpClient SetupHttpHeaders(HttpClient client, string username, string password)
        {
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes(
                       $"{username}:{password}")));

            return client;
        }

        public void Dispose()
        {
            Client.Dispose();
            TestServer.Dispose();
        }
    }
}
