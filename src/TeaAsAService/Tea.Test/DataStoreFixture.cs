using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Tea.Core;
using Tea.Core.Impl;
using Tea.Core.Impl.Data;
using Xunit;

namespace Tea.Test
{
    public class DataStoreFixture
    {
        public DataStore _dataStore;

        public DataStoreFixture()
        {
            var teaContext = new Mock<TeaContext>();
            var passwordHasher = new Mock<IPasswordHasher>();
            var logger = new Mock<ILogger<DataStore>>();

            _dataStore = new DataStore(teaContext.Object, passwordHasher.Object, logger.Object);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("user", "")]
        [InlineData("", "password")]
        private async Task AuthenticateAsync_ReturnsNullWithEmptyParams(string userName, string password)
        {
            var result = await _dataStore.AuthenticateAsync(userName, password);

            Assert.Null(result);
        }
    }
}
