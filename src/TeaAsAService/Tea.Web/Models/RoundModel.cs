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
        private bool _isRoundValid;
        private const int MinimumUsersPerRound = 2;
        public readonly IList<string> ValidationMessages;
        private readonly ICollection<RoundUser> _domainUsersForRound;

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
            var isValid = true;

            if (UsersInRound.Count() < MinimumUsersPerRound)
            {
                ValidationMessages.Add($"You need at least {MinimumUsersPerRound} people in a round");
                isValid = false;
            }

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

            _isRoundValid = isValid;
            return isValid;
        }

        public Round CreateRoundFromModel()
        {
            if (!_isRoundValid || !_domainUsersForRound.Any())
            {
                ValidationMessages.Add("Please validate the round before creating the domain object");
                return null;
            }

            return new Round
            {
                Id = Guid.NewGuid(),
                CreatedUtc = DateTime.UtcNow,
                RoundDescription = RoundDescription,
                UsersInRound = _domainUsersForRound
            };
        }
    }
}
