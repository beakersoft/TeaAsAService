using System;
using Tea.Core.Domain;
using Xunit;

namespace Tea.Test.Core.Entity
{
    public class UserFixture
    {        
        [Fact]
        public void NewUserCreates()
        {
            var password = "Password1";
            var locstring = "en-GB";
            var user = User.CreateNewUser(locstring, password);

            Assert.Equal(password, user.Password);
            Assert.Equal(locstring, user.Localization);
        }

        [Fact]
        public void CreateHistoryEntry_ReturnsValid()
        {
            var noOfBrews = 5;

            var dummyUser = new User
            {
                Id = Guid.NewGuid(),
                LastBrewTimeUtc = DateTime.UtcNow.AddHours(-1),
                CurrentDayCount = noOfBrews
            };

            var historyEntry = dummyUser.CreateHistoryEntry();

            Assert.Equal(noOfBrews, historyEntry.CountForDate);
            Assert.Equal(0, dummyUser.CurrentDayCount);
        }
    }
}
