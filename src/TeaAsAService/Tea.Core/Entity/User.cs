using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tea.Core.Entity
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string SimpleId { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string Localization { get; set; }
        public DateTime LastTimeUtc { get; set; }
        public int CurrentDayCount { get; set; }
        public virtual ICollection<History> History { get;set;}
    }
}
