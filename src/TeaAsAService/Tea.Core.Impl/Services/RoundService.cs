using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tea.Core.Data;
using Tea.Core.Domain;

namespace Tea.Core.Impl.Services
{
    public class RoundService : IRoundService
    {
        private readonly IDataStore _dataStore;

        public RoundService(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<bool> UpdateExistingRoundAsync(Round round, string userGettingRound, string roundNotes)
        {
            var roundDoneBy = await _dataStore.GetUserBySimpleIdAsync(userGettingRound);

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

            await _dataStore.CreateAsync(roundDetail);

            var updatedUsers = await UpdateUsersFromNewRound(round);
            updatedUsers.ForEach(x => _dataStore.UpdateAsync(x));

            return true;
        }

        private async Task<List<User>> UpdateUsersFromNewRound(Round round)
        {
            var updatedUsers = new List<User>();

            foreach (var roundUser  in round.UsersInRound)
            {
                var user = await _dataStore.GetAsync<User>(roundUser.User.Id);
                user.UpdateBrewCount();
                updatedUsers.Add(user);
            }

            return updatedUsers;
        }
    }
}
