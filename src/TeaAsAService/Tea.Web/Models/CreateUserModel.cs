using System.ComponentModel.DataAnnotations;

namespace Tea.Web.Models
{
    public class CreateUserModel
    {
        /// <summary>
        /// Must have at least one upper case letter, lower case letter and a digit.
        /// </summary>
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string LocalizedString { get; set; }
    }
}

