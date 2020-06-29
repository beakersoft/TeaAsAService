using System.ComponentModel.DataAnnotations;

namespace Tea.Web.Models
{
    public class UserHadBrew
    {
        [Required]
        public string UserId { get; set; }
    }
}
