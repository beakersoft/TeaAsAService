using System.Linq;

namespace Tea.Core
{
    public static class Extensions
    {
        public static bool ValidatePassword(this string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            if (password.Length < 8) return false;
            if (!password.Any(c => char.IsUpper(c))) return false;
            if (!password.Any(c => char.IsLower(c))) return false;
            if (!password.Any(c => char.IsDigit(c))) return false;
            if (!password.Any(c => char.IsUpper(c))) return false;
            if (!password.Any(c => !char.IsLetterOrDigit(c))) return false;

            return true;
        }
    }
}
