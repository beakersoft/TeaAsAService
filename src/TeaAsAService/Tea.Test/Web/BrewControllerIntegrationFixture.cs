using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Dynamic;
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
        private HttpClient _httpClient;
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
            var converter = new ExpandoObjectConverter();
            dynamic user = JsonConvert.DeserializeObject<ExpandoObject>(createUserResponse, converter);

            Assert.NotNull(user.simpleId);
            Assert.NotNull(user.password);
            Assert.NotNull(user.id);

            var hadBrewModel = new UserHadBrewModel
            {
                Id = Guid.Parse(user.id)
            };

            //update the auth headers for our new person
            _httpClient = TestServerBase.SetupHttpHeaders(_httpClient, user.simpleId, user.password);

            var updatebrewCountResponse = await PostAndAssert($"{RootApiPath}/hadbrew", hadBrewModel, _httpClient, true);
            user = JsonConvert.DeserializeObject<User>(updatebrewCountResponse);

            Assert.NotNull(user?.SimpleId);
            Assert.Equal(2, user.CurrentDayCount);
        }
    }
}
