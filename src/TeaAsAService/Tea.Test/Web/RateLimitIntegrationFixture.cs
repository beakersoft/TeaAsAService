using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Tea.Test.Web
{
    [CollectionDefinition("IntegrationApiCollection")]
    public class RateLimitIntegrationFixture : IntegrationBase
    {
        private readonly HttpClient _httpClient;

        public RateLimitIntegrationFixture()
        {
            _httpClient = TestServerBase.Client;
        }

        [Fact]
        public async Task Test_LimitHit_TeaContoller()
        {
            var numberOfTimes = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };            
            var allTasks = numberOfTimes.Select(n => Task.Run(async () =>
            {
                var response = await _httpClient.PostAsync($"api/brew/hadbrew", null);
                return new
                {                    
                    Headers = response.Headers.ToList()
                };
            })).ToList();

            var results = await Task.WhenAll(allTasks);
            var retryHeaders = results.SelectMany(x => x.Headers).Where(x => x.Key == "Retry-After").ToList();

            Assert.Equal(9, retryHeaders.Count);    //9 should have been re-tried only one valid
        }
    }
}
