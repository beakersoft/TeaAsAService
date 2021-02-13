using System;
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
            var model =DummyRoundModel(new List<string>{"userId1", "userId2"});

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
            var model =DummyRoundModel(new List<string>{"userId1", "userId2"});

            var res = await model.CreateRoundFromModel(_dataStore.Object);

            Assert.NotNull(res);
            Assert.Equal(2,res.UsersInRound.Count);
            Assert.Equal(model.RoundDescription,res.RoundDescription);
        }

        [Fact]
        public async Task UpdateRound_UserMissingFromModelIsRemovedFromRound()
        {
            var userIdList = new List<string> {"YYYYwwwww--", "89798hg--", "8987ggv--"};
            
            var existingRound = new Round
            {
                Id = Guid.NewGuid(),
                RoundDescription = "existing round",
            };

            userIdList.ForEach(x=> existingRound.AddUserToRound(new RoundUser
            {
                Id = Guid.NewGuid(),
                User = new User
                {
                    Id = Guid.NewGuid(),
                    SimpleId = x
                },
            }));

            var model = DummyRoundModel(new List<string> {"YYYYwwwww--","89798hg--"});

            var res = await model.UpdateRound(existingRound, _dataStore.Object);

            Assert.Equal(2,res.UsersInRound.Count);
            Assert.DoesNotContain(res.UsersInRound, x =>x.User.SimpleId == "8987ggv--");
            Assert.Equal(model.RoundDescription,res.RoundDescription);
            Assert.NotNull(res.UpdateEvents);
        }

        [Fact]
        public async Task UpdateRound_NewUserInModelIsCreatedInRound()
        {
            var userIdList = new List<string> {"YYYYwwwww--", "89798hg--"};
            _dataStore.Setup(x => x.GetUserBySimpleIdAsync("8987ggv--")).ReturnsAsync(new User{SimpleId = "8987ggv--"});
            
            var existingRound = new Round
            {
                Id = Guid.NewGuid(),
                RoundDescription = "existing round",
            };

            userIdList.ForEach(x=> existingRound.AddUserToRound(new RoundUser
            {
                Id = Guid.NewGuid(),
                User = new User
                {
                    Id = Guid.NewGuid(),
                    SimpleId = x
                },
            }));

            var model = DummyRoundModel(new List<string> {"YYYYwwwww--","89798hg--","8987ggv--"});

            var res = await model.UpdateRound(existingRound, _dataStore.Object);

            Assert.Equal(3,res.UsersInRound.Count);
            Assert.Contains(res.UsersInRound, x =>x.User.SimpleId == "8987ggv--");
            Assert.Equal(model.RoundDescription,res.RoundDescription);
            Assert.NotNull(res.UpdateEvents);
        }

        private RoundModel DummyRoundModel(List<string> usersInRound)
        {
            return new RoundModel
            {
                RoundDescription = "Test Round",
                UsersInRound = usersInRound
            };
        }
    }
}
