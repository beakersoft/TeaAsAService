using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tea.Core.Domain
{
    public class UpdateEvent
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public string ChangeSummary { get; set; }
    }
}
