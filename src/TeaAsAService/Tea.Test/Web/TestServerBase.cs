using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Tea.Web;

namespace Tea.Test.Web
{
    public class TestServerBase : IDisposable
    {
        public TestServer TestServer { get; }
        public HttpClient Client { get; }

        public TestServerBase()
        {
            var builder = new WebHostBuilder()
            .UseContentRoot(@"..\..\..\..\Tea.Web")             // refer to the API in the bin folder
            .ConfigureAppConfiguration((builderContext, config) =>
            {
                config.Sources.Clear();
                // specify the test Server uses which configuration json file.
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .UseEnvironment("Development")
            .UseStartup<Startup>();             //point to the web startup class

            TestServer = new TestServer(builder);
            Client = TestServer.CreateClient();
            Console.WriteLine("Test Server Started.");
        }

        public void Dispose()
        {
            Client.Dispose();
            TestServer.Dispose();
        }
    }
}
