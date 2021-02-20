using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Tea.Core.Impl.Data;
using Xunit;

namespace Tea.Test.Web
{
    [CollectionDefinition("DatabaseCollection")]
    public class DatabaseCollection : ICollectionFixture<IntegrationBase>
    {
    }

    public class IntegrationBase
    {
        public Guid TestAuthUserId;
        public string TestAuthSimpleUserId;
        public TestServerBase TestServerBase { get; }

        static IntegrationBase()
        {
            var baseFixture = new TestServerBase();

            //run the database seed
            var context = baseFixture.TestServer.Host.Services.GetRequiredService<TeaContext>();
            DbInitializer.Initialize(context);
        }

        public IntegrationBase()
        {
            TestAuthUserId = Guid.Parse("4f8c49ec-f7a9-487f-bf62-788dea8b095d");
            TestAuthSimpleUserId = Convert.ToBase64String(TestAuthUserId.ToByteArray());
            TestServerBase = new TestServerBase();
        }

        public static async Task<string> PostAndAssert(string url, HttpClient client, bool assertTrue)
        {
            var response = await client.PostAsync(url, null);
            var stringResponse = await response.Content.ReadAsStringAsync();

            if (assertTrue)
                Assert.True(response.IsSuccessStatusCode);
            else
                Assert.False(response.IsSuccessStatusCode);

            return stringResponse;
        }

        public static async Task<string> PostAndAssert<T>(string url, T model, HttpClient client, bool assertTrue)
        {
            var response = await client.PostAsJsonAsync(url, model);
            var stringResponse = await response.Content.ReadAsStringAsync();

            if (assertTrue)
                Assert.True(response.IsSuccessStatusCode);
            else
                Assert.False(response.IsSuccessStatusCode);

            return stringResponse;
        }

        public static async Task<string> PutAndAssert<T>(string url, T model, HttpClient client, bool assertTrue)
        {
            var response = await client.PutAsJsonAsync(url, model);
            var stringResponse = await response.Content.ReadAsStringAsync();

            if (assertTrue)
                Assert.True(response.IsSuccessStatusCode);
            else
                Assert.False(response.IsSuccessStatusCode);

            return stringResponse;

        }

        public static async Task<string> GetAndAssert(string url, HttpClient client, bool assertTrue)
        {
            var response = await client.GetAsync(url);
            var stringResponse = await response.Content.ReadAsStringAsync();

            if (assertTrue)
                Assert.True(response.IsSuccessStatusCode);
            else
                Assert.False(response.IsSuccessStatusCode);

            return stringResponse;
        }
    }
}
