using System;

namespace Tea.Core.Entity
{
    public class History
    {
        public Guid Id { get; set; }
        public virtual User User { get; set; }
        public DateTime HistoryDate { get; set; }
        public int CountForDate { get; set; }
    }
}
