using System;
using System.Collections.Generic;
using System.Text;

namespace Tea.Core.Entity
{
    public class Round
    {
        public Guid Id { get; set; }
        public IEnumerable<User> Users { get; private set; }
        public DateTime LastRoundTime { get; private set; }
        public User LastBrewMaker { get; private set; }
    }
}
