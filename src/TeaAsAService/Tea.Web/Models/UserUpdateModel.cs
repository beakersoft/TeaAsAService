using System;
using System.ComponentModel.DataAnnotations;
using Tea.Core;
using Tea.Core.Domain;

namespace Tea.Web.Models
{
    public class UserUpdateModel
    {
        [Required]
        public string SimpleId { get; set; }
        /// <summary>
        /// Must have at least one upper case letter, lower case letter and a digit.
        /// </summary>
        [MinLength(8)]
        public string Password { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string Localization { get; set; }
        [Required]
        public string UpdatedBy { get; set; }

        public User UpdateUserFromModel(User user, IPasswordHasher passwordHasher)
        {
            if (!string.IsNullOrEmpty(Password))
                user.SetPassword(Password, passwordHasher);

            if (!string.IsNullOrEmpty(Localization))
                user.Localization = Localization;

            if (!string.IsNullOrEmpty(EmailAddress))
                user.SetEmail(EmailAddress);

            user.LastUpdated = $"{DateTime.UtcNow} by {UpdatedBy}";

            return user;
        }
    }
}
