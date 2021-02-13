using System.ComponentModel.DataAnnotations;

namespace Tea.Web.Models
{
    public class UserHadBrewModel
    {
        [Required]
        public string UserId { get; set; }
    }
}
