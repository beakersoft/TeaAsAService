using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Tea.Core.Domain
{
    public class History
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public DateTime HistoryDate { get; set; }
        public int CountForDate { get; set; }
    }
}
