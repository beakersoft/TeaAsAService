using System;
using System.ComponentModel.DataAnnotations;
using Tea.Core.Domain;

namespace Tea.Web.Models
{
    public class UserUpdateModel
    {
        [Required]
        public string SimpleId { get; set; }
        public string Password { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string Localization { get; set; }
        [Required]
        public string UpdatedBy { get; set; }

        public User UpdateUserFromModel(User user)
        {
            if (!string.IsNullOrEmpty(Password))
                user.SetPassword(Password);

            if (!string.IsNullOrEmpty(Localization))
                user.Localization = Localization;

            if (!string.IsNullOrEmpty(EmailAddress))
                user.SetEmail(EmailAddress);

            user.LastUpdated = $"{DateTime.UtcNow} by {UpdatedBy}";

            return user;
        }
    }
}
