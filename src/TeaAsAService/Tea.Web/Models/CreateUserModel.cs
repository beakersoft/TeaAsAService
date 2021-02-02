using System;
using System.ComponentModel.DataAnnotations;
using Tea.Core.Domain;

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

        public User CreateUserFromModel(User user)
        {
            if (!string.IsNullOrEmpty(Firstname))
                user.Firstname = Firstname;

            if (!string.IsNullOrEmpty(Surname))
                user.Surname = Surname;

            if (!string.IsNullOrEmpty(EmailAddress))
                user.EmailAddress = EmailAddress;

            return user;
        }

    }
}

