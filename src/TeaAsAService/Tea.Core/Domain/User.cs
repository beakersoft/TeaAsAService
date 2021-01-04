using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tea.Core.Domain
{
    public class User : BaseDomain
    {
        [Required]
        public string SimpleId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password must include at least 8 characters, capital and lowercase letters, at least one number, and a special character.")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Password { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string Localization { get; set; }
        [Required]
        public DateTime LastBrewTimeUtc { get; set; }
        public int CurrentDayCount { get; set; }
        public ICollection<History> History { get; set;}
        public string LastUpdated { get; set; }

        public History CreateHistoryEntry()
        {
            var entry = new History
            {
                Id = Guid.NewGuid(),
                CountForDate = CurrentDayCount,
                CreatedUtc = LastBrewTimeUtc.Date,
                User = this
            };

            CurrentDayCount = 0;

            return entry;
        }

        public void SetPassword(string newPassword)
        {
            if (ValidatePassword(newPassword))
            {
                Password = newPassword;
            }
            else
            {
                Password = null;
            }

        }

        public static User CreateNewUser(string localizationString, string password)
        {
            var userId = Guid.NewGuid();
            var simpleId = Convert.ToBase64String(userId.ToByteArray());

            return new User
            {
                Id = userId,
                Password = password,
                Localization = localizationString,
                SimpleId = simpleId,
                CurrentDayCount = 1,
                LastBrewTimeUtc = DateTime.UtcNow,
                LastUpdated = $"{DateTime.UtcNow} by Test.User"
            };
        }

        public static User CreateLocalDevUser()
        {
            var userId = Guid.Parse("4f8c49ec-f7a9-487f-bf62-788dea8b095d");
            var simpleId = Convert.ToBase64String(userId.ToByteArray());

            return new User
            {
                Id = userId,
                Password = "TestPassword123*",
                Localization = "en-GB",
                SimpleId = simpleId,
                CurrentDayCount = 1,
                LastBrewTimeUtc = DateTime.UtcNow
            };
        }

        static bool ValidatePassword(string password)
        {
            const int MIN_LENGTH = 8;

            if (password == null) throw new ArgumentNullException();

            bool meetsLengthRequirements = password.Length >= MIN_LENGTH;
            bool hasUpperCaseLetter = false;
            bool hasLowerCaseLetter = false;
            bool hasDecimalDigit = false;
            bool hasSpecialCharacter = false;

            if (meetsLengthRequirements)
            {
                foreach (char c in password)
                {
                    if (char.IsUpper(c))
                        hasUpperCaseLetter = true;
                    else if (char.IsLower(c))
                        hasLowerCaseLetter = true;
                    else if (char.IsDigit(c))
                        hasDecimalDigit = true;
                    else if (!char.IsLetterOrDigit(c))
                        hasSpecialCharacter = true;
                }
            }

            bool isValid = meetsLengthRequirements
                        && hasUpperCaseLetter
                        && hasLowerCaseLetter
                        && hasDecimalDigit
                        && hasSpecialCharacter;

            return isValid;

        }
    }
}
