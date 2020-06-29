using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tea.Core.Entity
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string SimpleId { get; private set; }
        [Required]
        public string Password { get; private set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string Localization { get; set; }
        [Required]
        public DateTime LastTimeUtc { get; set; }
        public int CurrentDayCount { get; set; }
        public ICollection<History> History { get; set;}

        public History CreateHistoryEntry()
        {
            var entry = new History
            {
                Id = Guid.NewGuid(),
                CountForDate = CurrentDayCount,
                HistoryDate = LastTimeUtc.Date,
                User = this
            };

            CurrentDayCount = 0;

            return entry;
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
                LastTimeUtc = DateTime.UtcNow
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
                LastTimeUtc = DateTime.UtcNow
            };
        }
    }
}
