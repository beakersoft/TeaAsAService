using System.ComponentModel.DataAnnotations;

namespace Tea.Core.Domain
{
    public class History : BaseDomain
    {
        [Required]
        public virtual User User { get; set; }
        public int CountForDate { get; set; }
    }
}
