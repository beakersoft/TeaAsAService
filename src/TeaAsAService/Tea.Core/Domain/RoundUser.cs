using System.ComponentModel.DataAnnotations;

namespace Tea.Core.Domain
{
    public class RoundUser : BaseDomain
    {
        [Required]
        public User User { get; set; }
    }
}
