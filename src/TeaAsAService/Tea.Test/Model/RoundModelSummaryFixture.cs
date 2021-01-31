using System;
using System.Collections.Generic;
using Tea.Core.Domain;
using Tea.Web.Models;
using Xunit;

namespace Tea.Test.Model
{
    public class RoundModelSummaryFixture
    {
        [Fact]
        public void FromRound_ReturnsValidModel()
        {
            var round = DummyRound();
            var res = RoundModelSummary.FromRound(round);

            Assert.Equal(2,res.UsersInRound.Count);
            Assert.Equal(round.RoundDescription,res.RoundDescription);
        }

        private Round DummyRound()
        {
            return new Round
            {
                Id = Guid.NewGuid(),
                RoundDescription = "TestRound",
                UsersInRound = new List<RoundUser>
                {
                    new RoundUser
                    {
                        Id = Guid.NewGuid(),
                        User = new User
                        {
                            EmailAddress = "test1@domain1.com"
                        }
                    },
                    new RoundUser
                    {
                        Id = Guid.NewGuid(),
                        User = new User
                        {
                            EmailAddress = "test2@domain2.com"
                        }
                    }
                } 
            };
        }
    }
}
