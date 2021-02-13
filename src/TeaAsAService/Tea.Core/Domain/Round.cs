using System.Collections.Generic;

namespace Tea.Core.Domain
{
    public class Round : BaseDomain
    {
        public Round()
        {
            UsersInRound = new List<RoundUser>();
            // ReSharper disable once VirtualMemberCallInConstructor
            Rounds = new List<RoundDetail>();
        }        

        public virtual ICollection<RoundUser> UsersInRound { get; }
        public string RoundDescription { get; set; }
        public string RoundLocationName { get;set; }
        public virtual ICollection<RoundDetail> Rounds { get; set; }

        public void AddUserToRound(RoundUser user)
        {
            user.Round = this;
            UsersInRound.Add(user);
        }

        public void RemoveUserFromRound(RoundUser user)
        {
            UsersInRound.Remove(user);
        }
    }
}
