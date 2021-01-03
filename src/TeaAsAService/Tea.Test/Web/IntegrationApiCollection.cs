using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Tea.Web;
using Xunit;

namespace Tea.Test.Web
{
    [CollectionDefinition("IntegrationApiCollection")]
    public class IntegrationApiCollection : ICollectionFixture<IntegrationTestWebApplicationFactory>
    {
    }

    public class IntegrationTestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
        }
    }
}
