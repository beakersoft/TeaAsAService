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

            var response = await PostAndAssert($"{RootRoundApiPath}/new", DummyRoundModel(user.SimpleId), _httpClient,true);

            JsonAssert.EqualOverrideDefault(@"
{
    ""roundDescription"": ""Integration Test Round"",
    ""roundLocationName"": ""Bespin Feasting Table""
}"
                , response
                , new JsonDiffConfig(true)
            );
        }

        [Fact]
        public async Task EditRound_ReturnsRoundSummary()
        {
            var createUserResponse = await PostAndAssert($"{RootBrewApiPath}/newpersonhadbrew", _httpClient, true);
            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(createUserResponse);

            var response = await PostAndAssert($"{RootRoundApiPath}/new", DummyRoundModel(user.SimpleId), _httpClient,
                true);

            var createdRound = Newtonsoft.Json.JsonConvert.DeserializeObject<RoundSummaryModel>(response);
            
            var roundToEdit = DummyRoundModel(user.SimpleId);
            roundToEdit.Id = createdRound.Id;
            roundToEdit.RoundDescription = "I am an edit";

            var editRoundResponse = await PutAndAssert($"{RootRoundApiPath}/edit", roundToEdit, _httpClient,
                true);

            JsonAssert.EqualOverrideDefault(@"
{
    ""roundDescription"": ""I am an edit"",
    ""roundLocationName"": ""Bespin Feasting Table""
}"
                , editRoundResponse
                , new JsonDiffConfig(true)
            );
        }

        [Fact]
        public async Task HadRound_WithValidModelReturns()
        {
            var createUserResponse = await PostAndAssert($"{RootBrewApiPath}/newpersonhadbrew", _httpClient, true);
            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(createUserResponse);

            var response = await PostAndAssert($"{RootRoundApiPath}/new", DummyRoundModel(user.SimpleId), _httpClient,
                true);

            var createdRound = Newtonsoft.Json.JsonConvert.DeserializeObject<RoundSummaryModel>(response);

            var model = new HadRoundModel
            {
                Id = createdRound.Id,
                UserGettingRound = "7EmMT6n3f0i/YniN6osJXQ==",
                RoundNotes = "Fixture Round"
            };

            var hasRoundResponse = await PostAndAssert($"{RootRoundApiPath}/hadround", model, _httpClient,
                true);

            JsonAssert.EqualOverrideDefault(@"{
    ""roundDescription"": ""Integration Test Round"",
    ""roundLocationName"": ""Bespin Feasting Table""
}"
                , hasRoundResponse
                , new JsonDiffConfig(true)
            );
        }

        [Fact]
        public async Task CreateNewRound_ReturnsErrorAndValidationSummaryOnBadModelData()
        {
            var response = await PostAndAssert($"{RootRoundApiPath}/new",new RoundModel() , _httpClient,false);

            JsonAssert.EqualOverrideDefault(@"{
    ""errors"": {
        ""RoundDescription"": [
            ""The RoundDescription field is required.""
        ],
        ""RoundLocationName"": [
            ""The RoundLocationName field is required.""
        ]
    },
    ""title"": ""One or more validation errors occurred."",
    ""status"": 400
}"
                , response
                , new JsonDiffConfig(true)
            );
        }

        [Fact]
        public async Task CreateNewRound_ReturnsErrorAndValidationSummaryOnRoundValidationFail()
        {
            var response = await PostAndAssert($"{RootRoundApiPath}/new",DummyRoundModel("NotAUser") , _httpClient,false);

            JsonAssert.EqualOverrideDefault(@"{
    ""title"": ""Invalid round creation request"",
    ""status"": 400,
    ""detail"": ""User with id NotAUser was not found"",
    ""instance"": ""/api/round/new""
}"
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
