using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tea.Core.Domain
{
    public class User : BaseDomain
    {
        [Required]
        public virtual string SimpleId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password must include at least 8 characters, capital and lowercase letters, at least one number, and a special character.")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [JsonIgnore]
        public virtual string Password { get; private set; }

        [Required]
        [JsonIgnore]
        public virtual string Salt { get; private set; }

        [EmailAddress]
        public virtual string EmailAddress { get; set; }
        public virtual string Firstname { get;set; }
        public virtual string Surname { get;set; }
        public virtual string Localization { get; set; }
        [Required]
        public virtual DateTime LastBrewTimeUtc { get; set; }
        public virtual int CurrentDayCount { get; set; }
        public virtual ICollection<History> History { get; private set; }
        public virtual string LastUpdated { get; set; }

        public string UserName
        {
            get
            {
                if (!string.IsNullOrEmpty(Firstname) && !string.IsNullOrEmpty(Surname))
                    return $"{Firstname} {Surname}";

                return !string.IsNullOrEmpty(EmailAddress) ? EmailAddress : SimpleId;
            }
        }

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

        //TODO we need to store this enyrpyted and also pass a salt in
        public bool SetPassword(string newPassword, IPasswordHasher passwordHasher)
        {
            if (!newPassword.ValidatePassword()) 
                return false;

            var passwordHash = passwordHasher.Hash(newPassword);
            var parts = passwordHash.Split('.');

            Password = parts[2].ToString();
            Salt = parts[1].ToString();

            return true;
        }

        public bool SetEmail(string email)
        {
            if (!email.ValidateEmail()) 
                return false;

            EmailAddress = email;

            return true;
        }
            
        public History UpdateBrewCount()
        {
            History entry = null;

            if (CurrentDayCount > 0 && (DateTime.UtcNow.Subtract(LastBrewTimeUtc).TotalDays >= 1))
                entry = CreateHistoryEntry();

            CurrentDayCount++;
            LastBrewTimeUtc = DateTime.UtcNow;

            return entry;
        }

        public static User CreateNewUser(string localizationString)
        {
            var userId = Guid.NewGuid();
            var simpleId = Convert.ToBase64String(userId.ToByteArray());

            var user =  new User
            {
                Id = userId,
                Localization = localizationString,
                SimpleId = simpleId,
                CurrentDayCount = 1,
                LastBrewTimeUtc = DateTime.UtcNow,
                CreatedUtc = DateTime.UtcNow
            };
            
            return user;
        }

        public static User CreateNewUser(string localizationString, string firstName, string surname)
        {
            var userId = Guid.NewGuid();
            var simpleId = Convert.ToBase64String(userId.ToByteArray());

            return new User
            {
                Id = userId,
                Firstname = firstName,
                Surname = surname,
                Localization = localizationString,
                SimpleId = simpleId,
                CurrentDayCount = 1,
                LastBrewTimeUtc = DateTime.UtcNow,
                CreatedUtc = DateTime.UtcNow
            };
        }

        public static User CreateLocalDevUser(IPasswordHasher passwordHasher)
        {
            var userId = Guid.Parse("4f8c49ec-f7a9-487f-bf62-788dea8b095d");
            var simpleId = Convert.ToBase64String(userId.ToByteArray());
            
            var user = new User
            {
                Id = userId,
                Localization = "en-GB",
                SimpleId = simpleId,
                CurrentDayCount = 1,
                LastBrewTimeUtc = DateTime.UtcNow
            };

            user.SetPassword("TestPassword123*", passwordHasher);

            return user;
        }
    }
}
