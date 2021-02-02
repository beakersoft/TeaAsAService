using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Quibble.Xunit;
using Tea.Core.Domain;
using Tea.Web.Models;
using Xunit;

namespace Tea.Test.Web
{
    [CollectionDefinition("IntegrationApiCollection")]
    public class RoundControllerIntegrationFixture : IntegrationBase
    {
        private readonly HttpClient _httpClient;
        private const string RootRoundApiPath = "api/round";
        private const string RootBrewApiPath = "api/brew";

        public RoundControllerIntegrationFixture()
        {
            _httpClient = TestServerBase.Client;
        }

        [Fact]
        public async Task CreateNewRound_ReturnsRoundSummary()
        {
            //call the create user so we have 2 people to add to the round
            var createUserResponse = await PostAndAssert($"{RootBrewApiPath }/newpersonhadbrew", _httpClient, true);
            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(createUserResponse);

            var response = await PostAndAssert($"{RootRoundApiPath }/new", DummyRoundModel(user.SimpleId), _httpClient,true);

            JsonAssert.EqualOverrideDefault(@"
{""roundDescription"":""Integration Test Round"",
""allTimeTotalRounds"":0,""roundsToday"":0}"
                , response
                , new JsonDiffConfig(true)
            );
        }


        private RoundModel DummyRoundModel(string userId)
        {
            return new RoundModel
            {
                UsersInRound = new List<string>{"7EmMT6n3f0i/YniN6osJXQ==", userId},
                RoundDescription = "Integration Test Round",
                RoundLocationName = "Bespin Feasting Table"
            };
        }
    }
}
