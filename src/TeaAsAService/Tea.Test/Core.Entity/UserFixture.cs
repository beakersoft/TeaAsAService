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
            var user = DummyUser(DateTime.UtcNow.AddHours(-1), noOfBrews);
           
            var historyEntry = user.CreateHistoryEntry();

            Assert.Equal(noOfBrews, historyEntry.CountForDate);
            Assert.Equal(0, user.CurrentDayCount);
        }

        [Fact]
        public void UpdateBrewCount_UpdatesForCurrentDay()
        {
            var user = DummyUser(DateTime.UtcNow.AddMinutes(-1), 1);

            var res = user.UpdateBrewCount();

            Assert.Null(res);
            Assert.Equal(2, user.CurrentDayCount);
        }

        [Fact]
        public void UpdateBrewCount_CreatesHistoryEntryForFirstBrewOfDay()
        {
            var noOfBrews = 2;
            var user = DummyUser(DateTime.UtcNow.AddDays(-2), noOfBrews);

            var res = user.UpdateBrewCount();

            Assert.NotNull(res);
            Assert.Equal(noOfBrews, res.CountForDate);
            Assert.Equal(1, user.CurrentDayCount);
        }

        private User DummyUser(DateTime lastTime, int dayCount)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                LastBrewTimeUtc = lastTime,
                CurrentDayCount = dayCount
            };
        }
    }
}
