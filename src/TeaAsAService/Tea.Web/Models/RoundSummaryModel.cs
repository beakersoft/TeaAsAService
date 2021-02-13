using System;
using System.Collections.Generic;
using System.Linq;
using Tea.Core.Domain;

namespace Tea.Web.Models
{
    public class RoundSummaryModel
    {
        public Guid Id { get; set; }
        public string RoundDescription {get; private set; }
        public List<string> UsersInRound { get; private set;}
        public string RoundLocationName { get; private set; }
        public string LastRoundBy { get; private set; }

        public static RoundSummaryModel FromRound(Round round)
        {
            return new RoundSummaryModel
            {
                Id = round.Id,
                RoundDescription = round.RoundDescription,
                RoundLocationName = round.RoundLocationName,
                UsersInRound = round.UsersInRound.Select(x =>x.User.UserName).ToList(),
                LastRoundBy = round.Rounds.LastOrDefault()?.RoundBy?.User?.UserName
            };
        }
    }
}
