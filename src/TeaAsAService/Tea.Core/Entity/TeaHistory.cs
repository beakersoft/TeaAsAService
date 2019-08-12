using System;

namespace Tea.Core.Entity
{
    public class TeaHistory
    {
        public Guid UserId { get; set; }
        public DateTime HistoryDate { get; set; }
        public int DateCount { get; set; }
    }
}
