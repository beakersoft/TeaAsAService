using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Tea.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool ValidatePassword(this string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            if (password.Length < 8) return false;
            if (!password.Any(char.IsUpper)) return false;
            if (!password.Any(char.IsLower)) return false;
            if (!password.Any(char.IsDigit)) return false;
            if (!password.Any(char.IsUpper)) return false;
            if (password.All(c => char.IsLetterOrDigit(c))) return false;

            return true;
        }

        public static bool ValidateEmail(this string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }
    }
}
