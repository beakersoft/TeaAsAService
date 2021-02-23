using System.ComponentModel.DataAnnotations;

namespace Tea.Web.Models
{
	public class CreateUserModel
	{
		[Required]
		public string Password { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string LocalizedString { get; set; }
    }
}

