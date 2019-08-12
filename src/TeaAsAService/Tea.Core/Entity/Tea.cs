using System;

namespace Tea.Core.Entity
{
    public class Tea
    {
        public Guid UserId { get; set; }
        public string EmailAddress { get; set; }
        public DateTime LastTimeLocal { get; set; }
        public DateTime LastTimeUtc { get; set; }
        public int CurrentCount { get; set; }
    }
}
