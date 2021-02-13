using System.ComponentModel.DataAnnotations;

namespace Tea.Core.Domain
{
    public class RoundUser : BaseDomain
    {
        [Required]
        public virtual User User { get; set; }
        public virtual Round Round { get;set; }
    }
}
