using System;
using System.Collections.Generic;
using System.Text;

namespace Tea.Core.Domain
{
    public class Round : BaseDomain
    {
        public ICollection<User> Users { get; set; }
        public string RoundDescription { get; set; }
    }
}
