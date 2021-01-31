using System;
using System.Collections.Generic;
using System.Linq;
using Tea.Core.Domain;

namespace Tea.Web.Models
{
    public class RoundModelSummary
    {
        public Guid Id { get; set; }
        public string RoundDescription {get; set; }
        public List<string> UsersInRound { get; set; }
        public int AllTimeTotalRounds { get; set; }
        public int RoundsToday { get; set; }

        public static RoundModelSummary FromRound(Round round)
        {
            return new RoundModelSummary
            {
                Id = round.Id,
                RoundDescription = round.RoundDescription,
                UsersInRound = round.UsersInRound.Select(x =>x.User.UserName).ToList()
            };
        }
    }
}
