using System;

namespace Tea.Core
{
    public class ItemUpdated
    {
        public ItemUpdated() { }

        public ItemUpdated(string updatedBy)
        {
            UpdatedBy = updatedBy;
            UpdatedUtc = DateTime.UtcNow;
        }

        public string UpdatedBy { get; set; }
        public DateTime UpdatedUtc { get; set; }
    }
}
