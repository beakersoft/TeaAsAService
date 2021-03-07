using System;
using System.ComponentModel.DataAnnotations;

namespace Tea.Web.Models
{
    public class UserHadBrewModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}
