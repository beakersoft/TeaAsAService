using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tea.Core.Data;
using Tea.Core.Domain;
using Tea.Web.Models;
using Xunit;

namespace Tea.Test.Model
{
    public class RoundModelFixture
    {
        private readonly Mock<IDataStore> _dataStore;

        public RoundModelFixture()
        {
            _dataStore = new Mock<IDataStore>();
            _dataStore.Setup(x => x.GetUserBySimpleIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
        }

        [Fact]
        public async Task ValidateRound_Validates()
        {
            var model = DummyRoundModel();

            var result = await model.ValidateRound(_dataStore.Object);
            
            Assert.True(result);
            Assert.Empty(model.ValidationMessages);
            _dataStore.VerifyAll();
        }

        [Fact]
        public async Task ValidateRound_DoesNotValidate()
        {
            var model = new RoundModel
            {
                UsersInRound = new List<string>{"userId1"}
            };

            var result = await model.ValidateRound(_dataStore.Object);
            Assert.False(result);
            Assert.Equal(1,model.ValidationMessages.Count);
        }

        [Fact]
        public async Task CreateRoundFromModel_CreatesWithValidModel()
        {
            //run the validate first or it will fail
            var model = DummyRoundModel();
            await model.ValidateRound(_dataStore.Object);

            var res = model.CreateRoundFromModel();

            Assert.NotNull(res);
            Assert.Equal(2,res.UsersInRound.Count);
            Assert.Equal(model.RoundDescription,res.RoundDescription);
        }
        
        private RoundModel DummyRoundModel()
        {
            return new RoundModel
            {
                RoundDescription = "Test Round",
                UsersInRound = new List<string>{"userId1", "userId2"}
            };
        }
    }
}
