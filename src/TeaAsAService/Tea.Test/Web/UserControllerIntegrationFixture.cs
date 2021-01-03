using Quibble.Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using Tea.Web.Models;
using Xunit;

namespace Tea.Test.Web
{
    [CollectionDefinition("IntegrationApiCollection")]
    public class UserControllerIntegrationFixture : IntegrationBase
    {
        private readonly HttpClient _httpClient;

        public UserControllerIntegrationFixture()
        {
            _httpClient = TestServerBase.Client;
        }

        [Fact]
        public async Task Get_ReturnsUserObjectWithValidUserId()
        {
            var response = await GetAndAssert($"api/user?id={TestAuthSimpleUserId}", _httpClient, true);

            JsonAssert.EqualOverrideDefault(@"
{
    ""simpleId"": ""7EmMT6n3f0i/YniN6osJXQ=="",
    ""id"": ""4f8c49ec-f7a9-487f-bf62-788dea8b095d""
}"
                , response
                , new JsonDiffConfig(true)
            );
        }

        [Fact]
        public async Task Get_ReturnsNothingWithInValidUserId()
        {
            await GetAndAssert($"api/user?id=IamNotAUserId", _httpClient, false);
        }

        [Fact]
        public async Task UpdateUser_UpdatesWithValidModel()
        {
            var model = new UserUpdateModel
            {
                SimpleId = TestAuthSimpleUserId,
                EmailAddress = "testusers@domain.com",
                UpdatedBy = "TestUser1"
            };

            var response = await PostAndAssert($"api/user/updateuser", model, _httpClient,true);

            JsonAssert.EqualOverrideDefault(@"
{
    ""simpleId"": ""7EmMT6n3f0i/YniN6osJXQ=="",
    ""emailAddress"": ""testusers@domain.com""
}"
                , response
                , new JsonDiffConfig(true)
            );
        }

        [Fact]
        public async Task UpdateUser_UpdatesWithoutValidSimpleIdThrowsError()
        {
            var model = new UserUpdateModel
            {
                EmailAddress = "testusers@domain.com",
            };

            var response = await PostAndAssert($"api/user/updateuser", model, _httpClient,false);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task UpdateUser_UpdatesWithoutValidModelThrowsError()
        {
            var model = new UserUpdateModel
            {
                SimpleId = "IAMNOTAUSER",
                EmailAddress = "testusers@domain.com",
                UpdatedBy = "TestUser1"
            };

            var response = await PostAndAssert($"api/user/updateuser", model, _httpClient, false);

            Assert.NotNull(response);
        }
    }
}
