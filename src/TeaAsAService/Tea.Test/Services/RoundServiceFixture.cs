using Moq;
using System;
using System.Threading.Tasks;
using Tea.Core.Data;
using Tea.Core.Domain;
using Tea.Core.Impl.Services;
using Tea.Test.Web;
using Xunit;

namespace Tea.Test.Services
{
    public class RoundServiceFixture : IntegrationBase
    {
        public readonly RoundService _service;
        private readonly Mock<IDataStore> _dataStore;

        public RoundServiceFixture()
        {
            _dataStore = new Mock<IDataStore>();
            _dataStore.Setup(x => x.GetUserBySimpleIdAsync(It.IsAny<string>())).ReturnsAsync(new User());

            _service = new RoundService();
        }

        [Fact]
        public async Task UpdateExistingRound_ReturnsTrueOnValidRound()
        {
            var round = DummyRound();
            
            var res = await _service.UpdateExistingRoundAsync(round, _dataStore.Object,TestAuthSimpleUserId, "Test Round");

            Assert.True(res);
            _dataStore.Verify(x => x.CreateAsync(It.IsAny<RoundDetail>()), Times.Once);
            
        }

        private Round DummyRound()
        {
            return new Round
            {
                Id = Guid.NewGuid()
            };
        }
    }
}
