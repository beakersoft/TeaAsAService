using System.Net.Http;
using System.Threading.Tasks;
using Tea.Core.Domain;
using Tea.Web.Models;
using Xunit;

namespace Tea.Test.Web
{
    [CollectionDefinition("IntegrationApiCollection")]
    public class BrewControllerIntegrationFixture : IntegrationBase
    {
        private readonly HttpClient _httpClient;
        private const string RootApiPath = "api/brew";

        public BrewControllerIntegrationFixture()
        {
            _httpClient = TestServerBase.Client;
        }

        [Fact]
        public async Task CreateNewUser_AndAddBrewToUser()
        {
            //call the create user and parse to a user object
            var createUserResponse = await PostAndAssert($"{RootApiPath}/newpersonhadbrew", _httpClient, true);
            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(createUserResponse);

            Assert.NotNull(user?.SimpleId);

            var hadBrewModel = new UserHadBrew
            {
                UserId = user.SimpleId
            };

            //check the result is 2 brews
            var updatebrewCountResponse = await PostAndAssert($"{RootApiPath}/hadbrew", hadBrewModel, _httpClient, true);
            user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(updatebrewCountResponse);

            Assert.NotNull(user?.SimpleId);
            Assert.Equal(2, user?.CurrentDayCount);
        }
    }
}
