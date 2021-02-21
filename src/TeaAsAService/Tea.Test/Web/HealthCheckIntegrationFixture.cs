using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Tea.Test.Web
{
    [CollectionDefinition("IntegrationApiCollection")]
    public class HealthCheckIntegrationFixture : IntegrationBase
    {
        private readonly HttpClient _httpClient;
        private const string RootApiPath = "hc";

        public HealthCheckIntegrationFixture()
        {
            _httpClient = TestServerBase.Client;
        }

        [Fact]
        public async Task HeathCheckReturns_Healthy()
        {
            var response = await GetAndAssert($"{RootApiPath}", _httpClient, true);
            Assert.Equal("Healthy", response);
        }
    }
}
