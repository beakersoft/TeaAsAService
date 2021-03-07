using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tea.Core.Data;
using Tea.Core.Domain;

namespace Tea.Web.Models
{
    public class RoundModel
    {
        private readonly ICollection<RoundUser> _domainUsersForRound;
        public bool IsRoundValid;
        private const int MinimumUsersPerRound = 2;
        public readonly IList<string> ValidationMessages;
        public string AllValidationMessages => string.Join(" | ", ValidationMessages); 
        
        public RoundModel()
        {
            UsersInRound = new List<string>();
            ValidationMessages = new List<string>();
            _domainUsersForRound = new List<RoundUser>();
        }

        public Guid? Id { get; set; }

        public IList<string> UsersInRound { get; set; }

        [Required]
        public string RoundDescription { get; set; }

        [Required]
        public string RoundLocationName { get; set; }
       
        public async Task<bool> ValidateRound(IDataStore dataStore)
        {
            var isValid = CheckUsersInRound(UsersInRound.Count);
                       
            foreach (var user in UsersInRound)
            {
                var roundUser = await dataStore.GetUserBySimpleIdAsync(user);

                if (roundUser != null)
                {
                    _domainUsersForRound.Add(new RoundUser
                    {
                        Id = Guid.NewGuid(),
                        CreatedUtc = DateTime.UtcNow,
                        User = roundUser
                    });
                }
                else
                {
                    ValidationMessages.Add($"User with id {user} was not found");
                    isValid = false;
                }
            }

            IsRoundValid = isValid;
            return isValid;
        }

        public async Task<Round> CreateRoundFromModel(IDataStore dataStore)
        {
            if (!await (ValidateRound(dataStore)))
                return null;

            var round = new Round
            {
                Id = Guid.NewGuid(),
                CreatedUtc = DateTime.UtcNow,
                RoundDescription = RoundDescription,
                RoundLocationName = RoundLocationName
            };

            _domainUsersForRound.ToList().ForEach(x=> round.AddUserToRound(x));

            return round;
        }

        public async Task<Round> UpdateRound(Round round,IDataStore dataStore)
        {
            if (!await (ValidateRound(dataStore)))
                return null;
           
            var usersToRemove
                = round.UsersInRound.Where(p => UsersInRound.All(p2 => p2 != p.User.SimpleId)).ToList();
            usersToRemove.ForEach(round.RemoveUserFromRound);

            var usersToAdd
                = UsersInRound.Where(u => round.UsersInRound.All(r => r.User.SimpleId != u)).ToList();
            usersToAdd.ForEach(x=> round.AddUserToRound(_domainUsersForRound.First(y=>y.User.SimpleId == x)));

            if (!CheckUsersInRound(round.UsersInRound.Count))
                return null;

            round.RoundDescription = RoundDescription;
            round.RoundLocationName = RoundLocationName;
            round.UpdateEvents += $"|{DateTime.UtcNow} round updated";

            return round;
        }

        private bool CheckUsersInRound(int roundCount)
        {
            if (roundCount < MinimumUsersPerRound)
            {
                ValidationMessages.Add($"You need at least {MinimumUsersPerRound} people in a round");
                IsRoundValid = false;
                return false;
            }

            return true;
        }
    }
}
