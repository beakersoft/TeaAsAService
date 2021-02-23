using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tea.Core.Data;
using Tea.Core.Domain;

namespace Tea.Core.Impl.Services
{
    public class RoundService : IRoundService
    {
        public async Task<bool> UpdateExistingRoundAsync(Round round, IDataStore dataStore,
            string userGettingRound, string roundNotes)
        {
            var roundDoneBy = await dataStore.GetUserBySimpleIdAsync(userGettingRound);

            if (roundDoneBy == null)
                return false;

            var roundDetail = new RoundDetail
            {
                Id = Guid.NewGuid(),
                CreatedUtc = DateTime.UtcNow,
                RoundNotes = roundNotes,
                Round = round,
                RoundBy = new RoundUser
                {
                    Id = Guid.NewGuid(),
                    User = roundDoneBy
                }
            };

            await dataStore.CreateAsync(roundDetail);

            var updatedUsers = await UpdateUsersFromNewRound(round,dataStore);
            updatedUsers.ForEach(x => dataStore.UpdateAsync(x));

            return true;
        }

        private static async Task<List<User>> UpdateUsersFromNewRound(Round round, IDataStore dataStore)
        {
            var updatedUsers = new List<User>();

            foreach (var roundUser  in round.UsersInRound)
            {
                var user = await dataStore.GetAsync<User>(roundUser.User.Id);
                user.UpdateBrewCount();
                updatedUsers.Add(user);
            }

            return updatedUsers;
        }
    }
}
