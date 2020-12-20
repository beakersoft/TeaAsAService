using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Tea.Core.Domain
{
    public class History : BaseDomain
    {
        [Required]
        public User User { get; set; }
        public int CountForDate { get; set; }
    }
}
