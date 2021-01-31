using System.Collections.Generic;

namespace Tea.Core.Domain
{
    public class Round : BaseDomain
    {
        public ICollection<RoundUser> UsersInRound { get; set; }
        public string RoundDescription { get; set; }
    }
}
